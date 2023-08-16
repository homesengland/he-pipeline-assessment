using AutoFixture.Xunit2;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Tests.Common;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Common.Utility
{
    public class UserProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void GetUserName_ReturnsName_GivenHttpHttpContextResponseIsNotNull(
           [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
           ClaimsPrincipal claimsPrincipal,
           ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();

            var claim = new Claim("name", "Kevin");
            claimsIdentity.AddClaim(claim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;

            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.GetUserName();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Kevin", result);
        }

        [Theory]
        [AutoMoqData]
        public void GetUserName_ReturnsNull_GivenHttpHttpContextResponseIsNull(
           [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserProvider sut)
        {
            //Arrange
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns((HttpContext?)null);

            //Act
            var result = sut.GetUserName();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void GetUserEmail_ReturnsEmail_GivenHttpHttpContextResponseIsNotNull(
          [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
          ClaimsPrincipal claimsPrincipal,
          ClaimsIdentity claimsIdentity,
           UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();

            var claim = new Claim("emailaddress", "Kevin@homesengland.gov.uk");
            claimsIdentity.AddClaim(claim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;

            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.GetUserEmail();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Kevin@homesengland.gov.uk", result);
        }

        [Theory]
        [AutoMoqData]
        public void GetUserEmail_ReturnsNull_GivenHttpHttpContextResponseIsNull(
         [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
          UserProvider sut)
        {
            //Arrange
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns((HttpContext?)null);

            //Act
            var result = sut.GetUserEmail();

            //Assert
            Assert.Null(result);
        }
    }
}
