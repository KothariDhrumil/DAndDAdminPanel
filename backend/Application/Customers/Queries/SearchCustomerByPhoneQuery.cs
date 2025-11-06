using Application.Abstractions.Messaging;
using AuthPermissions.BaseCode.DataLayer.EfCode;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Customers.Queries;

public sealed record SearchCustomerByPhoneQuery(string PhoneNumber) : IQuery<CustomerBasicDto?>;

public sealed record CustomerBasicDto(Guid GlobalCustomerId, string PhoneNumber, string? FirstName, string? LastName);

internal sealed class SearchCustomerByPhoneQueryHandler(AuthPermissionsDbContext db)
    : IQueryHandler<SearchCustomerByPhoneQuery, CustomerBasicDto?>
{
    public async Task<Result<CustomerBasicDto?>> Handle(SearchCustomerByPhoneQuery query, CancellationToken cancellationToken)
    {
        var phone = query.PhoneNumber?.Trim();
        if (string.IsNullOrWhiteSpace(phone))
            return Result.Success<CustomerBasicDto?>(null);

        var customer = await db.CustomerAccounts
            .AsNoTracking()
            .Where(c => c.PhoneNumber == phone)
            .Select(c => new CustomerBasicDto(c.GlobalCustomerId, c.PhoneNumber, c.FirstName, c.LastName))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success<CustomerBasicDto?>(customer);
    }
}
