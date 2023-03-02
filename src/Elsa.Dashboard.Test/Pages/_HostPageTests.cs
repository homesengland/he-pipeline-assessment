using He.PipelineAssessment.Common.Tests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Moq;
using System;
using Xunit;
using Elsa.Dashboard.PageModels;
using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Microsoft.Extensions.Configuration;
using Elsa.CustomWorkflow.Sdk.Models.Activities;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using System.Net.WebSockets;

namespace Elsa.Dashboard.Tests.Pages
{
    public class _HostPageTests
    {
        //[Theory]
        //[AutoMoqData]
        //public async void OnGetRequestsDataOnce_GivenPageLoad(
        //Mock<IElsaServerHttpClient> httpClient,
        //Mock<IConfiguration> config,
        //Mock<IConfigurationSection> mockSection,
        //ILogger<ElsaDashboardLoader> logger,
        //Dictionary<string, HeActivityInputDescriptorDTO> data,
        //string mockUrl
        //    )
        //{

        //    // Arrange
        //    var dataJson = JsonConvert.SerializeObject(data);
        //    mockSection.Setup(c => c.GetValue<string>("ElsaServer")).Returns(mockUrl);
        //    config.Setup(c => c.GetSection("Urls")).Returns(mockSection.Object);
        //    httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
        //    var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger);

        //    // Act
        //    await pageModel.OnGetAsync();

        //    // Assert
        //    Assert.Equal((int)HttpStatusCode.OK, pageModel.HttpContext.Response.StatusCode);
        //}

        //[Theory]
        //[AutoMoqData]
        //public async void OnGetSetsPageData_GivenValuesReturnedWithoutError(
        //Mock<IElsaServerHttpClient> httpClient,
        //Mock<IConfiguration> config,
        //Mock<IConfigurationSection> mockSection,
        //Dictionary<string, HeActivityInputDescriptorDTO> data,
        //ILogger<ElsaDashboardLoader> logger,
        //string mockUrl
        //    )
        //{

        //    // Arrange
        //    var dataJson = JsonConvert.SerializeObject(data);
        //    mockSection.Setup(c => c.GetValue<string>("ElsaServer")).Returns(mockUrl);
        //    config.Setup(c => c.GetSection("Urls")).Returns(mockSection.Object);
        //    httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
        //    var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger);

        //    // Act
        //    await pageModel.OnGetAsync();

        //    // Assert
        //    Assert.Equal(dataJson, pageModel.JsonResponse);
        //}

        //[Theory]
        //[AutoMoqData]
        //public async void OnGetThrowsNullValueError_GivenConfigDoesNotContainValueForUrl(
        //                Mock<IElsaServerHttpClient> httpClient,
        //                Mock<IConfiguration> config,
        //                Mock<IConfigurationSection> mockSection,
        //                ILogger<ElsaDashboardLoader> logger
        //        )
        //{

        //    // Arrange
        //    string value = string.Empty;
        //    config.Setup(c => c.GetValue<string>("ElsaServer")).Returns(value);
        //    //config.Setup(c => c.GetSection("Urls")).Returns(mockSection.Object);
        //    var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger);

        //    // Act


        //    // Assert
        //    await Assert.ThrowsAsync<NullReferenceException>(pageModel.OnGetAsync);
        //}

    }
}
