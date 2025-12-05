using FluentValidation.Results;
using System.ComponentModel.DataAnnotations;
using ValidationResult = FluentValidation.Results.ValidationResult;


namespace He.PipelineAssessment.UI.Features.Funds.ViewModels
{
    public class AssessmentFundsDTO
    {
        public int? Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        [Display(Name = "Is early stage?")]
        public bool IsEarlyStage { get; set; }
        public bool IsDisabled { get; set; }
        public ValidationResult? ValidationResult { get; set; }
    }
}
