using Azure.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Storage.Blobs;

namespace DotNetFrameworkUsingManagedIdentityWithTerraform
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //! Install and run terraform
            //! Wait for a couple minutes to allow permissions to sync up in azure

            var blobContainer = Console.ReadLine();
            var blobStorageName = Console.ReadLine();

            var blobService = new BlobService(blobStorageName, blobContainer, Console.ReadLine());

            var blobs = await blobService.ReadBlobs();

            foreach (var blob in blobs)
            {
                
            }
        }
    }

    public class BlobService
    {
        private readonly string _blobContainerName;
        private readonly string _storageAccountName;
        private readonly BlobServiceClient _blobServiceClient;

        public BlobService(string containerName, string storageName, string clientId = null)
        {
            _blobContainerName = containerName;
            _storageAccountName = storageName;

            if (string.IsNullOrWhiteSpace(clientId))
            {
                var credential = new DefaultAzureCredential();
                _blobServiceClient = new BlobServiceClient(new Uri($"https://{_storageAccountName}.blob.core.windows.net"), credential);
            }
            else
            {
                var credential = new ManagedIdentityCredential(clientId);
                _blobServiceClient = new BlobServiceClient(new Uri($"https://{_storageAccountName}.blob.core.windows.net"), credential);
            }
        }

        public async Task ReadBlobs()
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            foreach (var blobItem in blobContainerClient.GetBlobs())
            {
                Console.WriteLine($"Blob Name: {blobItem.Name}");
                await ReadBlob(blobItem.Name);
            }
        }

        public async Task ReadBlob(string blobName)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            var response = await blobClient.DownloadAsync();
            
            using (var streamReader = new StreamReader(response.Value.Content))
            {
                string content = await streamReader.ReadToEndAsync();
                Console.WriteLine(content);
            }
        }

        public async Task WriteBlob(string blobName, string content)
        {
            var blobContainerClient = _blobServiceClient.GetBlobContainerClient(_blobContainerName);
            var blobClient = blobContainerClient.GetBlobClient(blobName);

            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(content)))
            {
                await blobClient.UploadAsync(memoryStream, true);
            }
        }
    }
}
