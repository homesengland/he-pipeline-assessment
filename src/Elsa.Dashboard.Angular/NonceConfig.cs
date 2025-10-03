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
        private const string GovUkSetupKey = "GovUkSetup";
        private const string DataTablesSetupKey = "DataTablesSetup";
        private const string JQuerySetupKey = "JQuerySetup";

    private static RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
        private Dictionary<string, string> nonces = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="NonceConfig"/> class.
        /// </summary>
        public NonceConfig()
        {
            this.Add(ElsaSetupKey);
            this.Add(GovUkSetupKey);
            this.Add(DataTablesSetupKey);
            this.Add(JQuerySetupKey);
    }

        /// <summary>
        /// Gets the nonce for the js enabled script.
        /// </summary>
        public string ElsaSetup => this.Get(ElsaSetupKey);
    public string GovUkSetup => this.Get(GovUkSetupKey);
    public string DataTablesSetup => this.Get(DataTablesSetupKey);
    public string JQuerySetup => this.Get(JQuerySetupKey);

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

      string nonce = BitConverter.ToString(byteArray, 0).Replace("-", string.Empty, false, CultureInfo.InvariantCulture);
      return nonce.Replace(" ", "");
    }
  }
}
