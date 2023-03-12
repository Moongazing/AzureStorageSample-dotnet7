using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAO.AzureStorage.Services.Abstract;

namespace TAO.AzureStorage.Services.Concrete
{
    public class AzQueue : IAzQueue
    {
        private readonly QueueClient _client;
        public AzQueue(string queueName)
        {
            _client = new QueueClient(ConnectionStrings.AzureStorageConnectionsString, queueName);
            _client.CreateIfNotExists();
        }
        public async Task DeleteMessageAsync(string messageId, string popReceipt)
        {
            await _client.DeleteMessageAsync(messageId, popReceipt);
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _client.GetPropertiesAsync();
            if (properties.ApproximateMessagesCount > 0)
            {
                QueueMessage[] queueMessages = await _client.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));
                if (queueMessages.Any())
                {
                    return queueMessages[0];
                }
            }
            return null;

        }

        public async Task SendMessageAsync(string message)
        {
            await _client.SendMessageAsync(message);

        }
    }
}
