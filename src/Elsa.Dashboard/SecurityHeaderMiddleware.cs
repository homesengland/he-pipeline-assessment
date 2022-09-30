using System.Diagnostics.CodeAnalysis;

namespace Elsa.Dashboard
{
    public class SecurityHeaderMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityHeaderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        /// <param name="configuration"></param>
        public SecurityHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            this.next = next;
            _configuration = configuration;
        }

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nonceConfig">The nonce config.</param>
        /// <returns>A task.</returns>
        public async Task Invoke([NotNull] HttpContext context, [NotNull] NonceConfig nonceConfig)
        {
            var elsaSetupNonce = $"nonce-{nonceConfig.ElsaSetup}";
            var elsaStylesNonce = $"nonce-{nonceConfig.ElsaStyles}";
            var elsaServer = _configuration["Urls:ElsaServer"];

            SetHeader(context, "Content-Security-Policy", $"connect-src 'self' {elsaServer}; default-src 'self'; script-src 'self' 'strict-dynamic' '{elsaSetupNonce}' 'unsafe-eval'; style-src-elem 'self'; style-src 'self'; img-src 'self' data:; font-src 'self'");
            SetHeader(context, "X-Frame-Options", "DENY");
            SetHeader(context, "X-Content-Type-Options", "nosniff");
            SetHeader(context, "Referrer-Policy", "strict-origin-when-cross-origin");
            SetHeader(context, "X-Permitted-Cross-Domain-Policies", "none");
            SetHeader(context, "Pragma", "No-cache");
            SetHeader(context, "Cache-control", "No-cache");
            await this.next(context).ConfigureAwait(false);
        }

        private static void SetHeader(HttpContext context, string header, string value)
        {
            var headers = context.Response.Headers;

            if (headers.ContainsKey(header))
            {
                context.Response.Headers[header] = value;
            }
            else
            {
                context.Response.Headers.Add(header, value);
            }
        }
    }
}
