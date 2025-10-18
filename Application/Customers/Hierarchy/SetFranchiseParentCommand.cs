using Application.Abstractions.Messaging;
using Application.Abstractions.Persistence;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Hierarchy;

/// <summary>
/// Sets/changes the parent of a tenant customer profile (franchise hierarchy) within the same tenant.
/// Rebuilds materialized path + depth for the node and its subtree (depth <= 5 enforced).
/// </summary>
public sealed class SetFranchiseParentCommand : ICommand
{
    public int TenantId { get; set; }
    public Guid GlobalCustomerId { get; set; }
    public Guid? NewParentGlobalCustomerId { get; set; } // null -> make root

    internal sealed class Handler(ITenantRetailDbContextFactory factory)
        : ICommandHandler<SetFranchiseParentCommand>
    {
        public async Task<Result> Handle(SetFranchiseParentCommand command, CancellationToken ct)
        {
            if (command.GlobalCustomerId == command.NewParentGlobalCustomerId)
                return Result.Failure(Error.Validation("ParentSame", "A customer cannot be its own parent."));

            var db = await factory.CreateAsync(command.TenantId, ct);

            // Load target node
            var node = await db.TenantCustomerProfiles
                .SingleOrDefaultAsync(p => p.GlobalCustomerId == command.GlobalCustomerId, ct);
            if (node == null)
                return Result.Failure(Error.NotFound("CustomerProfileNotFound", "Profile not found."));

            // Build map for subtree updates
            var all = await db.TenantCustomerProfiles
                .Where(p => p.HierarchyPath.Contains(command.GlobalCustomerId.ToString()))
                .ToListAsync(ct);

            // Validate new parent exists (unless root)
            TenantCustomerProfile? parent = null;
            string newParentPath = string.Empty;
            byte newDepthBase = 0;
            if (command.NewParentGlobalCustomerId.HasValue)
            {
                parent = await db.TenantCustomerProfiles.SingleOrDefaultAsync(p => p.GlobalCustomerId == command.NewParentGlobalCustomerId.Value, ct);
                if (parent == null)
                    return Result.Failure(Error.NotFound("ParentNotFound", "Parent profile not found."));

                // Prevent cycle: parent can't be in node's subtree
                if (all.Any(p => p.GlobalCustomerId == command.NewParentGlobalCustomerId.Value))
                    return Result.Failure(Error.Validation("Cycle", "Cannot set a descendant as parent."));

                newParentPath = parent.HierarchyPath;
                newDepthBase = parent.Depth;
            }
            else
            {
                newParentPath = "|"; // root path prefix
                newDepthBase = 0;
            }

            // Old path for subtree replace
            var oldPath = node.HierarchyPath;
            var subtree = await db.TenantCustomerProfiles
                .Where(p => p.HierarchyPath.StartsWith(oldPath))
                .ToListAsync(ct);

            // Update target node
            node.ParentGlobalCustomerId = command.NewParentGlobalCustomerId;
            node.HierarchyPath = newParentPath + node.GlobalCustomerId.ToString() + "|";
            node.Depth = (byte)(newDepthBase + 1);
            node.UpdatedAt = DateTime.UtcNow;

            // Update descendants paths
            foreach (var child in subtree.Where(c => c.GlobalCustomerId != node.GlobalCustomerId))
            {
                child.HierarchyPath = node.HierarchyPath + child.HierarchyPath.Substring(oldPath.Length);
                // Recompute depth = number of GUID separators - 2 (because path starts and ends with |)
                child.Depth = (byte)(child.HierarchyPath.Count(ch => ch == '|') - 2);
                if (child.Depth > 5)
                    return Result.Failure(Error.Validation("DepthExceeded", "Hierarchy depth exceeds maximum (5)."));
                child.UpdatedAt = DateTime.UtcNow;
            }

            // Depth check for node itself
            if (node.Depth > 5)
                return Result.Failure(Error.Validation("DepthExceeded", "Hierarchy depth exceeds maximum (5)."));

            await db.SaveChangesAsync(ct);
            return Result.Success();
        }
    }
}
