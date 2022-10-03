using System.Globalization;
using System.Security.Cryptography;

namespace Elsa.Dashboard
{
    /// <summary>
    /// Nonces used for inline script execution.
    /// </summary>
    public class NonceConfig
    {
        private const string ElsaSetupKey = "ElsaSetup";

        private static RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        private Dictionary<string, string> nonces = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NonceConfig"/> class.
        /// </summary>
        public NonceConfig()
        {
            this.Add(ElsaSetupKey);
        }

        /// <summary>
        /// Gets the nonce for the js enabled script.
        /// </summary>
        public string ElsaSetup => this.Get(ElsaSetupKey);

        /// <summary>
        /// Adds a nonce.
        /// </summary>
        /// <param name="name">The name of the nonce.</param>
        public void Add(string name)
        {
            this.nonces.Add(name, GenerateNonce(8));
        }

        /// <summary>
        /// Gets a nonce by name.
        /// </summary>
        /// <param name="name">The name of the nonce.</param>
        /// <returns>The nonce.</returns>
        public string Get(string name)
        {
            return this.nonces[name];
        }

        private static string GenerateNonce(int byteCount)
        {
            var byteArray = new byte[byteCount];
            randomNumberGenerator.GetBytes(byteArray);

            return BitConverter.ToString(byteArray, 0).Replace("-", string.Empty, false, CultureInfo.InvariantCulture);
        }
    }
}
