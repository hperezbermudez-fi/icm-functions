using ICM.Functions.Infrastructure.Models;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using PgpCore;
using System.Text;

namespace ICM.Functions.Infrastructure
{
    public class PGPEncrypter : IEncrypter
    {
        private readonly string _publicKeyBase64;

        public PGPEncrypter(string publicKeyBase64)
        {
            _publicKeyBase64 = publicKeyBase64;
        }

        public async Task<FileModel> EncryptAsync(IBinder binder, FileModel inputFileModel, ILogger logger)
        {
            var origin = $"{inputFileModel.CsvFolderName}/{inputFileModel.CsvFileName}";
            var destination = $"{inputFileModel.PgpFolderName}/{inputFileModel.PgpFileName}";

            logger.LogInformation("Encrypt and save file {input} to {output}", inputFileModel.CsvFileName, inputFileModel.PgpFileName);

            var inputBlobAttribute = new BlobAttribute(origin, FileAccess.Read);
            var outputBlobAttribute = new BlobAttribute(destination, FileAccess.Write);

            byte[] publicKeyBytes = Convert.FromBase64String(_publicKeyBase64);
            var publicKey = Encoding.UTF8.GetString(publicKeyBytes);

            using var inputBlob = await binder.BindAsync<Stream>(inputBlobAttribute);
            using var outputBlob = await binder.BindAsync<Stream>(outputBlobAttribute);
            using var publicKeyStream = Utils.GenerateStreamFromString(publicKey);
            using var pgp = new PGP(new EncryptionKeys(publicKeyStream))
            {
                CompressionAlgorithm = Org.BouncyCastle.Bcpg.CompressionAlgorithmTag.Zip
            };

            await pgp.EncryptStreamAsync(inputBlob, outputBlob);
            logger.LogInformation("Encrypted {input} to {output}", inputFileModel.CsvFileName, inputFileModel.PgpFileName);

            return inputFileModel;
        }
    }
}
