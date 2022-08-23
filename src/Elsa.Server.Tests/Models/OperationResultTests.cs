using AutoFixture.Xunit2;
using Elsa.Server.Models;
using FluentAssertions;
using Xunit;

namespace Elsa.Server.Tests.Models
{
    public class OperationResultTests
    {
        [Theory]
        [AutoData]
        public void OperationResult_ShouldReturnIsSuccessFalse_WhenErrorMessageExists
        (
            OperationResult<string> sut
        )
        {
            //Arrange


            //Act

            //Assert
            sut.ErrorMessages.Should().HaveCount(3);
            Assert.False(sut.IsSuccess);
        }

        [Theory]
        [AutoData]
        public void OperationResult_ShouldReturnIsValidFalse_WhenValidationMessageExists
        (
            OperationResult<string> sut
        )
        {
            //Arrange


            //Act

            //Assert
            sut.ValidationMessages.Should().HaveCount(3);
            Assert.False(sut.IsValid);
        }

        [Theory]
        [AutoData]
        public void OperationResult_ShouldReturnIsValidTrue_WhenNoValidationMessagesExist
        (
            OperationResult<string> sut
        )
        {
            //Arrange
            sut.ValidationMessages = new List<string>();

            //Act

            //Assert
            sut.ValidationMessages.Should().HaveCount(0);
            Assert.True(sut.IsValid);
        }

        [Theory]
        [AutoData]
        public void OperationResult_ShouldReturnIsSuccessfulTrue_WhenNoErrorMessagesExist
        (
            OperationResult<string> sut
        )
        {
            //Arrange
            sut.ErrorMessages = new List<string>();

            //Act

            //Assert
            sut.ErrorMessages.Should().HaveCount(0);
            Assert.True(sut.IsSuccess);
        }
    }
}
