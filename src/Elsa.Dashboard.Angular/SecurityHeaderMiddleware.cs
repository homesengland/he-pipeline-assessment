using System.Diagnostics.CodeAnalysis;

namespace Elsa.Dashboard
{
  public class SecurityHeaderMiddleware
  {
    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    /// <summary>
    /// Initializes a new instance of the <see cref="SecurityHeaderMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next delegate.</param>
    /// <param name="configuration"></param>
    public SecurityHeaderMiddleware(RequestDelegate next, IConfiguration configuration)
    {
      _next = next;
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
      var govUkSetupNonce = $"nonce-{nonceConfig.GovUkSetup}";
      var elsaServer = _configuration["Urls:ElsaServer"];
      var oAuthToken = "https://*.homesengland.org.uk";
      var oAuthToken2 = "https://*.auth0.com";
      var auth0Script = "https://unpkg.com/@auth0/auth0-spa-js@1.1.1/dist/auth0-spa-js.production.esm.js";
      var axios = "https://cdn.jsdelivr.net/npm/axios-middleware@0.3.1/dist/axios-middleware.esm.js";

      var connectSrc = $"connect-src 'self' {elsaServer} {govUkSetupNonce} {oAuthToken} {oAuthToken2} {auth0Script} {axios};";
      var defaultSrc = $"default-src 'self';";
      var scriptSrc = $"script-src 'self' 'strict-dynamic' '{govUkSetupNonce}' '{elsaSetupNonce}' 'unsafe-eval';";
      var styleSrcElem = $"style-src-elem 'self' 'unsafe-inline';";
      var styleSrc = $"style-src 'self' 'unsafe-inline';";
      var imgSrc = $"img-src 'self' data: https://unpkg.com/benteststencil@0.0.16/;";

      var fontSrc = $"font-src 'self';";
      var frameSrc = $"frame-src 'self' {oAuthToken} {axios} {oAuthToken2};";

      SetHeader(context, "Content-Security-Policy", $"{connectSrc} {defaultSrc} {scriptSrc} {styleSrcElem} {styleSrc} {imgSrc} {fontSrc} {frameSrc}");
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
