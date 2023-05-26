using He.PipelineAssessment.Tests.Common;
using He.PipelineAssessment.UI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace He.PipelineAssessment.UI.Tests.Helper;
public class CurrencyHelperTests
{
    [Theory]
    [InlineData("2001.233", "2,001.23")]
    [InlineData("200.111", "200.11")]
    [InlineData("20.33", "20.33")]
    [InlineData("2000000.23", "2,000,000.23")]
    [InlineData("2000000", "2,000,000")]
    [InlineData("20", "20")]
    [InlineData("200", "200")]
    [InlineData("2001", "2,001")]
    public void ToCommaSeparatedNumber_ShouldFormateDecimal_AsCurrency(
        string inputCurrency, string formatedExpectedResult
        )
    {
        //Act
        decimal? value = Convert.ToDecimal(inputCurrency);

        var result = value.ToCommaSeparatedNumber();

        //Assert
        Assert.Equal(formatedExpectedResult, result);

    }

    [Fact]
    public void ToCommaSeparatedNumber_ShouldReturnEmptyString_WhenGivenInputNull(
      
      )
    {
        //Act
        decimal? value = null;

        var result = value.ToCommaSeparatedNumber();

        //Assert
        Assert.Equal(string.Empty, result);

    }

    [Theory]
    [InlineData("2001.233", "2,001.23")]
    [InlineData("200.111", "200.11")]
    [InlineData("20.33", "20.33")]
    [InlineData("2000000.23", "2,000,000.23")]
    [InlineData("2000000", "2,000,000")]
    [InlineData("20", "20")]
    [InlineData("200", "200")]
    [InlineData("2001", "2,001")]
    public void ToCommaSeparatedNumber_ShouldFormateString_AsCurrency(
    string inputCurrency, string formatedExpectedResult
    )
    {
        //Act
        var result = inputCurrency.ToCommaSeparatedNumber();

        //Assert
        Assert.Equal(formatedExpectedResult, result);

    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData(" ")]
    public void ToCommaSeparatedNumber_ShouldReturnEmptyString_GivenNoInputValue(string inputValue)
    {
        //Act

        var result = inputValue.ToCommaSeparatedNumber();

        //Assert
        Assert.Equal(string.Empty, result);

    }
}
