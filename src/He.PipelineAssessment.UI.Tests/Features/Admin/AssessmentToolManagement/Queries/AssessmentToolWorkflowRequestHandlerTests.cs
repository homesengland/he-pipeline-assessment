using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows;
using He.PipelineAssessment.UI.Features.Funds.FundsList;
using He.PipelineAssessment.UI.Features.Funds.ViewModels;
using MediatR;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using Xunit;
using AssessmentToolWorkflowDto = He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentToolWorkflows.AssessmentToolWorkflowDto;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Queries
{
    public class AssessmentToolWorkflowRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowsException_GivenRepoReturnsNull(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            AssessmentToolWorkflowQuery request,
            AssessmentToolWorkflowRequestHandler sut
        )
        {
            //Arrange  
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(request.AssessmentToolId)).ReturnsAsync(
            (AssessmentTool?)null);

            //Act
            var result =
                await Assert.ThrowsAsync<ArgumentNullException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal("Value cannot be null. (Parameter 'Assessment not found')", result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Should_ReturnObjectWithWorkflows_WhenAssessmentToolHasNoWorkflows(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowQuery request,
            AssessmentTool assessmentTool,
            FundsListResponse fundOptions,
            AssessmentToolWorkflowRequestHandler sut
        )
        {
            //Arrange  
            assessmentTool.AssessmentToolWorkflows = null;
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(request.AssessmentToolId))
                .ReturnsAsync(assessmentTool);

           mediator.Setup(x => x.Send(It.IsAny<FundsListRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(fundOptions);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(assessmentTool.Id, result.AssessmentToolId);
            Assert.Equal(assessmentTool.Name, result.AssessmentToolName);
            Assert.Empty(result.AssessmentToolWorkflowDtos);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_Should_ReturnObjectWithWorkflows_WhenAssessmentToolHasWorkflows(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            [Frozen] Mock<IAssessmentToolMapper> assessmentToolMapper,
            [Frozen] Mock<IMediator> mediator,
            AssessmentToolWorkflowQuery request,
            AssessmentTool assessmentTool,
            List<AssessmentToolWorkflowDto> assessmentToolWorkflowDtos,
            FundsListResponse fundOptions,
            AssessmentToolWorkflowRequestHandler sut
        )
        {
            //Arrange  
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(request.AssessmentToolId)).ReturnsAsync(assessmentTool);
            assessmentToolMapper.Setup(x => x.AssessmentToolWorkflowsToAssessmentToolDto(It.IsAny<List<AssessmentToolWorkflow>>()))
                .Returns(assessmentToolWorkflowDtos);

            mediator.Setup(x => x.Send(It.IsAny<FundsListRequest>(), It.IsAny<CancellationToken>())).ReturnsAsync(fundOptions);


            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.Equal(assessmentTool.Id, result.AssessmentToolId);
            Assert.Equal(assessmentTool.Name, result.AssessmentToolName);
            Assert.Equal(assessmentToolWorkflowDtos, result.AssessmentToolWorkflowDtos);
        }
    }
}
