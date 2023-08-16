using AutoFixture.Xunit2;
using Elsa.Server.Models;
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
            Assert.Equal(3, sut.ErrorMessages.Count);
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
            Assert.Equal(3, sut.ValidationMessages!.Errors.Count);
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
            sut.ValidationMessages = null;

            //Act

            //Assert

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
            Assert.Equal(0, sut.ErrorMessages.Count);
            Assert.True(sut.IsSuccess);
        }
    }
}
