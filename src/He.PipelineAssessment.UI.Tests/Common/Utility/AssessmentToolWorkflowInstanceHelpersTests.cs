using He.PipelineAssessment.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Tests.Common;
using Xunit;
using He.PipelineAssessment.UI.Common.Utility;
using Moq;
using AssessmentToolWorkflowInstance = He.PipelineAssessment.Models.AssessmentToolWorkflowInstance;

namespace He.PipelineAssessment.UI.Tests.Common.Utility
{
    public class AssessmentToolWorkflowInstanceHelpersTests
    {

        [Theory]
        [AutoMoqData]
        public async Task IsOrderEqualToLatestSubmittedWorkflowOrder_ReturnsFalse_GivenNoLatestSubmittedWorkflowInstance(
            List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            foreach (var assessmentToolWorkflowInstance in assessmentToolWorkflowInstances)
            {
                assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Draft;
            }

            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances = assessmentToolWorkflowInstances;

            //Act
            var result = await sut.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsOrderEqualToLatestSubmittedWorkflowOrder_ReturnsFalse_GivenLatestSubmittedWorkflowInstanceInfoIsNull(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            foreach (var assessmentToolWorkflowInstance in assessmentToolWorkflowInstances)
            {
                assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
            }

            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances = assessmentToolWorkflowInstances;
            currentAssessmentToolWorkflowInstance.SubmittedDateTime = DateTime.MaxValue;

            assessmentRepository
                .Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(currentAssessmentToolWorkflowInstance.WorkflowInstanceId))
                .ReturnsAsync((AssessmentToolWorkflowInstance?)null);

            //Act
            var result = await sut.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsOrderEqualToLatestSubmittedWorkflowOrder_ReturnsTrue_GivenLatestSubmittedIsCurrentInstance(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            foreach (var assessmentToolWorkflowInstance in assessmentToolWorkflowInstances)
            {
                assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
            }

            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances = assessmentToolWorkflowInstances;
            currentAssessmentToolWorkflowInstance.SubmittedDateTime = DateTime.MaxValue;
            currentAssessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances.Add(currentAssessmentToolWorkflowInstance);

            assessmentRepository
                .Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(currentAssessmentToolWorkflowInstance.WorkflowInstanceId))
                .ReturnsAsync(currentAssessmentToolWorkflowInstance);

            //Act
            var result = await sut.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsOrderEqualToLatestSubmittedWorkflowOrder_ReturnsTrue_GivenLatestSubmittedIsDifferentInstanceWithSameOrder(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            foreach (var assessmentToolWorkflowInstance in assessmentToolWorkflowInstances)
            {
                assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
            }

            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances = assessmentToolWorkflowInstances;
            var lastInstance = assessmentToolWorkflowInstances.Last();
            lastInstance.SubmittedDateTime = DateTime.MaxValue;
            lastInstance.AssessmentToolWorkflow.AssessmentTool.Order = 123;
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order = 123;

            assessmentRepository
                .Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(lastInstance.WorkflowInstanceId))
                .ReturnsAsync(lastInstance);

            //Act
            var result = await sut.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsOrderEqualToLatestSubmittedWorkflowOrder_ReturnsFalse_GivenLatestSubmittedIsDifferentInstanceWithDifferentOrder(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolWorkflowInstance> assessmentToolWorkflowInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            foreach (var assessmentToolWorkflowInstance in assessmentToolWorkflowInstances)
            {
                assessmentToolWorkflowInstance.Status = AssessmentToolWorkflowInstanceConstants.Submitted;
            }

            currentAssessmentToolWorkflowInstance.Assessment.AssessmentToolWorkflowInstances = assessmentToolWorkflowInstances;
            var lastInstance = assessmentToolWorkflowInstances.Last();
            lastInstance.SubmittedDateTime = DateTime.MaxValue;
            lastInstance.AssessmentToolWorkflow.AssessmentTool.Order = 123;
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.AssessmentTool.Order = 125;

            assessmentRepository
                .Setup(x =>
                    x.GetAssessmentToolWorkflowInstance(lastInstance.WorkflowInstanceId))
                .ReturnsAsync(lastInstance);

            //Act
            var result = await sut.IsOrderEqualToLatestSubmittedWorkflowOrder(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsFalse_GivenNotLastAndNotVariationInstance(
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = false;
            currentAssessmentToolWorkflowInstance.IsVariation = false;

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsFalse_GivenLastInstanceButOtherLastInstanceInDraft(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolWorkflowInstance> draftInstances,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = true;
            currentAssessmentToolWorkflowInstance.IsVariation = false;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(draftInstances);

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsFalse_GivenLastInstanceButNextWorkflowsAvailable(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolInstanceNextWorkflow> nextWorkflows,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = true;
            currentAssessmentToolWorkflowInstance.IsVariation = false;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(new List<AssessmentToolWorkflowInstance>());
            assessmentRepository
                .Setup(x => x.GetLastNextWorkflows(currentAssessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(nextWorkflows);

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsTrue_GivenLastInstanceWithNoNextWorkflowsOrDraftInstances(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = true;
            currentAssessmentToolWorkflowInstance.IsVariation = false;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(new List<AssessmentToolWorkflowInstance>());
            assessmentRepository
                .Setup(x => x.GetLastNextWorkflows(currentAssessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(new List<AssessmentToolInstanceNextWorkflow>());

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsFalse_GivenVariationButOtherLastInstanceInDraft(
           [Frozen] Mock<IAssessmentRepository> assessmentRepository,
           List<AssessmentToolWorkflowInstance> draftInstances,
           AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
           AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = false;
            currentAssessmentToolWorkflowInstance.IsVariation = true;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(draftInstances);

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsFalse_GivenVariationButNextWorkflowsAvailable(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            List<AssessmentToolInstanceNextWorkflow> nextWorkflows,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = false;
            currentAssessmentToolWorkflowInstance.IsVariation = true;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(new List<AssessmentToolWorkflowInstance>());
            assessmentRepository
                .Setup(x => x.GetLastNextWorkflows(currentAssessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(nextWorkflows);

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public async Task IsVariationAllowed_ReturnsTrue_GivenVariationWithNoNextWorkflowsOrDraftInstances(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentToolWorkflowInstance currentAssessmentToolWorkflowInstance,
            AssessmentToolWorkflowInstanceHelpers sut)
        {
            //Arrange
            currentAssessmentToolWorkflowInstance.AssessmentToolWorkflow.IsLast = false;
            currentAssessmentToolWorkflowInstance.IsVariation = true;
            assessmentRepository
                .Setup(x => x.GetLastInstancesByStatus(currentAssessmentToolWorkflowInstance.AssessmentId,
                    AssessmentToolWorkflowInstanceConstants.Draft))
                .ReturnsAsync(new List<AssessmentToolWorkflowInstance>());
            assessmentRepository
                .Setup(x => x.GetLastNextWorkflows(currentAssessmentToolWorkflowInstance.AssessmentId))
                .ReturnsAsync(new List<AssessmentToolInstanceNextWorkflow>());

            //Act
            var result = await sut.IsVariationAllowed(currentAssessmentToolWorkflowInstance);

            //Assert
            Assert.True(result);
        }
    }
}
