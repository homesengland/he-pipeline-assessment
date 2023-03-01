using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Mappers
{
    public class AssessmentToolMapperTests
    {
        [Theory]
        [AutoMoqData]
        public void AssessmentToolsToAssessmentToolData_Should_Map_WhenEmptyAssessmentToolWorkFlows(
            List<AssessmentTool> assessmentTools,
            AssessmentToolMapper sut
        )
        {
            //Arrange  
            foreach (var assessmentTool in assessmentTools)
            {
                assessmentTool.AssessmentToolWorkflows = null;
            }

            //Act
            var result = sut.AssessmentToolsToAssessmentToolData(assessmentTools);

            //Assert
            Assert.Equal(assessmentTools.Count, result.AssessmentTools.Count);
            for (var i = 0; i < result.AssessmentTools.Count; i++)
            {
                Assert.Equal(assessmentTools[i].Order, result.AssessmentTools[i].Order);
                Assert.Equal(assessmentTools[i].Name, result.AssessmentTools[i].Name);
                Assert.Empty(result.AssessmentTools[i].AssessmentToolWorkFlows);
            }
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentToolsToAssessmentToolData_Should_Map_WhenNonEmptyAssessmentToolWorkFlows(
            List<AssessmentTool> assessmentTools,
            AssessmentToolMapper sut
        )
        {
            //Act
            var result = sut.AssessmentToolsToAssessmentToolData(assessmentTools);

            //Assert
            Assert.Equal(assessmentTools.Count, result.AssessmentTools.Count);
            for (var i = 0; i < result.AssessmentTools.Count; i++)
            {
                Assert.Equal(assessmentTools[i].Order, result.AssessmentTools[i].Order);
                Assert.Equal(assessmentTools[i].Name, result.AssessmentTools[i].Name);
                for (var j = 0; j < result.AssessmentTools[i].AssessmentToolWorkFlows.Count; j++)
                {
                    Assert.NotNull(assessmentTools[i].AssessmentToolWorkflows);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].Id, result.AssessmentTools[i].AssessmentToolWorkFlows[j].Id);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].Name, result.AssessmentTools[i].AssessmentToolWorkFlows[j].Name);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].AssessmentToolId, result.AssessmentTools[i].AssessmentToolWorkFlows[j].AssessmentToolId);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].IsFirstWorkflow, result.AssessmentTools[i].AssessmentToolWorkFlows[j].IsFirstWorkflow);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].IsLatest, result.AssessmentTools[i].AssessmentToolWorkFlows[j].IsLatest);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].WorkflowDefinitionId, result.AssessmentTools[i].AssessmentToolWorkFlows[j].WorkflowDefinitionId);
                    Assert.Equal(assessmentTools[i].AssessmentToolWorkflows![j].Version, result.AssessmentTools[i].AssessmentToolWorkFlows[j].Version);
                }
            }
        }

        [Theory]
        [AutoMoqData]
        public void CreateAssessmentToolCommandToAssessmentTool_Should_Map_Correctly(
            CreateAssessmentToolCommand createAssessmentToolCommand,
            AssessmentToolMapper sut
        )
        {
            //Act
            var result = sut.CreateAssessmentToolCommandToAssessmentTool(createAssessmentToolCommand);

            //Assert
            Assert.Equal(createAssessmentToolCommand.Name, result.Name);
            Assert.Equal(createAssessmentToolCommand.Order, result.Order);
            Assert.True(result.IsVisible);
        }

        [Theory]
        [AutoMoqData]
        public void AssessmentToolWorkflowsToAssessmentToolDto_Should_Map_Correctly(
            List<AssessmentToolWorkflow> assessmentToolWorkflows,
            AssessmentToolMapper sut
        )
        {
            //Act
            var result = sut.AssessmentToolWorkflowsToAssessmentToolDto(assessmentToolWorkflows);

            //Assert
            Assert.Equal(assessmentToolWorkflows.Count, result.Count);
            for (var i = 0; i < result.Count; i++)
            {
                Assert.Equal(assessmentToolWorkflows[i].Id, result[i].Id);
                Assert.Equal(assessmentToolWorkflows[i].Name, result[i].Name);
                Assert.Equal(assessmentToolWorkflows[i].IsFirstWorkflow, result[i].IsFirstWorkflow);
                Assert.Equal(assessmentToolWorkflows[i].IsLatest, result[i].IsLatest);
                Assert.Equal(assessmentToolWorkflows[i].Version, result[i].Version);
                Assert.Equal(assessmentToolWorkflows[i].WorkflowDefinitionId, result[i].WorkflowDefinitionId);
            }
        }
    }
}
