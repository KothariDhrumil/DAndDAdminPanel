# Package Version Issue - Action Required

## Issue Description

The project currently references `AuthPermissions.AspNetCore` version **9.0.9**, which **does not exist** in the NuGet repository. The latest available version is **9.0.0**.

## Impact

- ❌ **Build fails** due to package restore errors
- ❌ **Cannot run tests** until resolved
- ❌ **SonarQube analysis blocked** (requires successful build)
- ❌ **CI/CD workflows will fail** on this step

## Root Cause

The code uses properties on the `AuthUser` class that don't exist in version 9.0.0:
- `FirstName`
- `LastName`
- `PhoneNumber`

These properties appear to be available in a newer version (9.0.9) that hasn't been released yet.

Additionally, the code references `RoleTypes.FeatureRole` which doesn't exist in version 9.0.0.

## Affected Files

### Project Files (package references):
- `Application/Application.csproj`
- `AuthPermissions.SqlServer/AuthPermissions.SqlServer.csproj`
- `DealersAndDistributors.Server/DealersAndDistributors.Server.csproj`
- `Domain/Domain.csproj`
- `Infrastructure/Infrastructure.csproj`
- `SharedKernel/SharedKernel.csproj`

### Code Files (using unavailable features):
- `SharedKernel/CommonAdmin/AuthUserDisplay.cs` - Uses FirstName, LastName, PhoneNumber, RoleTypes.FeatureRole

## Possible Solutions

### Option 1: Wait for Package Release (Recommended if imminent)
If AuthPermissions 9.0.9 is being released soon:
1. Wait for the package to be published to NuGet
2. No code changes needed
3. Simply restore packages once available

### Option 2: Downgrade to 9.0.0 (Requires Code Changes)
If you need to build immediately:

1. **Keep package references at 9.0.0** (already attempted, reverted)
2. **Refactor SharedKernel/CommonAdmin/AuthUserDisplay.cs**:
   - Remove usage of `FirstName`, `LastName`, `PhoneNumber` from `AuthUser`
   - These might need to come from a different source or be removed
   - Replace `RoleTypes.FeatureRole` with available enum value
3. **Test thoroughly** to ensure functionality isn't broken

### Option 3: Use Pre-release Package (If Available)
Check if there's a pre-release version:
```bash
# Search for all versions including pre-release
dotnet add package AuthPermissions.AspNetCore --version "*-*"
```

### Option 4: Use Private Package Source
If you have access to a private NuGet feed with version 9.0.9:
1. Add the private source to `nuget.config`
2. Restore packages from that source

## Recommended Action Plan

1. **Contact AuthPermissions maintainers**:
   - Check the [GitHub repository](https://github.com/JonPSmith/AuthPermissions.AspNetCore)
   - Ask about version 9.0.9 release timeline
   - Verify if the features you're using exist in an unreleased version

2. **Choose appropriate solution** based on their response:
   - If releasing soon: Wait
   - If no plans: Downgrade and refactor
   - If available elsewhere: Add that source

3. **Once resolved**:
   - Run `dotnet restore`
   - Run `dotnet build`
   - Run existing tests
   - Execute SonarQube analysis

## Temporary Workaround

To continue development on other parts of the project:
1. Work on the client application (`dealersanddistributors.client`)
2. Focus on documentation
3. Design new features
4. Review and plan for post-build tasks

## Testing After Resolution

Once the package issue is resolved, run these commands to verify:

```bash
# Restore packages
dotnet restore

# Build solution
dotnet build --configuration Release

# Run tests (if any)
dotnet test

# Run SonarQube analysis (requires server setup)
dotnet sonarscanner begin /k:"DealersAndDistributors" /d:sonar.host.url="YOUR_URL" /d:sonar.login="YOUR_TOKEN"
dotnet build
dotnet sonarscanner end /d:sonar.login="YOUR_TOKEN"
```

## Support

If you need assistance with any of these solutions, please:
1. Check the AuthPermissions.AspNetCore documentation
2. Review the package's GitHub issues
3. Contact the package maintainers
4. Consult with your development team lead

---

**Note**: This issue was identified during the SonarQube integration work. All other improvements (validators, code quality fixes, CI/CD setup) are complete and ready to use once this dependency issue is resolved.
