using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Common.Exceptions;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.UpdateAssessmentTool;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class UpdateAssessmentToolCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenDependencyThrows
        (
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
             Exception exception,
             UpdateAssessmentToolCommand updateAssessmentToolCommand,
             UpdateAssessmentToolCommandHandler sut
        )
        {
            //Arrange
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(updateAssessmentToolCommand.Id))
                .Throws(exception);

            //Act
            var result =
                await Assert.ThrowsAsync<Exception>(() => sut.Handle(updateAssessmentToolCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exception.Message, result.Message);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_CallsUpdateAssessmentToolRepositoryWithCorrectValues
         (
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
             UpdateAssessmentToolCommand updateAssessmentToolCommand,
             AssessmentTool assessmentTool,
             UpdateAssessmentToolCommandHandler sut
         )
        {
            //Arrange           
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(updateAssessmentToolCommand.Id))
                .ReturnsAsync(assessmentTool);

            //Act
            var result = await sut.Handle(updateAssessmentToolCommand, CancellationToken.None);

            //Assert          
            adminAssessmentToolRepository.Verify(x => x.UpdateAssessmentTool(assessmentTool), Times.Once);
            Assert.Equal(Unit.Value, result);
            adminAssessmentToolRepository.Verify(
              x => x.UpdateAssessmentTool(It.Is<AssessmentTool>(y =>
                  y.Order == updateAssessmentToolCommand.Order &&
                  y.Name == updateAssessmentToolCommand.Name)), Times.Once);
        }


        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowException_GivenAssessmentToolNotFound
        (
             [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
             UpdateAssessmentToolCommand updateAssessmentToolCommand,
             UpdateAssessmentToolCommandHandler sut
        )
        {
            //Arrange
            string exceptionMessage = $"Assessment Tool with Id {updateAssessmentToolCommand.Id} not found";

            adminAssessmentToolRepository.Setup(x => x.GetAssessmentToolById(updateAssessmentToolCommand.Id))
                .ReturnsAsync((AssessmentTool?)null);

            //Act
            var result = await Assert.ThrowsAsync<NotFoundException>(() => sut.Handle(updateAssessmentToolCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exceptionMessage, result.Message);
        }
    }

}
