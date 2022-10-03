﻿using System.Diagnostics.CodeAnalysis;

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
            var elsaServer = _configuration["Urls:ElsaServer"];

            var connectSrc = $"connect-src 'self' {elsaServer};";
            var defaultSrc = $"default-src 'self';";
            var scriptSrc = $"script-src 'self' 'strict-dynamic' '{elsaSetupNonce}' 'unsafe-eval';";
            var styleSrcElem = $"style-src-elem 'self' 'unsafe-inline';";
            var styleSrc = $"style-src 'self' 'unsafe-inline';";
            var imgSrc = $"img-src 'self' data: https://unpkg.com/benteststencil@0.0.16/;";
            var fontSrc = $"font-src 'self';";

            SetHeader(context, "Content-Security-Policy", $"{connectSrc} {defaultSrc} {scriptSrc} {styleSrcElem} {styleSrc} {imgSrc} {fontSrc}");
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
