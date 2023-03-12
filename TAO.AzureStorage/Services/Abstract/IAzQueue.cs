using Azure.Storage.Queues.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TAO.AzureStorage.Services.Abstract
{
    public interface IAzQueue
    {
        Task SendMessageAsync(string message);
        Task<QueueMessage> RetrieveNextMessageAsync();
        Task DeleteMessageAsync(string messageId, string popReceipt);

    }
}
