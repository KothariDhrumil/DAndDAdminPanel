using Ardalis.Specification;
using Ardalis.Specification.EntityFrameworkCore;
using Application;
using Domain;
using Infrastructure.Persistence.Contexts;
using Mapster;

namespace Infrastructure.Persistence;

// Inherited from Ardalis.Specification's RepositoryBase<T>
public class RetailDbRepository<T> : RepositoryBase<T>, IReadRepository<T>, IRepository<T>
    where T : class, IAggregateRoot
{
    public RetailDbRepository(RetailDbContext dbContext)
        : base(dbContext)
    {
    }

    // We override the default behavior when mapping to a dto.
    // We're using Mapster's ProjectToType here to immediately map the result from the database.
    // This is only done when no Selector is defined, so regular specifications with a selector also still work.
    protected override IQueryable<TResult> ApplySpecification<TResult>(ISpecification<T, TResult> specification) =>
        specification.Selector is not null
            ? base.ApplySpecification(specification)
            : ApplySpecification(specification, false)
                .ProjectToType<TResult>();
}