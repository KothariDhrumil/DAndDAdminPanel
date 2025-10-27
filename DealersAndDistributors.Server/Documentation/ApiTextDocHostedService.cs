using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DealersAndDistributors.Server.Documentation;

// Generates a plain text API document describing all controllers/actions, their routes,
// HTTP methods, parameters, and request/response models. Runs once at startup.
// Output path: {ContentRoot}/ApiDocs/api-docs.txt
//
// Enhanced: If ApiExplorer doesn't provide response types, a lightweight source-code
// scanner will attempt to infer the success payload type by traversing controller calls
// to known service interfaces and locating usages of Result.Success<T> in the implementation.
public sealed class ApiTextDocHostedService : IHostedService
{
    private readonly IApiDescriptionGroupCollectionProvider _apiExplorer;
    private readonly IWebHostEnvironment _env;

    public ApiTextDocHostedService(
        IApiDescriptionGroupCollectionProvider apiExplorer,
        IWebHostEnvironment env)
    {
        _apiExplorer = apiExplorer;
        _env = env;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            var groups = _apiExplorer.ApiDescriptionGroups.Items
                .OrderBy(g => g.GroupName, StringComparer.OrdinalIgnoreCase)
                .ToList();

            var sb = new StringBuilder();
            sb.AppendLine("API Reference (text)");
            sb.AppendLine($"Generated: {DateTime.UtcNow:u} UTC");
            sb.AppendLine();
            sb.AppendLine("Conventions:");
            sb.AppendLine("- Path: relative to the app root");
            sb.AppendLine("- Auth: [Auth] if authorization is required, [Anon] if anonymous");
            sb.AppendLine("- Params: name (source) : .NET type");
            sb.AppendLine();

            // Build a quick index of interface implementors for later source scanning.
            var sourceIndex = new SourceIndex(_env.ContentRootPath);

            foreach (var group in groups)
            {
                if (group.Items.Count == 0) continue;

                sb.AppendLine(new string('=', 80));
                sb.AppendLine($"Controller: {group.GroupName}");
                sb.AppendLine(new string('=', 80));

                foreach (var api in group.Items.OrderBy(a => a.RelativePath, StringComparer.OrdinalIgnoreCase)
                                                .ThenBy(a => a.HttpMethod, StringComparer.OrdinalIgnoreCase))
                {
                    var cad = api.ActionDescriptor as ControllerActionDescriptor;
                    var controllerName = cad?.ControllerName ?? "UnknownController";
                    var actionName = cad?.ActionName ?? "UnknownAction";
                    var method = api.HttpMethod ?? "*";
                    var path = "/" + (api.RelativePath ?? string.Empty);

                    // Determine authorization
                    var (requiresAuth, allowAnon) = GetAuth(cad);
                    var authTag = allowAnon ? "[Anon]" : (requiresAuth ? "[Auth]" : "[Anon]");

                    sb.AppendLine();
                    sb.AppendLine($"{method} {path}  {authTag}");
                    sb.AppendLine($"  Action: {controllerName}.{actionName}");

                    // Parameters
                    if (api.ParameterDescriptions.Count > 0)
                    {
                        sb.AppendLine("  Params:");
                        foreach (var p in api.ParameterDescriptions)
                        {
                            var src = p.Source?.Id ?? "unknown";
                            var typeName = ToFriendlyName(p.Type ?? p.ModelMetadata?.ModelType);
                            sb.AppendLine($"    - {p.Name} ({src}) : {typeName}");
                        }
                    }
                    else
                    {
                        sb.AppendLine("  Params: none");
                    }

                    // Request body model (if any)
                    var bodyParam = api.ParameterDescriptions.FirstOrDefault(d => d.Source == BindingSource.Body);
                    var requestType = bodyParam?.Type ?? bodyParam?.ModelMetadata?.ModelType;
                    if (requestType != null)
                    {
                        sb.AppendLine("  Request Model:");
                        AppendTypeSchema(sb, requestType, indent: "    ");
                    }

                    // Responses - try ApiExplorer first
                    var hadResponses = false;
                    if (api.SupportedResponseTypes != null && api.SupportedResponseTypes.Count > 0)
                    {
                        sb.AppendLine("  Responses:");
                        foreach (var r in api.SupportedResponseTypes.OrderBy(r => r.StatusCode))
                        {
                            hadResponses = true;
                            var respType = r.Type;
                            var respTypeName = ToFriendlyName(respType);
                            sb.AppendLine($"    - {r.StatusCode} : {respTypeName}");
                            if (respType != null && respType != typeof(void))
                            {
                                AppendTypeSchema(sb, respType, indent: "      ", maxDepth: 2);
                            }
                        }
                    }

                    if (!hadResponses)
                    {
                        // Try to infer from source: service call that produces Result.Success<T>
                        var inferred = TryInferResponseFromSource(cad, sourceIndex);
                        if (inferred != null)
                        {
                            sb.AppendLine("  Responses (inferred):");
                            sb.AppendLine($"    - 200 : {inferred}");
                        }
                        else
                        {
                            sb.AppendLine("  Responses: unspecified");
                        }
                    }
                }
            }

            var outDir = Path.Combine(_env.ContentRootPath, "ApiDocs");
            Directory.CreateDirectory(outDir);
            var outPath = Path.Combine(outDir, "api-docs.txt");
            File.WriteAllText(outPath, sb.ToString());
        }
        catch
        {
            // Swallow errors to avoid impacting app start; this is best-effort documentation.
        }

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

    private static (bool requiresAuth, bool allowAnon) GetAuth(ControllerActionDescriptor? cad)
    {
        if (cad == null) return (false, true);

        bool HasAuth(ICustomAttributeProvider p) => p.GetCustomAttributes(typeof(AuthorizeAttribute), true).Any();
        bool HasAnon(ICustomAttributeProvider p) => p.GetCustomAttributes(typeof(AllowAnonymousAttribute), true).Any();

        var methodInfo = cad.MethodInfo as ICustomAttributeProvider;
        var controllerType = cad.ControllerTypeInfo as ICustomAttributeProvider;

        var allowAnon = (methodInfo != null && HasAnon(methodInfo)) || (controllerType != null && HasAnon(controllerType));
        var requiresAuth = (methodInfo != null && HasAuth(methodInfo)) || (controllerType != null && HasAuth(controllerType));

        return (requiresAuth, allowAnon);
    }

    private static void AppendTypeSchema(StringBuilder sb, Type type, string indent, int depth = 0, int maxDepth = 1, HashSet<Type>? visited = null)
    {
        visited ??= new HashSet<Type>();

        var t = Nullable.GetUnderlyingType(type) ?? type;
        if (IsSimple(t))
        {
            sb.AppendLine($"{indent}- {ToFriendlyName(type)}");
            return;
        }

        if (!visited.Add(t))
        {
            sb.AppendLine($"{indent}- {ToFriendlyName(type)} (…)");
            return;
        }

        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(t) && t != typeof(string))
        {
            var elemType = t.IsArray ? t.GetElementType() : t.GenericTypeArguments.FirstOrDefault() ?? typeof(object);
            sb.AppendLine($"{indent}- array<{ToFriendlyName(elemType)}>");
            if (depth < maxDepth)
            {
                AppendTypeSchema(sb, elemType!, indent + "  ", depth + 1, maxDepth, visited);
            }
            return;
        }

        sb.AppendLine($"{indent}- {t.Name} {{");
        var props = t.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                     .Where(p => p.CanRead)
                     .OrderBy(p => p.Name, StringComparer.OrdinalIgnoreCase);
        foreach (var p in props)
        {
            var propType = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
            var simple = IsSimple(propType);
            if (simple)
            {
                sb.AppendLine($"{indent}  {p.Name}: {ToFriendlyName(p.PropertyType)}");
            }
            else if (depth < maxDepth)
            {
                sb.AppendLine($"{indent}  {p.Name}: {ToFriendlyName(p.PropertyType)}");
                AppendTypeSchema(sb, propType, indent + "    ", depth + 1, maxDepth, visited);
            }
            else
            {
                sb.AppendLine($"{indent}  {p.Name}: {ToFriendlyName(p.PropertyType)} …");
            }
        }
        sb.AppendLine($"{indent}}}");
    }

    private static bool IsSimple(Type t)
    {
        t = Nullable.GetUnderlyingType(t) ?? t;
        return t.IsPrimitive
               || t.IsEnum
               || t == typeof(string)
               || t == typeof(decimal)
               || t == typeof(DateTime)
               || t == typeof(DateTimeOffset)
               || t == typeof(Guid)
               || t == typeof(TimeSpan)
               || t == typeof(Uri);
    }

    private static string ToFriendlyName(Type? t)
    {
        if (t == null) return "void";
        t = Nullable.GetUnderlyingType(t) ?? t;
        if (t.IsGenericType)
        {
            var name = t.Name[..t.Name.IndexOf('`')];
            var args = string.Join(", ", t.GetGenericArguments().Select(ToFriendlyName));
            if (t.IsAssignableTo(typeof(System.Collections.Generic.Dictionary<,>)))
                return $"Dictionary<{args}>";
            if (t.IsAssignableTo(typeof(System.Collections.Generic.IEnumerable<>)))
                return $"IEnumerable<{args}>";
            return $"{name}<{args}>";
        }
        if (t.IsArray) return $"{ToFriendlyName(t.GetElementType())}[]";
        return t.Name;
    }

    private static string? TryInferResponseFromSource(ControllerActionDescriptor? cad, SourceIndex sourceIndex)
    {
        if (cad == null) return null;
        var controllerName = cad.ControllerName;
        var actionName = cad.ActionName;

        var controllerText = sourceIndex.TryGetControllerSource(controllerName);
        if (controllerText == null) return null;

        // Find private readonly field declarations to map service fields to interface types
        var fields = SourceIndex.ParsePrivateReadonlyFields(controllerText);

        // Extract the action method block text
        var methodBlock = SourceIndex.ExtractMethodBlock(controllerText, actionName);
        if (methodBlock == null) return null;

        // Look for awaited service calls inside the method
        var calls = SourceIndex.FindAwaitServiceCalls(methodBlock);
        foreach (var (field, method) in calls)
        {
            if (!fields.TryGetValue(field, out var ifaceType)) continue;
            // Find implementation of the interface
            var implOpt = sourceIndex.FindImplementationOfInterface(ifaceType);
            if (implOpt == null) continue;
            var impl = implOpt.Value;

            // Find method implementation block
            var implMethod = SourceIndex.ExtractMethodBlock(impl.SourceText, method);
            if (implMethod == null) continue;

            // Try to infer Result.Success<T> usage
            var inferred = SourceIndex.InferResultSuccessType(implMethod);
            if (!string.IsNullOrWhiteSpace(inferred))
                return $"Result<{inferred}>";
        }

        return null;
    }

    private sealed class SourceIndex
    {
        private readonly string _root;
        private readonly Dictionary<string, string> _controllerCache = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<(string FilePath, string SourceText)> _allSources;

        public SourceIndex(string root)
        {
            _root = root;
            _allSources = Directory.EnumerateFiles(_root, "*.cs", SearchOption.AllDirectories)
                                   .Select(path => (FilePath: path, SourceText: SafeReadAllText(path)))
                                   .Where(t => t.SourceText != null)
                                   .Select(t => (t.FilePath, t.SourceText!))
                                   .ToList();
        }

        public string? TryGetControllerSource(string controllerName)
        {
            if (_controllerCache.TryGetValue(controllerName, out var cached))
                return cached;

            var file = _allSources.FirstOrDefault(t => t.FilePath.EndsWith($"{controllerName}Controller.cs", StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrEmpty(file.SourceText))
            {
                _controllerCache[controllerName] = file.SourceText;
                return file.SourceText;
            }
            return null;
        }

        public static Dictionary<string, string> ParsePrivateReadonlyFields(string source)
        {
            // Pattern: private readonly IType _field;
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var line in source.Split('\n'))
            {
                var l = line.Trim();
                if (!l.StartsWith("private readonly ", StringComparison.Ordinal)) continue;
                // naive split
                // private readonly IMyService _myService;
                var semi = l.IndexOf(';');
                if (semi < 0) continue;
                var head = l.Substring(0, semi);
                var parts = head.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (parts.Length >= 4)
                {
                    var type = parts[2];
                    var field = parts[3];
                    dict[field] = type;
                }
            }
            return dict;
        }

        public static string? ExtractMethodBlock(string source, string methodName)
        {
            // Find "<methodName>(" then capture until matching braces
            var idx = source.IndexOf(methodName + "(", StringComparison.Ordinal);
            if (idx < 0) return null;
            // Move to start of block '{'
            var braceIdx = source.IndexOf('{', idx);
            if (braceIdx < 0) return null;
            int depth = 0;
            for (int i = braceIdx; i < source.Length; i++)
            {
                if (source[i] == '{') depth++;
                else if (source[i] == '}')
                {
                    depth--;
                    if (depth == 0)
                    {
                        return source.Substring(braceIdx, i - braceIdx + 1);
                    }
                }
            }
            return null;
        }

        public static List<(string field, string method)> FindAwaitServiceCalls(string methodBlock)
        {
            // very simple pattern: await _field.Method(
            var results = new List<(string, string)>();
            var lines = methodBlock.Split('\n');
            foreach (var line in lines)
            {
                var l = line.Trim();
                var awaitIdx = l.IndexOf("await ");
                if (awaitIdx < 0) continue;
                var start = awaitIdx + 6;
                // expect _field.Method(
                var dotIdx = l.IndexOf('.', start);
                if (dotIdx < 0) continue;
                var field = l.Substring(start, dotIdx - start).Trim();
                var parenIdx = l.IndexOf('(', dotIdx + 1);
                if (parenIdx < 0) continue;
                var method = l.Substring(dotIdx + 1, parenIdx - (dotIdx + 1)).Trim();
                if (field.StartsWith("_"))
                    results.Add((field, method));
            }
            return results;
        }

        public (string FilePath, string SourceText)? FindImplementationOfInterface(string interfaceType)
        {
            // search for classes implementing the interface (supports primary ctor syntax too)
            // patterns: "class Name : ..., <interfaceType>" or "class Name( ... ) : <interfaceType>"
            foreach (var t in _allSources)
            {
                var s = t.SourceText;
                if (s.IndexOf("class ", StringComparison.Ordinal) < 0) continue;
                if (s.Contains($": {interfaceType}") || s.Contains($", {interfaceType}"))
                {
                    return t;
                }
            }
            return null;
        }

        public static string? InferResultSuccessType(string methodBlock)
        {
            // Try Result.Success<T>(...) generic capture
            var idx = methodBlock.IndexOf("Result.Success<", StringComparison.Ordinal);
            if (idx >= 0)
            {
                var start = idx + "Result.Success<".Length;
                var end = methodBlock.IndexOf('>', start);
                if (end > start)
                {
                    return methodBlock.Substring(start, end - start).Trim();
                }
            }
            // Try Result.Success(new TypeName(
            idx = methodBlock.IndexOf("Result.Success(", StringComparison.Ordinal);
            if (idx >= 0)
            {
                var newIdx = methodBlock.IndexOf("new ", idx);
                if (newIdx > 0)
                {
                    var start = newIdx + 4;
                    // read until first '(' or whitespace
                    int i = start;
                    while (i < methodBlock.Length && (char.IsLetterOrDigit(methodBlock[i]) || methodBlock[i] == '_' || methodBlock[i] == '.')) i++;
                    var typeName = methodBlock.Substring(start, i - start).Trim();
                    if (!string.IsNullOrEmpty(typeName)) return typeName;
                }
            }
            // Try PagedResult.Success<T>(...)
            idx = methodBlock.IndexOf("PagedResult.Success<", StringComparison.Ordinal);
            if (idx >= 0)
            {
                var start = idx + "PagedResult.Success<".Length;
                var end = methodBlock.IndexOf('>', start);
                if (end > start)
                {
                    return methodBlock.Substring(start, end - start).Trim();
                }
            }
            return null;
        }

        private static string? SafeReadAllText(string path)
        {
            try { return File.ReadAllText(path); } catch { return null; }
        }
    }
}
