using Moq;
using Xunit;
using Elsa.Dashboard.PageModels;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Activities;
using Newtonsoft.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Elsa.Dashboard.Models;
using He.PipelineAssessment.Tests.Common;

namespace Elsa.Dashboard.Tests.Pages
{
    public class _HostPageTests
    {
        [Theory]
        [AutoMoqData]
        public async void OnGetRequestsDataOnce_GivenPageLoad(
        Mock<IElsaServerHttpClient> httpClient,
        Mock<IOptions<Urls>> config,
        Mock<IOptions<Auth0Config>> auth0Config,
        Mock<Urls> urlMock,
        ILogger<ElsaDashboardLoader> logger,
        Dictionary<string, HeActivityInputDescriptorDTO> data,
        Dictionary<string, string> dataDictionaryData,
        string mockUrl
            )
        {

            // Arrange
            var dataJson = JsonConvert.SerializeObject(data);
            var dataDictionaryDataJson = JsonConvert.SerializeObject(dataDictionaryData);
            config.SetupGet(o => o.Value).Returns(urlMock.Object);
            urlMock.SetupGet(u => u.ElsaServer).Returns(mockUrl);
            httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
            httpClient.Setup(c => c.LoadDataDictionary(mockUrl, false)).ReturnsAsync(dataDictionaryDataJson);
            var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger, auth0Config.Object);

            // Act
            await pageModel.OnGetAsync();

            // Assert
            Assert.Equal(dataJson, pageModel.JsonResponse);
        }

        [Theory]
        [AutoMoqData]
        public async void OnGetSetsPageData_GivenValuesReturnedWithoutError(
        Mock<IElsaServerHttpClient> httpClient,
        Mock<IOptions<Urls>> config,
        Mock<IOptions<Auth0Config>> auth0Config,
        Mock<Urls> urlMock,
        Dictionary<string, HeActivityInputDescriptorDTO> data,
        Dictionary<string, string> dataDictionaryData,
        ILogger<ElsaDashboardLoader> logger,
        string mockUrl
            )
        {

            // Arrange
            var dataJson = JsonConvert.SerializeObject(data);
            var dataDictionaryDataJson = JsonConvert.SerializeObject(dataDictionaryData);
            config.SetupGet(o => o.Value).Returns(urlMock.Object);
            urlMock.SetupGet(u => u.ElsaServer).Returns(mockUrl);
            httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
            httpClient.Setup(c => c.LoadDataDictionary(mockUrl, false)).ReturnsAsync(dataDictionaryDataJson);
            var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger, auth0Config.Object);

            // Act
            await pageModel.OnGetAsync();

            // Assert
            Assert.Equal(dataJson, pageModel.JsonResponse);
        }

        [Theory]
        [AutoMoqData]
        public async void OnGetThrowsNullValueError_GivenConfigDoesNotContainValueForUrl(
                        Mock<IElsaServerHttpClient> httpClient,
                        Mock<IOptions<Urls>> config,
                        Mock<IOptions<Auth0Config>> auth0Config,
                        Mock<Urls> urlMock,
                        ILogger<ElsaDashboardLoader> logger
                )
        {
            // Arrange
            string value = string.Empty;
            config.SetupGet(o => o.Value).Returns(urlMock.Object);
            urlMock.SetupGet(u => u.ElsaServer).Returns(string.Empty);
            var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger, auth0Config.Object);

            // Act

            // Assert
            await Assert.ThrowsAsync<NullReferenceException>(pageModel.OnGetAsync);
        }

        [Theory]
        [AutoMoqData]
        public async void OnGetSetsDcitionaryData_GivenValuesReturnedWithoutError(
        Mock<IElsaServerHttpClient> httpClient,
        Mock<IOptions<Urls>> config,
        Mock<IOptions<Auth0Config>> auth0Config,
        Mock<Urls> urlMock,
        Dictionary<string, string> dataDictionaryData,
        Dictionary<string, HeActivityInputDescriptorDTO> data,
        ILogger<ElsaDashboardLoader> logger,
        string mockUrl
    )
        {

            // Arrange
            dataDictionaryData.Add("Dictionary", "dataDictionaryResponse");
            var dataJson = JsonConvert.SerializeObject(data);
            var dataDictionaryDataJson = JsonConvert.SerializeObject(dataDictionaryData);
            config.SetupGet(o => o.Value).Returns(urlMock.Object);
            urlMock.SetupGet(u => u.ElsaServer).Returns(mockUrl);
            httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
            httpClient.Setup(c => c.LoadDataDictionary(mockUrl, false)).ReturnsAsync(dataDictionaryDataJson);
            var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger, auth0Config.Object);


            // Act
            await pageModel.OnGetAsync();

            // Assert
            Assert.Equal("dataDictionaryResponse", pageModel.DictionaryResponse);
        }

        [Theory]
        [AutoMoqData]
        public async void OnGetSetsIntellisense_GivenValuesReturnedWithoutError(
        Mock<IElsaServerHttpClient> httpClient,
        Mock<IOptions<Urls>> config,
        Mock<IOptions<Auth0Config>> auth0Config,
        Mock<Urls> urlMock,
        Dictionary<string, string> dataDictionaryData,
        Dictionary<string, HeActivityInputDescriptorDTO> data,
        ILogger<ElsaDashboardLoader> logger,
        string mockUrl
        )
        {
            // Arrange
            dataDictionaryData.Add("Intellisense", "intellisenseResponse");
            var dataJson = JsonConvert.SerializeObject(data);
            var dataDictionaryDataJson = JsonConvert.SerializeObject(dataDictionaryData);
            config.SetupGet(o => o.Value).Returns(urlMock.Object);
            urlMock.SetupGet(u => u.ElsaServer).Returns(mockUrl);
            httpClient.Setup(c => c.LoadCustomActivities(mockUrl)).ReturnsAsync(dataJson);
            httpClient.Setup(c => c.LoadDataDictionary(mockUrl, false)).ReturnsAsync(dataDictionaryDataJson);
            var pageModel = new ElsaDashboardLoader(httpClient.Object, config.Object, logger, auth0Config.Object);


            // Act
            await pageModel.OnGetAsync();

            // Assert
            Assert.Equal("intellisenseResponse", pageModel.IntellisenseResponse);
        }
    }
}
