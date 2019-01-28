using AZSync.API.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AZSyncFnApp
{
    public static class SyncHubBlobTriggerFn
    {
        [FunctionName("SyncHubBlobTriggerFn")]
        public static async Task Run([BlobTrigger("syncontainer/{name}", Connection = "AzureWebJobsStorage")]Stream myBlob, string name, TraceWriter log)
        {
            StreamReader reader = new StreamReader(myBlob);
            //JArray jsonObject = JArray.Parse(await reader.ReadToEndAsync());
            //List<JToken> jTokens = jsonObject.Descendants().Where(p => p.Count() == 0).ToList();
            //Dictionary<string, string> results = jTokens.Aggregate(new Dictionary<string, string>(), (properties, jToken) =>
            //{
            //    properties.Add(jToken.Path, jToken.ToString());
            //    return properties;
            //});

            log.Info(await reader.ReadToEndAsync());
            log.Info($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {myBlob.Length} Bytes");
        }
    }
}
