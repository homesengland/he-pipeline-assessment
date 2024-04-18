using MediatR;

namespace He.PipelineAssessment.UI.Integration.Project
{
    public class UpsertProjectCommand : IRequest<int>
    {
        public UpsertProjectCommand(ProjectDTO projectData)
        {
            ProjectData = projectData;
        }
        public ProjectDTO ProjectData { get; set; }
    }
}
