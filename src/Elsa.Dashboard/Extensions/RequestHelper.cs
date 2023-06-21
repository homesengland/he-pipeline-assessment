namespace Elsa.CustomInfrastructure.Extensions
{
  using System;
  using System.Net;

  public static class RequestHelper
  {
    public static HttpRequestMessage ToHttpRequestMessage(this HttpRequest oldRequest, string elsaServer)
        => new HttpRequestMessage()
            .SetAbsoluteUri(oldRequest, elsaServer)
            .SetContent(oldRequest);

    private static HttpRequestMessage SetAbsoluteUri(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest, string elsaServer)
    {
      var relativeUri = oldRequest.Path.Value!.Replace("/ElsaServer", "");
      var uriString = $"{elsaServer}{relativeUri}";
      newRequestMsg.RequestUri = new Uri(uriString);
      return newRequestMsg;
    }

    private static HttpRequestMessage SetContent(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest)
    {
       newRequestMsg.Content = new StreamContent(oldRequest.Body);
       return newRequestMsg;
    }
  }
}
