using Domain.Entities;
using System.Linq.Expressions;
using System.Security.Principal;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,TKey> where TEntity : BaseEntity<TKey>
    {
        //GetAll
        Task<IEnumerable<TEntity>> GetAllAsync( bool asNoTracking = false);
        //GetById
        Task<TEntity?> GetByIdAsync(TKey id);
        //Add
        Task AddAsync(TEntity entity);
        //Delete
        void Delete(TEntity entity);
        //Update
        void Update(TEntity entity);


        #region Specifications

        Task<IEnumerable<TEntity>> GetAllAsync( ISpecifications<TEntity , TKey> specifications );
        Task<TEntity?> GetByIdAsync( ISpecifications<TEntity, TKey> specifications );

        #endregion
    }
}
