using Domain.Contracts;
using Domain.Entities;

namespace Presistence
{
    internal static class SpecificationEvaluater
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> inputQuery,
           ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;

            // Apply Criteria
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            // Apply Sorting
            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.OrderByDescending is not null)
                query = query.OrderByDescending(specifications.OrderByDescending);

            // Apply Includes
            if (specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                //foreach (var exp in specifications.IncludeExpressions)
                //    query = query.Include(exp);

                query = specifications.IncludeExpressions
                    .Aggregate(query, (currentQuery, exp) => currentQuery.Include(exp)); // Using Aggregate to apply all includes
            }

            return query;
        }
    }
}
