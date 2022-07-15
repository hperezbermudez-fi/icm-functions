using ICM.Functions.Application.Infrastructure;
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

        public async Task EncryptAsync(Stream inputBlob, Stream outputBlob, ILogger logger)
        {
            byte[] publicKeyBytes = Convert.FromBase64String(_publicKeyBase64);
            var publicKey = Encoding.UTF8.GetString(publicKeyBytes);

            using var publicKeyStream = Utils.GenerateStreamFromString(publicKey);
            using var pgp = new PGP(new EncryptionKeys(publicKeyStream))
            {
                CompressionAlgorithm = Org.BouncyCastle.Bcpg.CompressionAlgorithmTag.Zip
            };

            await pgp.EncryptStreamAsync(inputBlob, outputBlob);
        }
    }
}
