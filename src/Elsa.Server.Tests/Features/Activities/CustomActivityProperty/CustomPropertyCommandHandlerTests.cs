﻿using AutoFixture.Xunit2;
using Elsa.CustomActivities.Activities.QuestionScreen;
using Elsa.CustomActivities.Describers;
using Elsa.CustomInfrastructure.Data.Repository;
using Elsa.CustomModels;
using Elsa.CustomWorkflow.Sdk;
using Elsa.Server.Features.Activities.CustomActivityProperties;
using Elsa.Server.Features.Workflow.LoadCheckYourAnswersScreen;
using Elsa.Server.Providers;
using He.PipelineAssessment.Common.Tests;
using Moq;
using NuGet.Frameworks;
using Xunit;

namespace Elsa.Server.Tests.Features.Activities.CustomActivityProperty;

public class CustomPropertyCommandHandlerTests
{
    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsCorrectValues_GivenNoErrorsThrown(
        [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
        CustomPropertyCommand loadCustomProperties,
        List<HeActivityInputDescriptor> inputDescriptors,
        CustomPropertyCommandHandler sut)
    {
        //Arrange
        propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Returns(inputDescriptors);

        //Act
        var result = await sut.Handle(loadCustomProperties, CancellationToken.None);

        //Assert
        Assert.True(result.GetType() == typeof(Dictionary<string, List<HeActivityInputDescriptor>>));
        Assert.Equal(inputDescriptors, result.Values.First());
    }

    [Theory]
    [AutoMoqData]
    public async Task Handle_ReturnsEmptyDictionary_GivenExceptionIsThrown(
        [Frozen] Mock<ICustomPropertyDescriber> propertyDescriber,
        CustomPropertyCommand loadCustomProperties,
        Exception e,
        CustomPropertyCommandHandler sut)
    {
        //Arrange
        propertyDescriber.Setup(x => x.DescribeInputProperties(typeof(Question))).Throws(e);

        //Act
        var result = await sut.Handle(loadCustomProperties, CancellationToken.None);

        //Assert
        Assert.Empty(result);
    }
}