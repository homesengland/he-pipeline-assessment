using System.Globalization;

namespace He.PipelineAssessment.Models.ViewModels
{

    public class AssessmentDataViewModel : AuditableEntityViewModel
    {
        public int Id { get; set; }
        public int SpId { get; set; }
        public string? SiteName { get; set; }
        public string? Counterparty { get; set; }
        public string? Reference { get; set; }
        public string? ProjectManager { get; set; }
        public string? ProjectManagerEmail { get; set; }
        public string? LocalAuthority { get; set; }
        public decimal? FundingAsk { get; set; }
        public int? NumberOfHomes { get; set; }
        public string? Status { get; set; }

        public string StatusDisplayTag()
        {
            switch (Status)
            {
                case "New":
                    return "blue";
                case "Complete":
                    return "green";
                case "Failed":
                    return "red";
            }
            return "yellow";
        }

        public string FundingAskCurrency
        {
            get
            {
                if (FundingAsk.HasValue)
                {
                    return String.Format(new CultureInfo("en-GB", false), "{0:c0}", this.FundingAsk);
                }
                else
                {
                    return "-";
                }
            }
        }

        public string NumberOfHomesFormatted
        {
            get
            {
                if (NumberOfHomes.HasValue)
                {
                    return NumberOfHomes.Value.ToString("N0");
                }
                else
                {
                    return "-";
                }
            }
        }
    }
}
