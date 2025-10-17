using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        //SaveChangesAsync
        Task<int> SaveChangesAsync();

        //2- Method return obj of IGenericRepository
        // new GenericRepository<Product,int>
        // new GenericRepository<Category,int> ...
        IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

    }
}
