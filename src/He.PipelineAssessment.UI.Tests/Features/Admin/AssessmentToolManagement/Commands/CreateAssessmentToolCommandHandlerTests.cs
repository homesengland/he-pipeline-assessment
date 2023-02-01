using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands
{
    public class CreateAssessmentToolCommandHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ThrowExceptoin_GivenDependencyThrows
        (
             [Frozen] Mock<IAssessmentToolMapper> assessmentToolMapper,
             CreateAssessmentToolCommand createAssessmentToolCommand,
             Exception exception,
             CreateAssessmentToolCommandHandler sut 
         )
        {
            //Arrange
            assessmentToolMapper.Setup(x => x.CreateAssessmentToolCommandToAssessmentTool(createAssessmentToolCommand))
                .Throws(exception);           

            //Act
            var result = await Assert.ThrowsAsync<Exception>(() => sut.Handle(createAssessmentToolCommand, CancellationToken.None));

            //Assert          
            Assert.Equal(exception.Message, result.Message);

        }
    }
}
