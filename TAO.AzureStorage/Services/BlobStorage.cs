using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Specialized;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TAO.AzureStorage.Enums;

namespace TAO.AzureStorage.Services
{
    public class BlobStorage : IBlobStorage
    {
        private readonly BlobServiceClient _blobServiceClient;
        public BlobStorage()
        {
            _blobServiceClient = new BlobServiceClient(ConnectionStrings.AzureStorageConnectionString);
        }

        public string BlobUrl => "https://moongazingstorageaccount.blob.core.windows.net";

        public async Task DeleteAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.DeleteAsync();

        }

        public async Task<Stream> DowloadAsync(string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            var blobClient = containerClient.GetBlobClient(fileName);

            var info = await blobClient.DownloadAsync();

            return info.Value.Content;

        }

        public async Task<List<string>> GetLogAsync(string fileName)
        {
            List<string> logs = new List<string>();

            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.Log.ToString());

            var appendBlobClient = containerClient.GetAppendBlobClient(fileName);

            await appendBlobClient.CreateIfNotExistsAsync();

            var info = await appendBlobClient.DownloadAsync();

            using (StreamReader streamReader = new(info.Value.Content))
            {
                string line = string.Empty;

                while ((line = streamReader.ReadLine()) != null)
                {
                    logs.Add(line);
                }
            }

            return logs;




        }

        public List<string> GetNames(EContainerName eContainerName)
        {
            List<string> blobNames = new List<string>();

            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            var blobs = containerClient.GetBlobs();

            blobs.ToList().ForEach(x=>
            {
                blobNames.Add(x.Name)
            });
            return blobNames;
        }

        public async Task SetLog(string text, string fileName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(EContainerName.Log.ToString());

            var appendBlobClient = containerClient.GetAppendBlobClient(fileName);

            await appendBlobClient.CreateIfNotExistsAsync();

            using (MemoryStream memoryStream = new())
            {
                using (StreamWriter streamWriter = new(memoryStream))
                {
                    streamWriter.Write($"{DateTime.Now}:{text}\n");

                    streamWriter.Flush();

                    memoryStream.Position = 0;
                    await appendBlobClient.AppendBlockAsync(memoryStream);
                }
            }

        }

        public async Task UploadAsync(Stream fileStream, string fileName, EContainerName eContainerName)
        {
            var containerClient = _blobServiceClient.GetBlobContainerClient(eContainerName.ToString());

            await containerClient.CreateIfNotExistsAsync();

            await containerClient.SetAccessPolicyAsync(Azure.Storage.Blobs.Models.PublicAccessType.BlobContainer);

            var blobClient = containerClient.GetBlobClient(fileName);

            await blobClient.UploadAsync(fileStream);
        }
    }
}
