using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Customers;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Queries
{
    public sealed record GetTenantCustomerProfileQuery(Guid GlobalCustomerId) : IQuery<TenantCustomerDetailProfileDto>;

    public sealed class TenantCustomerDetailProfileDto
    {
        public Guid TenantUserId { get; set; }
        public Guid GlobalCustomerId { get; set; }
        public int TenantId { get; set; }
        public Guid? ParentGlobalCustomerId { get; set; }
        public string HierarchyPath { get; set; } = string.Empty;
        public byte Depth { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Address { get; set; } = string.Empty;
        public double OpeningBalance { get; set; }
        public bool IsActive { get; set; }
        public bool TaxEnabled { get; set; }
        public bool CourierChargesApplicable { get; set; }
        public string GSTNumber { get; set; } = string.Empty;
        public string GSTName { get; set; } = string.Empty;
        public double CreditLimit { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
    }

    internal sealed class GetTenantCustomerProfileQueryHandler(IRetailDbContext db)
        : IQueryHandler<GetTenantCustomerProfileQuery, TenantCustomerDetailProfileDto>
    {
        public async Task<Result<TenantCustomerDetailProfileDto>> Handle(GetTenantCustomerProfileQuery query, CancellationToken ct)
        {
            var profile = await db.TenantCustomerProfiles.AsNoTracking()
                .Where(x => x.GlobalCustomerId == query.GlobalCustomerId)
                .Select(x => new TenantCustomerDetailProfileDto
                {
                    TenantUserId = x.TenantUserId,
                    GlobalCustomerId = x.GlobalCustomerId,
                    TenantId = x.TenantId,
                    ParentGlobalCustomerId = x.ParentGlobalCustomerId,
                    HierarchyPath = x.HierarchyPath,
                    Depth = x.Depth,
                    OpeningBalance = x.OpeningBalance,
                    Address = x.Address,
                    IsActive = x.IsActive,
                    TaxEnabled = x.TaxEnabled,
                    CourierChargesApplicable = x.CourierChargesApplicable,
                    GSTNumber = x.GSTNumber,
                    GSTName = x.GSTName,
                    CreditLimit = x.CreditLimit,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhoneNumber = x.PhoneNumber,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt,

                })
                .SingleOrDefaultAsync(ct);
            return profile == null
                ? Result.Failure<TenantCustomerDetailProfileDto>(Error.NotFound("ProfileNotFound", "Tenant customer profile not found."))
                : Result.Success(profile);
        }
    }
}
