using Microsoft.Azure.Cosmos.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace TAO.AzureStorage.Services
{
    public class TableStorage<TEntity> : INoSqlStorage<TEntity> where TEntity : TableEntity, new()
    {
        private readonly CloudTableClient _cloudTableClient;
        private readonly CloudTable _cloudTable;
        public TableStorage()
        {
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(ConnectionStrings.AzureStorageConnectionString);

            _cloudTableClient = storageAccount.CreateCloudTableClient();
            _cloudTable = _cloudTableClient.GetTableReference(typeof(TEntity).Name);

            _cloudTable.CreateIfNotExists();
        }
        public Task<TEntity> Add(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public Task Delete(string rowKey, string partitionKey)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Get(string rowKey, string partitionKey)
        {
            throw new NotImplementedException();
        }

        public Task<IQueryable<TEntity>> GetAll()
        {
            throw new NotImplementedException();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {
            throw new NotImplementedException();
        }

        public Task<TEntity> Update(TEntity entity)
        {
            throw new NotImplementedException();
        }
    }
}
