using AutoFixture.Xunit2;
using Elsa.CustomWorkflow.Sdk.Providers;
using Xunit;

namespace Elsa.CustomWorkflow.Sdk.Tests.Providers
{
    public class DateTimeProviderTests
    {
        [Theory]
        [AutoData]
        public void DateTimeProviderReturnsUtcDate_WhenCalled(DateTimeProvider sut)
        {
            //Arrange


            //Act
            var output = sut.UtcNow();
            //Assert
            Assert.IsType<DateTime>(output);
            Assert.Equal(output.ToUniversalTime(), output);
        }
    }
}
