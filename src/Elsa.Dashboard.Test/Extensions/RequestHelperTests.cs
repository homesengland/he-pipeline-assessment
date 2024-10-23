using Elsa.CustomInfrastructure.Extensions;
using Elsa.Dashboard.PageModels;
using He.PipelineAssessment.Tests.Common;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Reflection.PortableExecutable;
using Xunit;

namespace Elsa.Dashboard.Tests.Pages
{
    public class RequestHelperTests
    {
        [Fact]
        public async void RequestHelper_ReturnsHttpRequestMessageWithCorrectParameters_FromHttpRequest()
        {
            // Arrange
            var context = new DefaultHttpContext();
            var request = context.Request;
            var elsaServerUrl = "https://elsa-server-url";
            request.Method= "GET";
            request.Headers.Append("Accept-Language", "en-US,en;q=0.5");
            request.Headers.Append("Accept", "*/*");
            request.Headers.Append("Content-Type", "application/json");
            request.Path = "/ElsaServer/GetWorkflows";
            request.QueryString = new QueryString().Add("Param1", "test1");

            var stream = new MemoryStream();
            var writer = new StreamWriter(stream);
            writer.Write("test body");
            writer.Flush();
            request.Body = stream;
            stream.Position = 0;


            // Act
            var httpRequestMessage = request.ToHttpRequestMessage(elsaServerUrl);

            // Assert
            // Assert request method
            Assert.Equal(request.Method, httpRequestMessage.Method.ToString());

            // Assert request headers
            Assert.Equal(request.Headers.Count()-1, httpRequestMessage.Headers.Count());
            Assert.Equal(new String[] { "en-US", "en; q=0.5" }, httpRequestMessage.Headers.GetValues("Accept-Language"));
            Assert.Equal(new String[] { "*/*" }, httpRequestMessage.Headers.GetValues("Accept"));

            // Assert request content type
            Assert.Equal(new String[] { "application/json" }, httpRequestMessage.Content!.Headers.GetValues("Content-Type"));

            // Assert request uri
            Assert.Equal("https://elsa-server-url/GetWorkflows?Param1=test1", httpRequestMessage.RequestUri!.AbsoluteUri);

            // Assert request content
            var httpRequsetMessageBodyStream = await httpRequestMessage.Content.ReadAsStreamAsync();
            var streamReader = new StreamReader(httpRequsetMessageBodyStream);
            var httpRequsetMessageBody = streamReader.ReadToEnd();
            Assert.Equal("test body", httpRequsetMessageBody);
        }


    }
}
