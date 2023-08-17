// [START kms_decrypt_asymmetric]

using Confluent.Kafka;
using Google.Cloud.Kms.V1;
using Google.Protobuf;
using kafkapublisher.Kafka;
using System.Text;

namespace kafkapublisher.Crypt
{
    public class DecryptAsymmetric : IDecryptAsymmetric    
    {
        private readonly string _projID;
        private readonly string _locationId;
        private readonly string _keyRingId;
        private readonly string _keyId;
        private readonly string _keyVersionId;        
        private readonly ICryptoSettings _cryptoSettings;

        public DecryptAsymmetric(IConfiguration config)
        {
            /*
            _cryptoSettings = config?.GetSection("cryptoSettings")?.Get<CryptoSettings>();

            if (_cryptoSettings != null)
            {
                _projID = _cryptoSettings.ProjId;
                _locationId = _cryptoSettings.LocationId;
                _keyRingId = _cryptoSettings.KeyRingId;
                _keyId = _cryptoSettings.KeyId;
                _keyVersionId = _cryptoSettings.KeyVersionId;
            }
            */
            // temp hardcode
            _projID = "my-project";
                _locationId = "us-east1";
                _keyRingId = "my-key-ring";
                _keyId = "my-key";
                _keyVersionId = "123";
        }

        public ICryptoSettings GetConfigSettings()
        {
            return _cryptoSettings;
        }

        public string DecryptAsymmetricString(byte[] ciphertext = null)
        {
            if (string.IsNullOrWhiteSpace(_projID) || string.IsNullOrWhiteSpace(_locationId) || string.IsNullOrWhiteSpace(_keyRingId)
                || string.IsNullOrWhiteSpace(_keyId) || string.IsNullOrWhiteSpace(_keyVersionId))
            {
                throw new ArgumentNullException();
            }

            // Create the client.
            KeyManagementServiceClient client = KeyManagementServiceClient.Create();

            // Build the key version name.
            CryptoKeyVersionName keyVersionName = new CryptoKeyVersionName(_projID, _locationId, _keyRingId, _keyId, _keyVersionId);

            // Call the API.
            AsymmetricDecryptResponse result = client.AsymmetricDecrypt(keyVersionName, ByteString.CopyFrom(ciphertext));

            // Get the plaintext. Cryptographic plaintexts and ciphertexts are
            // always byte arrays.
            byte[] plaintext = result.Plaintext.ToByteArray();

            // Return the result.
            return Encoding.UTF8.GetString(plaintext);
        }
    }
}
// [END kms_decrypt_asymmetric]
