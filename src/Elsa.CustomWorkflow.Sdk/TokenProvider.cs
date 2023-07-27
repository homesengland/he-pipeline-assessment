using Elsa.CustomWorkflow.Sdk.Models.Auth;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Elsa.CustomWorkflow.Sdk.TokenProvider;

namespace Elsa.CustomWorkflow.Sdk
{

    public interface ITokenProvider
    {
        public Token? GetToken(bool forceRefresh = false);
    }
    public class TokenProvider : ITokenProvider
    {
        private readonly string domain;
        private readonly string clientId;
        private readonly string clientSecret;
        private readonly string audience;
        private Token? currentToken;

        public TokenProvider(string domain, string clientId, string clientSecret, string audience)
        {
            this.domain = domain;
            this.clientId = clientId;
            this.clientSecret = clientSecret;
            this.audience = audience;
        }

        public string Domain => this.domain;

        public string ClientId => this.clientId;

        public string ClientSecret => this.clientSecret;

        public string Audience => this.audience;


        public Token? GetToken(bool forceRefresh = false)
        {
            if (this.currentToken == null || forceRefresh)
            {
                var client = new RestClient($"https://{this.Domain}/oauth/token");
                var request = new RestRequest("", Method.Post);
                request.AddHeader("content-type", "application/x-www-form-urlencoded");
                request.AddParameter("application/x-www-form-urlencoded", $"grant_type=client_credentials&client_id={this.ClientId}&client_secret={this.ClientSecret}&audience={this.audience}", ParameterType.RequestBody);
                RestResponse response = client.Execute(request);
                if(response != null && response.Content != null)
                {
                    var token = JsonConvert.DeserializeObject<Token>(response.Content);
                    this.currentToken = token;
                }
            }

            return this.currentToken;
        }

    }
}
