using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.UI.Features.Error;
using He.PipelineAssessment.UI.Features.Workflow;
using He.PipelineAssessment.UI.Features.Workflow.LoadWorkflowActivity;
using He.PipelineAssessment.UI.Features.Workflow.SaveAndContinue;
using He.PipelineAssessment.UI.Features.Workflow.StartWorkflow;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Error
{
    public class ErrorControllerTests
    {
        [Theory]
        [AutoMoqData]
        public void Index_ShouldReturn(
            ErrorController sut)
        {
            //Arrange

            //Act
            var result = sut.Index("");

            //Assert
            Assert.IsAssignableFrom<IActionResult>(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
