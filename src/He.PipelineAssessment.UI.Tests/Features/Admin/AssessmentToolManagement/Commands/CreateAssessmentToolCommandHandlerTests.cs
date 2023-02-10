﻿using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Common.Utility;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands.CreateAssessmentTool;
using He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Mappers;
using MediatR;
using Moq;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Features.Admin.AssessmentToolManagement.Commands;

public class CreateAssessmentToolCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ThrowException_GivenDependencyThrows
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
        var result =
            await Assert.ThrowsAsync<Exception>(() => sut.Handle(createAssessmentToolCommand, CancellationToken.None));

        //Assert          
        Assert.Equal(exception.Message, result.Message);
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_CallsCreateAssessmentOnRepositoryWithCorrectValues
    (
        [Frozen] Mock<IAssessmentToolMapper> assessmentToolMapper,
        [Frozen] Mock<IAdminAssessmentToolRepository> adminAssessmentToolRepository,
        [Frozen] Mock<IDateTimeProvider> dateTimeProvider,
        CreateAssessmentToolCommand createAssessmentToolCommand,
        DateTime date,
        AssessmentTool assessmentTool,
        CreateAssessmentToolCommandHandler sut
    )
    {
        //Arrange
        assessmentToolMapper.Setup(x => x.CreateAssessmentToolCommandToAssessmentTool(createAssessmentToolCommand))
            .Returns(assessmentTool);
        dateTimeProvider.Setup(x => x.UtcNow()).Returns(date);

        //Act
        var result = await sut.Handle(createAssessmentToolCommand, CancellationToken.None);

        //Assert          
        adminAssessmentToolRepository.Verify(
            x => x.CreateAssessmentTool(It.Is<AssessmentTool>(y =>
                y.CreatedDate == date && y.LastModified == date)), Times.Once);
        Assert.Equal(Unit.Value, result);
    }
}