using Domain.Entities;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface ISpecifications<TEnitiy , TKey> where TEnitiy : BaseEntity<TKey>
    {
        // Signature for Property [Expression ==> Where]
        public Expression<Func<TEnitiy , bool>> Criteria { get;}

        // Signature for Prperty [Expressions ==> Include]
        public List<Expression<Func<TEnitiy , object>>> IncludeExpressions { get; }
    }
}
