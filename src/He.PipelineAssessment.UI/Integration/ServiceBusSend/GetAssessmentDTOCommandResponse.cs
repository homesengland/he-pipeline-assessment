using FluentValidation.Results;
using He.PipelineAssessment.UI.Integration.ServiceBusSend;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.QuestionScreenSaveAndContinue
{
    public class GetAssessmentDTOCommandResponse : IRequest<GetAssessmentDTOCommand>
    {
        public GetAssessmentDTOCommandResponse()
        { 
            this.Assessment = new AssessmentDTO();
        }

        public AssessmentDTO Assessment { get; set; }
    }
}
