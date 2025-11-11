using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using Moq;
using Xunit;
using He.PipelineAssessment.UI.Features.Funds.FundsList;
using MediatR;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Infrastructure.Repository;
using Microsoft.Extensions.Logging;
using Auth0.ManagementApi.Models;


namespace He.PipelineAssessment.UI.Tests.Features.Funds.FundsList
{
    public class FundsListRequestHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public void FundsListRequest_Should_Inherit_From_Mediator(FundsListRequest fundsListRequest)
        {
            //Arrange 
            //gets the type information of the FundsListRequest class and stores it in the type variable.
            var mediatorType = typeof(IRequest<FundsListResponse>);

            //Act
            //checks if the FundsListRequest class implements or inherits from the IRequest<FundsListResponse> interface
            // stores the result in the result variable.
            var result = fundsListRequest.GetType().IsAssignableTo(mediatorType);

            //Assert
            // checks if the expected value and the actual value are equal.
            Assert.True(result, "FundsListRequest does not inherit from IRequest<FundsListResponse.");
        }

        [Theory]
        [AutoMoqData]
        public void FundsListHandler_Should_Inherit_From_Mediator(FundsListRequestHandler fundsListRequestHandler)
        {
            // Arrange
            var type = fundsListRequestHandler.GetType();

            // Act
            var result = typeof(IRequestHandler<FundsListRequest, FundsListResponse>).IsAssignableFrom(type);

            // Assert
            Assert.True(result, "FundsListRequestHandler does not inherit from IRequestHandler<FundsListRequest, FundsListResponse>");
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Method_Should_Return_List_Of_Funds_When_Repo_Returns_List(
            [Frozen] Mock<IAssessmentRepository> repo,
            FundsListRequest fundsListRequest,
            FundsListRequestHandler sut,
            List<AssessmentFund> assessmentFunds)
        {
            // Arrange
            repo.Setup(x => x.GetAllFunds()).ReturnsAsync(assessmentFunds);

            // Act
            var result = await sut.Handle(fundsListRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<FundsListResponse>(result);
            Assert.Equal(assessmentFunds.Count(), result.Funds.Count());
            Assert.Equal(assessmentFunds.ToString, result.Funds.ToString);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Method_Should_Return_Empty_List_Of_Funds_When_Repo_Returns_Empty_List_Of_Funds(
            [Frozen] Mock<IAssessmentRepository> repo,
            FundsListRequest fundsListRequest,
            FundsListRequestHandler sut)
        {
            // Arrange
            repo.Setup(x => x.GetAllFunds()).ReturnsAsync(new List<AssessmentFund>());

            // Act
            var result = await sut.Handle(fundsListRequest, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result.Funds);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Method_Should_Catch_Exception_If_Repo_Throws_Exception(
            [Frozen] Mock<IAssessmentRepository> repo,
            FundsListRequest fundsListRequest,
            Exception exception,
            FundsListRequestHandler sut)
        {
            // Arrange
            // COMMENT: ThrowsAsync is used to simulate/fake an exception being thrown in asynchronously.
            repo.Setup(x => x.GetAllFunds()).ThrowsAsync(exception);

            // Act 
            var exception = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(fundsListRequest, CancellationToken.None));

            //Assert
            Assert.Equal("Unable to get list of funds.", exception.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Exception_Should_Be_Logged_When_Repo_Throws_Exception(
            [Frozen] Mock<IAssessmentRepository> repo,
            [Frozen] Mock<ILogger<FundsListRequestHandler>> logger,
            FundsListRequest fundsListRequest,
            Exception exception,
            FundsListRequestHandler sut)
        {
            //Arrange
            repo.Setup(x => x.GetAllFunds()).ThrowsAsync(exception);

            // Act
            var exceptionMessage = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(fundsListRequest, CancellationToken.None));

            // Assert
            logger.Verify(
                x => x.Log(
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains(exception.Message)),
                    exception,
                    (Func<It.IsAnyType, Exception?, string>)It.IsAny<object>()),
                Times.Once);

        }
    }
}
