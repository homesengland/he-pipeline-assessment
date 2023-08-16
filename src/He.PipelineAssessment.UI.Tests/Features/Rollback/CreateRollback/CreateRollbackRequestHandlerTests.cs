using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Features.Rollback.CreateRollback;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Common.Utility;
using Moq;
using Xunit;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.UI.Features.Intervention;

namespace He.PipelineAssessment.UI.Tests.Features.Rollback.CreateRollback
{
   
    public class CreateRollbackRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceCannotBeFound(
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut)
        {
            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to create rollback request. WorkflowInstanceId: {request.WorkflowInstanceId}", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentNotAuthorised(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId))
                .ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(false);

            //Act
            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"You do not have permission to access this resource.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenAssessmentToolWorkflowInstanceIsNotLatestSubmitted(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId))
                .ReturnsAsync(instance);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(false);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to create rollback for Assessment Tool Workflow Instance as this is not the latest submitted Workflow Instance for this Assessment.", ex.Message);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldError_GivenOpenRequestAlreadyExistsForAssessment(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackRequest request,
            CreateRollbackRequestHandler sut,
            List<AssessmentIntervention> assessmentInterventions)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId))
                .ReturnsAsync(instance);
            repository.Setup(x => x.GetOpenAssessmentInterventions(instance.AssessmentId))
            .ReturnsAsync(assessmentInterventions);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);

            //Act
            var ex = await Assert.ThrowsAsync<ApplicationException>(() => sut.Handle(request, CancellationToken.None));

            //Assert
            Assert.Equal($"Unable to create request as an open request already exists for this assessment.", ex.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ShouldReturnAssessmentInterventionDto(
            [Frozen] Mock<IAssessmentRepository> repository,
            [Frozen] Mock<IRoleValidation> roleValidation,
            [Frozen] Mock<IAssessmentToolWorkflowInstanceHelpers> assessmentToolWorkflowInstanceHelpers,
            [Frozen] Mock<IUserProvider> userProvider,
            [Frozen] Mock<IAssessmentInterventionMapper> mapper,
            AssessmentToolWorkflowInstance instance,
            CreateRollbackRequest request,
            List<InterventionReason> reasons,
            AssessmentInterventionDto dto,
            CreateRollbackRequestHandler sut)
        {
            //Arrange
            repository.Setup(x => x.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId))
                .ReturnsAsync(instance);
            repository.Setup(x => x.GetOpenAssessmentInterventions(instance.AssessmentId)).ReturnsAsync(new List<AssessmentIntervention>());
            repository.Setup(x => x.GetInterventionReasons())
                .ReturnsAsync(reasons);

            roleValidation.Setup(x => x.ValidateRole(It.IsAny<int>(), It.IsAny<string>())).ReturnsAsync(true);

            assessmentToolWorkflowInstanceHelpers.Setup(x => x.IsLatestSubmittedWorkflow(instance)).Returns(true);
            userProvider.Setup(x => x.GetUserEmail()).Returns("");
            userProvider.Setup(x => x.GetUserName()).Returns("");

            mapper.Setup(x => x.AssessmentInterventionDtoFromWorkflowInstance(instance, reasons,It.IsAny<DtoConfig>()))
                .Returns(dto);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.IsType<AssessmentInterventionDto>(result);

        }


    }
}
