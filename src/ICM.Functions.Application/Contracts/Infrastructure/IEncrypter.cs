using Microsoft.Extensions.Logging;

namespace ICM.Functions.Application.Infrastructure
{
    public interface IEncrypter
    {
        public Task EncryptAsync(Stream inputBlob, Stream outputBlob, ILogger logger);
    }
}
