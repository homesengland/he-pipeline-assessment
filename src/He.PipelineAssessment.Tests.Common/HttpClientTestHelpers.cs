using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;

namespace He.PipelineAssessment.Tests.Common;

public class HttpClientTestHelpers
{
    public static void SetupHttpClientWithExpectedStatusCode<T>(T response,
        HttpStatusCode httpStatusCode, Mock<IHttpClientFactory> httpClientFactoryMock,
        Mock<HttpMessageHandler> httpMessageHandlerMock)
    {
        httpMessageHandlerMock.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = httpStatusCode,
                Content = new StringContent(JsonConvert.SerializeObject(response)),
            });

        var client = new HttpClient(httpMessageHandlerMock.Object);
        client.BaseAddress = new Uri("http://baseUrl");
        httpClientFactoryMock.Setup(_ => _.CreateClient(It.IsAny<string>())).Returns(client);
    }
}