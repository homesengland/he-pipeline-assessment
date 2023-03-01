using AutoFixture.Xunit2;
using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Queries.GetAssessmentTools;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Queries
{
    public class AssessmentToolRequestHandlerTests
    {
        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsEmptyAssessmentTools_GivenRepoReturnsEmptyList(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            AssessmentToolQuery query,
            AssessmentToolRequestHandler sut
        )
        {
            //Arrange  
            var emptyAssessmentTool = Enumerable.Empty<AssessmentTool>();
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentTools()).ReturnsAsync(emptyAssessmentTool);
            var assessmentToolListData = new AssessmentToolListData();

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(assessmentToolListData.AssessmentTools, result.AssessmentTools);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_GetAssessmentTools_GivenRepoReturnsNonEmptyList(
            [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
            [Frozen] Mock<IAssessmentToolMapper> assessmentToolMapper,
            IEnumerable<AssessmentTool> assessmentTools,
            AssessmentToolListData assessmentToolListData,
            AssessmentToolQuery query,
            AssessmentToolRequestHandler sut
        )
        {
            //Arrange  
            adminAssessmentToolRepository.Setup(x => x.GetAssessmentTools()).ReturnsAsync(assessmentTools);
            assessmentToolMapper.Setup(x => x.AssessmentToolsToAssessmentToolData(It.IsAny<List<AssessmentTool>>()))
                .Returns(assessmentToolListData);

            //Act
            var result = await sut.Handle(query, CancellationToken.None);

            //Assert
            Assert.Equal(assessmentToolListData, result);
        }
    }
}
