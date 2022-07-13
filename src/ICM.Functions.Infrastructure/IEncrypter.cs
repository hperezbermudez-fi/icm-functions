using ICM.Functions.Infrastructure.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

namespace ICM.Functions.Infrastructure
{
    public interface IEncrypter
    {
        public Task<FileModel> EncryptAsync(IBinder binder, FileModel fileModel, ILogger logger);
    }
}
