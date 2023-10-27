using He.PipelineAssessment.UI.Features.Intervention;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Variation.CreateVariation
{
    public class CreateVariationCommand : AssessmentInterventionCommand, IRequest<int>
    {

    }
}
