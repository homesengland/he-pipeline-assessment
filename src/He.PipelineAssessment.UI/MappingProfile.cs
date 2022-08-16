using AutoMapper;
using He.PipelineAssessment.UI.Models;

namespace He.PipelineAssessment.UI
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Activitydata, Elsa.CustomModels.ActivityData>();
            CreateMap<Choice, Elsa.CustomModels.Choice>();
            CreateMap<Case, Elsa.CustomModels.Case>();
            CreateMap<Elsa.CustomModels.ActivityData, Activitydata>();
            CreateMap<Elsa.CustomModels.Choice, Choice>();
            CreateMap<Elsa.CustomModels.Case, Case>();
        }
    }
}