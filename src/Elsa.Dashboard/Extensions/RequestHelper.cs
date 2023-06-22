namespace Elsa.CustomInfrastructure.Extensions
{
  using System;
  using System.Net;

  public static class RequestHelper
  {
    public static HttpRequestMessage ToHttpRequestMessage(this HttpRequest oldRequest, string elsaServer)
        => new HttpRequestMessage()
            .SetMethod(oldRequest)
            .SetAbsoluteUri(oldRequest, elsaServer)
            .SetHeaders(oldRequest)
            .SetContent(oldRequest)
            .SetContentType(oldRequest);

    private static HttpRequestMessage SetAbsoluteUri(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest, string elsaServer)
    {
      var relativeUri = oldRequest.Path.Value!.Replace("/ElsaServer", "");
      var uriString = $"{elsaServer}{relativeUri}";
      newRequestMsg.RequestUri = new Uri(uriString);
      return newRequestMsg;
    }

    private static HttpRequestMessage SetMethod(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest)
    {
      newRequestMsg.Method = new HttpMethod(oldRequest.Method);
      return newRequestMsg;
    }

    private static HttpRequestMessage SetHeaders(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest)
    {
      foreach(var header in oldRequest.Headers)
      {
        newRequestMsg.Headers.TryAddWithoutValidation(header.Key, header.Value.AsEnumerable());
      }
      return newRequestMsg;
    }

    private static HttpRequestMessage SetContent(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest)
    {
       newRequestMsg.Content = new StreamContent(oldRequest.Body);
       return newRequestMsg;
    }

    private static HttpRequestMessage SetContentType(this HttpRequestMessage newRequestMsg, HttpRequest oldRequest)
    {
      if(oldRequest.Headers.ContainsKey("Content-Type") && newRequestMsg.Content != null)
      {
        newRequestMsg.Content.Headers.Add("Content-Type", oldRequest.ContentType);
      }
      return newRequestMsg;
    }

  }
}
