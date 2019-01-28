using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AZSync.API.Messaging
{
    public class Publisher : IPublisher
    {
        private readonly CloudBlobContainer container;
        public Publisher()
        {
            CloudStorageAccount cloudStorageAccount = CloudStorageAccount.Parse("DefaultEndpointsProtocol=https;AccountName=synchubaccount;AccountKey=6RbuFRwi1KoVamPWdWY9nviRYo5I/CclrgKXUZzj2OXdUXIQVdl/l+i5HZh1UdKk21/Vq15hLKR9PugA6A2MRw==;EndpointSuffix=core.windows.net");
            CloudBlobClient blobClient = cloudStorageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("syncontainer");
        }

        public async Task PublishBlob<T>(List<T> items)
        {
            string message = JsonConvert.SerializeObject(items, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{DateTime.Now.ToString("dd-MM-yyyy hh-mm tt")}/{Guid.NewGuid().ToString()}.json");
            byte[] byteArray = Encoding.ASCII.GetBytes(message);
            MemoryStream stream = new MemoryStream(byteArray);
            await blockBlob.UploadFromStreamAsync(stream);
        }
    }
}
