using AutoFixture.Xunit2;
using Elsa.Server.Models;
using Elsa.Server.Providers;
using Xunit;

namespace Elsa.Server.Tests.Providers
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
