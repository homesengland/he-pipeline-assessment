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
        #region UserName Tests

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsName_GivenHttpHttpContextResponseIsNotNull(
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
            var result = sut.UserName();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Kevin", result);
        }

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsNull_GivenHttpHttpContextResponseIsNull(
           [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserProvider sut)
        {
            //Arrange
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns((HttpContext?)null);

            //Act
            var result = sut.UserName();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsValue_GivenLocalAuthorityIssuer(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var claimsIdentity = new ClaimsIdentity();
            
            var claim = new Claim(ClaimTypes.Name, "LocalUser", ClaimValueTypes.String, "LOCAL AUTHORITY");
            claimsIdentity.AddClaim(claim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.UserName();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("LocalUser", result);
        }

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsNull_GivenNoIdentities(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var claimsPrincipal = new ClaimsPrincipal(); // No identities added
            
            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.UserName();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsNull_GivenNoClaims(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var claimsIdentity = new ClaimsIdentity(); // No claims added
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.UserName();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void UserName_ReturnsNull_GivenNoNameClaim(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            
            // Add a claim that's NOT a "name" claim
            var claim = new Claim("email", "test@example.com");
            claimsIdentity.AddClaim(claim);
            var localAuthClaim = claimsIdentity.FindFirst(claim => claim.Issuer == "LOCAL AUTHORITY");
            claimsIdentity.RemoveClaim(localAuthClaim); // Ensure issuer is not "LOCAL AUTHORITY" to avoid special handling
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.UserName();

            //Assert
            Assert.Null(result);
        }

        #endregion

        #region Email Tests

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsEmail_GivenHttpHttpContextResponseIsNotNull(
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
            var result = sut.Email();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Kevin@homesengland.gov.uk", result);
        }

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsNull_GivenHttpHttpContextResponseIsNull(
         [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
          UserProvider sut)
        {
            //Arrange
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns((HttpContext?)null);

            //Act
            var result = sut.Email();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsNull_GivenNoIdentities(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var claimsPrincipal = new ClaimsPrincipal(); // No identities added
            
            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.Email();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsNull_GivenNoClaims(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var claimsIdentity = new ClaimsIdentity(); // No claims added
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.Email();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsNull_GivenNoEmailClaim(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            
            // Add a claim that doesn't contain "emailaddress"
            var claim = new Claim("name", "Test User");
            claimsIdentity.AddClaim(claim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.Email();

            //Assert
            Assert.Null(result);
        }

        [Theory]
        [AutoMoqData]
        public void Email_ReturnsEmail_GivenClaimContainsEmailAddress(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            
            // Test that claim type contains "emailaddress" (not exact match)
            var claim = new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", "user@test.com");
            claimsIdentity.AddClaim(claim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.Email();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("user@test.com", result);
        }

        #endregion

        #region Role Checking Tests

        [Theory]
        [AutoMoqData]
        public void CheckUserRole_ReturnsTrue_GivenUserIsInRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, "TestRole");
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.CheckUserRole("TestRole");

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void CheckUserRole_ReturnsFalse_GivenUserIsNotInRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, "DifferentRole");
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.CheckUserRole("TestRole");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public void CheckUserRole_ReturnsFalse_GivenHttpContextIsNull(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            UserProvider sut)
        {
            //Arrange
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns((HttpContext?)null);

            //Act
            var result = sut.CheckUserRole("TestRole");

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsAdmin_ReturnsTrue_GivenUserHasAdminRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, RoleConstants.AppRole.PipelineAdminOperations);
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsAdmin();

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsAdmin_ReturnsFalse_GivenUserDoesNotHaveAdminRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, "SomeOtherRole");
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsAdmin();

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsProjectManager_ReturnsTrue_GivenUserHasProjectManagerRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, RoleConstants.AppRole.PipelineProjectManager);
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsProjectManager();

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsProjectManager_ReturnsFalse_GivenUserDoesNotHaveProjectManagerRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, "SomeOtherRole");
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsProjectManager();

            //Assert
            Assert.False(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsEconomist_ReturnsTrue_GivenUserHasEconomistRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, RoleConstants.AppRole.PipelineEconomist);
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsEconomist();

            //Assert
            Assert.True(result);
        }

        [Theory]
        [AutoMoqData]
        public void IsEconomist_ReturnsFalse_GivenUserDoesNotHaveEconomistRole(
            [Frozen] Mock<IHttpContextAccessor> httpContextAccessor,
            ClaimsPrincipal claimsPrincipal,
            ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            var context = new DefaultHttpContext();
            var roleClaim = new Claim(ClaimTypes.Role, "SomeOtherRole");
            claimsIdentity.AddClaim(roleClaim);
            claimsPrincipal.AddIdentity(claimsIdentity);

            context.User = claimsPrincipal;
            httpContextAccessor.SetupGet(accessor => accessor.HttpContext).Returns(context);

            //Act
            var result = sut.IsEconomist();

            //Assert
            Assert.False(result);
        }

        #endregion
    }
}
