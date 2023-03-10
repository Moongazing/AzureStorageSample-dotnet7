using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAO.AzureStorage
{
    public interface INoSqlStorage<TEntity>
    {
        Task<TEntity> Add(TEntity entity);
        Task Delete(string rowKey, string partitionKey);
        Task<TEntity> Update(TEntity entity);
        Task<TEntity> Get(string rowKey, string partitionKey);
        Task<IQueryable<TEntity>> GetAll();
        IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query);

    }
}
