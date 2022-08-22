using AutoMapper;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.UI.Models;
using ActivityData = Elsa.CustomWorkflow.Sdk.Models.Workflow.ActivityData;
using Choice = Elsa.CustomWorkflow.Sdk.Models.Workflow.Choice;

namespace He.PipelineAssessment.UI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkflowActivityData, WorkflowActivityDataViewModel>();
            CreateMap<WorkflowActivityDataViewModel, WorkflowActivityDataDto>();
            CreateMap<ActivityData, Models.ActivityData>();
            CreateMap<Models.ActivityData, ActivityData>();
            CreateMap<Choice, Models.Choice>();
            CreateMap<Models.Choice, Choice>();
        }
    }
}