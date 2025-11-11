using FluentValidation.Results;


namespace He.PipelineAssessment.UI.Features.Funds.ViewModels
{
    public class AssessmentFundsDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public bool IsEarlyStage { get; set; }
        public bool IsDisabled { get; set; }

        //OVER HERE:
        public ValidationResult? ValidationResult { get; set; }
    }
}
