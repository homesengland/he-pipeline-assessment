using AutoMapper;
using Elsa.CustomWorkflow.Sdk.Models;
using He.PipelineAssessment.UI.Models;
using ActivityData = Elsa.CustomWorkflow.Sdk.Models.ActivityData;
using Choice = Elsa.CustomWorkflow.Sdk.Models.Choice;

namespace He.PipelineAssessment.UI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<WorkflowNavigationData, WorkflowNavigationViewModel>();
            CreateMap<WorkflowNavigationViewModel, WorkflowNavigationDto>();
            CreateMap<ActivityData, Models.ActivityData>();
            CreateMap<Models.ActivityData, ActivityData>();
            CreateMap<Choice, Models.Choice>();
            CreateMap<Models.Choice, Choice>();
        }
    }
}