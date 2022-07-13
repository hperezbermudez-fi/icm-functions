
using ICM.Functions.Infrastructure;
using ICM.Functions.Infrastructure.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;

namespace ICM.Functions.Durable
{
    [StorageAccount("BlobConnectionString")]
    public class PGPEncrypter
    {
        private readonly IEncrypter _encrypter;
        
        public PGPEncrypter(IEncrypter encrypter)
        {
            _encrypter = encrypter;
        }

        [FunctionName("PGPEncrypter")]
        public static async Task<FileModel> RunOrchestrator(
            [OrchestrationTrigger] IDurableOrchestrationContext context)
        {
            FileModel inputFileModel = context.GetInput<FileModel>();

            FileModel fileModel = await context.CallActivityAsync<FileModel>("PGPEncrypter_Encrypt", inputFileModel);
            return fileModel;
        }

        [FunctionName("PGPEncrypter_Encrypt")]
        public async Task<FileModel> EncryptAsync(
            [ActivityTrigger] FileModel inputFileModel, IBinder binder, ILogger logger)
        {
            var originFile = string.Concat(inputFileModel.CsvFolderName,"/", inputFileModel.CsvFileName);
            var destinationFile = string.Concat(inputFileModel.PgpFolderName, "/", inputFileModel.PgpFileName);

            logger.LogInformation($"Encrypt and save file = '{originFile}' to '{destinationFile}'.");

            return await _encrypter.EncryptAsync(binder, inputFileModel, logger);
        }


        [FunctionName("PGPEncrypter_HttpStart")]
        public  async Task<HttpResponseMessage> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableOrchestrationClient starter, ILogger logger)
        {
            var data = await req.Content.ReadAsAsync<FileModel>();
            // Function input comes from the request content.
            string instanceId = await starter.StartNewAsync("PGPEncrypter", data);
            logger.LogInformation($"Started orchestration with ID = '{instanceId}'.");

            return starter.CreateCheckStatusResponse(req, instanceId);
        }
    }
}
