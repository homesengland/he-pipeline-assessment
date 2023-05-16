namespace He.PipelineAssessment.UI.Helper;

public static class CurrencyHelper
{
    public static string ToCommaSeparatedNumber(this decimal? inputCurrency)
    {
        var result = String.Format("{0:#,##0.##}", inputCurrency);
        return result.ToString();
    }

    public static string ToCommaSeparatedNumber(this string inputCurrency)
    {
        if(string.IsNullOrEmpty(inputCurrency) || string.IsNullOrWhiteSpace(inputCurrency))
        {
            return string.Empty;
        } 

        var result = "£" + String.Format("{0:#,##0.##}", Convert.ToDecimal(inputCurrency));
        return result;
    }
}
