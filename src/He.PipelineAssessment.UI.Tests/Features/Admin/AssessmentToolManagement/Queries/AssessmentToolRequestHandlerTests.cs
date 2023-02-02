using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessment.AssessmentSummary;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Queries
{
    public class AssessmentToolRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsNull_GivenRepoReturnNull(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            [Frozen] Mock<IAssessmentToolMapper> assessmentToolMapper,
            AssessmentToolRequest request,
            AssessmentToolListData assessmentToolListData,
            AssessmentToolRequestHandler sut
        )
        {
            //Arrange  
            var emptyAssessmentTool = Enumerable.Empty<AssessmentTool>();
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentTools()).ReturnsAsync(emptyAssessmentTool);
            assessmentToolListData = new AssessmentToolListData();
            // assessmentToolMapper.Setup(x => x.AssessmentToolsToAssessmentToolData(emptyAssessmentTool.ToList())).Returns(assessmentToolListData);

            //Act
            var result = await sut.Handle(request, CancellationToken.None);

            //Assert
            //Assert.IsType<AssessmentToolListData>(result);
            Assert.Equal(assessmentToolListData.AssessmentTools, result.AssessmentTools);           
           
        }
    }
}
