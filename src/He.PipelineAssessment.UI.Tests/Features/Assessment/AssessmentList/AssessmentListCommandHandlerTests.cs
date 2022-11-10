using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Repo = He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Tests.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandlerTests
    {

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsError_GivenRepoThrowsError(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
            AssessmentListCommand assessmentListCommand,
            Exception exception,
            AssessmentListCommandHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessments()).Throws(exception);

            //Act
            var result = await sut.Handle(assessmentListCommand, CancellationToken.None);

            //Assert

            Assert.IsType<AssessmentListData>(result);
            Assert.Empty(result.ListOfAssessments);
            Assert.False(result.IsValid);
            Assert.Single(result.ValidationMessages);
        }

        [Theory]
        [AutoMoqData]
        public async Task Handle_ReturnsLAssessmentListData_GivenNoErrorsEncountered(
            [Frozen] Mock<IAssessmentRepository> assessmentRepository,
             AssessmentListCommand assessmentListCommand,
            List<Repo.Assessment> assessments,
            AssessmentListCommandHandler sut
        )
        {
            //Arrange
            assessmentRepository.Setup(x => x.GetAssessments())
                .ReturnsAsync(assessments);

            //Act
            var result = await sut.Handle(assessmentListCommand, CancellationToken.None);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(assessments.Count(), result.ListOfAssessments.Count());
        }
    }
}
