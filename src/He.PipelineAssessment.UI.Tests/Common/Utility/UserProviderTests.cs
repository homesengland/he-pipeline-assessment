using AutoFixture.Xunit2;
using He.PipelineAssessment.Common.Tests;
using He.PipelineAssessment.Data.PCSProfile;
using He.PipelineAssessment.UI.Common.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Moq;
using System;
using System.Net;
using System.Security.Claims;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Common.Utility
{
    public class UserProviderTests
    {
        [Theory]
        [AutoMoqData]
        public void GetUserName_ReturnsNull_GivenHttpHttpContextResponseIsNull(
           [Frozen] Mock<IHttpContextAccessor> httpContextMock, 
          // ClaimsIdentity claimsIdentity,
            UserProvider sut)
        {
            //Arrange
            // httpContextMock.Setup(h => h.HttpContext.User.Identities.First().Claims.First(c => c.Type == "name").Value).Returns(It.IsAny<string>());
            // var mockIHttpContextAccessor = new Mock<IHttpContextAccessor>();
            //mockIHttpContextAccessor.Setup(h => h.HttpContext.User.Identity.Name).Returns(It.IsAny<string>());
            // mockIHttpContextAccessor.Object.HttpContext.User.Identities.Append(ClaimsIdentity).

            //var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            //Arrange
           // Mock<IHttpContextAccessor> mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            //var claim = new Claim(ClaimTypes.Name, "Kevin");
        

            //var mockPrincipal = new Mock<ClaimsPrincipal>(claim);
            //mockPrincipal.Setup(x => x.Identities.First().Claims.First()).Returns(claim);

          
            //var context = new DefaultHttpContext() { User = mockPrincipal.Object };

            //var anme = context.User.Claims.First(c => c.Type == "name").Value;

            //mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);




            ClaimsPrincipal user = new ClaimsPrincipal(
                        new ClaimsIdentity(
                            new Claim[] { new Claim("MyClaim", "MyClaimValue") },
                            "Basic")
                        ); 
            
            httpContextMock.Setup(h => h.HttpContext.User).Returns(user);


            //Act
            var result = sut.GetUserName();

            //Assert
            Assert.NotNull(result);
            Assert.Equal("Kevin Nick", result);
            //Assert.Null(result);
            //Assert.Equal("The HttpContext is null", result);
        }
    }
}
