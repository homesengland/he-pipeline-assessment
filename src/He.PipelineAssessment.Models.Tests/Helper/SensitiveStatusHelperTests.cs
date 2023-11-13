using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using He.PipelineAssessment.Models.Helper;
using He.PipelineAssessment.Tests.Common;
using Xunit;

namespace He.PipelineAssessment.Models.Tests.Helper
{
    public class SensitiveStatusHelperTests
    {
        [Theory]
        [InlineAutoMoqData("sensitive - nda in place")]
        [InlineAutoMoqData("sensitive - plc involved in delivery")]
        [InlineAutoMoqData("Sensitive - PLC involved in delivery")]
        [InlineAutoMoqData("Sensitive - NDA in place")]
        [InlineAutoMoqData("SenSitiVe - NdA in plAce")]
        public void IsSensitiveStatus_ReturnsTrue_GivenStatusConsideredSensitive(string status)
        {
            //Act
            var result = SensitiveStatusHelper.IsSensitiveStatus(status);

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineAutoMoqData("sensitive - nda in place not")]
        [InlineAutoMoqData("sensitive")]
        [InlineAutoMoqData("Not Commercially sensitive")]
        [InlineAutoMoqData("Not Commercially Sensitive")]
        [InlineAutoMoqData("Commercially Sensitive")]
        [InlineAutoMoqData("Not Reviewed")]
        [InlineAutoMoqData("")]
        [InlineAutoMoqData("-")]
        [InlineAutoMoqData((string?)null)]

        public void IsSensitiveStatus_ReturnsFalse_GivenStatusNotConsideredSensitive(string? status)
        {
            //Act
            var result = SensitiveStatusHelper.IsSensitiveStatus(status);

            // Assert
            Assert.False(result);
        }
    }
}
