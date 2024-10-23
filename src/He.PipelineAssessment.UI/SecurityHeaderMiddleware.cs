using System.Diagnostics.CodeAnalysis;

namespace He.PipelineAssessment.UI
{
    public class SecurityHeaderMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityHeaderMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next delegate.</param>
        public SecurityHeaderMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// Invoke.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="nonceConfig">The nonce config.</param>
        /// <returns>A task.</returns>
        public async Task Invoke([NotNull] HttpContext context, [NotNull] NonceConfig nonceConfig)
        {
            var govUkSetupNonce = $"nonce-{nonceConfig.GovUkSetup}";
            var dataTablesSetupNonce = $"nonce-{nonceConfig.DataTablesSetup}";

            var connectSrc = $"connect-src 'self';";
            var defaultSrc = $"default-src 'self';";
            var scriptSrc = $"script-src 'self' '{govUkSetupNonce}' '{dataTablesSetupNonce}' 'unsafe-eval';";
            var styleSrcElem = $"style-src-elem 'self';";
            var styleSrc = $"style-src 'self';";
            var imgSrc = $"img-src 'self';";
            var fontSrc = $"font-src 'self';";

            SetHeader(context, "Content-Security-Policy", $"{connectSrc} {defaultSrc} {scriptSrc} {styleSrcElem} {styleSrc} {imgSrc} {fontSrc}");
            SetHeader(context, "X-Frame-Options", "DENY");
            SetHeader(context, "X-Content-Type-Options", "nosniff");
            SetHeader(context, "Referrer-Policy", "strict-origin-when-cross-origin");
            SetHeader(context, "X-Permitted-Cross-Domain-Policies", "none");
            SetHeader(context, "Pragma", "No-cache");
            SetHeader(context, "Cache-control", "No-cache");
            await _next(context).ConfigureAwait(false);
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
                context.Response.Headers.Append(header, value);
            }
        }
    }
}
