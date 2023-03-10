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
        public async Task<TEntity> Add(TEntity entity)
        {

            var operation = TableOperation.InsertOrMerge(entity);

            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;

        }

        public async Task Delete(string rowKey, string partitionKey)
        {
            var entity = await Get(rowKey, partitionKey);

            var operation = TableOperation.Delete(entity);

            await _cloudTable.ExecuteAsync(operation);
        }

        public async Task<TEntity> Get(string rowKey, string partitionKey)
        {
            var operation = TableOperation.Retrieve<TEntity>(rowKey, partitionKey);

            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }

        public IQueryable<TEntity> GetAll()
        {
            return _cloudTable.CreateQuery<TEntity>().AsQueryable();
        }

        public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> query)
        {

            return _cloudTable.CreateQuery<TEntity>().Where(query);

        }

        public async Task<TEntity> Update(TEntity entity)
        {
            var operation = TableOperation.Replace(entity);

            var execute = await _cloudTable.ExecuteAsync(operation);

            return execute.Result as TEntity;
        }
    }
}
