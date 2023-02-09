using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using MediatR;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncCommandHandler : IRequestHandler<SyncCommand, SyncResponse>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ISinglePipelineService _singlePipelineService;
        private readonly ISyncCommandHandlerHelper _syncCommandHandlerHelper;

        public SyncCommandHandler(IAssessmentRepository assessmentRepository, ISinglePipelineService singlePipelineService, ISyncCommandHandlerHelper syncCommandHandlerHelper)
        {
            _assessmentRepository = assessmentRepository;
            _singlePipelineService = singlePipelineService;
            _syncCommandHandlerHelper = syncCommandHandlerHelper;
        }

        public async Task<SyncResponse> Handle(SyncCommand request, CancellationToken cancellationToken)
        {
            var errorMessages = new List<string>();
            try
            {
                var data = await _singlePipelineService.GetSinglePipelineData();
                if (data != null)
                {
                    List<int> sourceAssessmentSpIds = data.Select(x => x.sp_id!.Value).ToList();

                    var destinationAssessments = await _assessmentRepository.GetAssessments();
                    var destinationAssessmentSpIds = destinationAssessments.Select(x => x.SpId).ToList();

                    var assessmentsToBeAdded = _syncCommandHandlerHelper.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, data);
                    await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);

                    var existingAssessments = destinationAssessmentSpIds.Intersect(sourceAssessmentSpIds).ToList();
                    _syncCommandHandlerHelper.UpdateAssessments(destinationAssessments, existingAssessments, data);
                    await _assessmentRepository.SaveChanges();

                }
                else
                {
                    errorMessages.Add("Single Pipeline Response data returned null");
                }

            }
            catch (Exception e)
            {
                errorMessages.Add(e.Message);
            }

            return new SyncResponse()
            {
                ErrorMessages = errorMessages
            };
        }
    }
}
