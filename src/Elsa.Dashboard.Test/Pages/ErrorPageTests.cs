using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Activities;
using Elsa.Dashboard.PageModels;
using He.PipelineAssessment.Common.Tests;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Elsa.Dashboard.Tests.Pages
{
    public class ErrorPageTests
    {
        [Theory]
        [AutoMoqData]
        public void ErrorPageCorrectlyIdentifiesError_GivenHttpContextError(
                Mock<IFeatureCollection> features,
                Mock<HttpContext> context,
                ILogger<ErrorModel> logger)
        {

            // Arrange
            features.Setup(p => p.Get<IExceptionHandlerPathFeature>()).Returns(new ExceptionHandlerFeature
                    {
                        Path = "/",
                        Error = new HttpRequestException()
                    });
            context.Setup(p => p.Features).Returns(features.Object);
            var pageModel = new ErrorModel(logger);
            var pageContext = new PageContext
            {
                HttpContext = context.Object
            };
            pageModel.PageContext = pageContext;

            // Act
            pageModel.OnGet();

            // Assert
            Assert.Equal("Sorry, there is a problem with the service.", pageModel.ErrorMessage);
            Assert.Equal("Please wait a few moments and try again.", pageModel.AdditionalMessage);
            Assert.True(pageModel.SuggestRetry);
        }
    }
}
