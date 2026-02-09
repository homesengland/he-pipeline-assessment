using He.PipelineAssessment.Data.SinglePipeline;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.SinglePipeline.Sync
{
    public class SyncCommandHandler : IRequestHandler<SyncCommand, SyncModel>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ISinglePipelineProvider _singlePipelineProvider;
        private readonly ISyncCommandHandlerHelper _syncCommandHandlerHelper;
        private readonly ILogger<SyncCommandHandler> _logger;

        public SyncCommandHandler(IAssessmentRepository assessmentRepository, ISinglePipelineProvider singlePipelineProvider, ISyncCommandHandlerHelper syncCommandHandlerHelper, ILogger<SyncCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _singlePipelineProvider = singlePipelineProvider;
            _syncCommandHandlerHelper = syncCommandHandlerHelper;
            _logger = logger;
        }

        public async Task<SyncModel> Handle(SyncCommand request, CancellationToken cancellationToken)
        {
            var syncModel = new SyncModel();
            try
            {
                var data = await _singlePipelineProvider.GetSinglePipelineData();
                if (data.Any())
                {
                    List<int> sourceAssessmentSpIds = data.Select(x => x.sp_id!.Value).ToList();

                    var destinationAssessments = await _assessmentRepository.GetAssessments();
                    var destinationAssessmentSpIdsThatHaveBeganAssessment = destinationAssessments.Where(x => x.AssessmentToolWorkflowInstances != null && x.AssessmentToolWorkflowInstances.Any())
                        .Select(x => x.SpId).ToList();
                    var destinationAssessmentSpIdsThatHaveNotBeganAssessment = destinationAssessments.Where(x => x.AssessmentToolWorkflowInstances == null || x.AssessmentToolWorkflowInstances.Count == 0)
                        .Select(x => x.SpId).ToList();
                    
                    var assessmentsToBeRemoved = _syncCommandHandlerHelper.AssessmentsToBeRemoved(sourceAssessmentSpIds, destinationAssessmentSpIdsThatHaveNotBeganAssessment, destinationAssessments);
                    var removedAssessmentCount =  _syncCommandHandlerHelper.SetAssessmentsAsInvalid(assessmentsToBeRemoved);
                    await _assessmentRepository.SaveChanges();
                    syncModel.SetToInvalidAssessmentCount = removedAssessmentCount;
                    //await _assessmentRepository.RemoveAssesments(assessmentsToBeRemoved);

                    var validDestinationAssessmentSpIdsThatHaveNotBeganAssessment = destinationAssessmentSpIdsThatHaveNotBeganAssessment.Except(assessmentsToBeRemoved.Select(x => x.SpId)).ToList();
                    var destinationAssessmentSpIds = destinationAssessmentSpIdsThatHaveBeganAssessment.Concat(validDestinationAssessmentSpIdsThatHaveNotBeganAssessment).ToList();
                    var assessmentsToBeAdded = _syncCommandHandlerHelper.AssessmentsToBeAdded(sourceAssessmentSpIds, destinationAssessmentSpIds, data);
                    await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);

                    var existingAssessments = destinationAssessmentSpIds.Intersect(sourceAssessmentSpIds).ToList();
                    var updatedAssessmentCount = _syncCommandHandlerHelper.UpdateAssessments(destinationAssessments, existingAssessments, data);
                    await _assessmentRepository.SaveChanges();
                    syncModel.NewAssessmentCount = assessmentsToBeAdded.Count;
                    syncModel.UpdatedAssessmentCount = updatedAssessmentCount;
                    syncModel.Synced = true;
                }
                else
                {
                    _logger.LogError("Single Pipeline Response data returned null");
                    throw new ApplicationException("Single Pipeline Response data returned null");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e,e.Message);
                throw new ApplicationException("Single Pipeline Data failed to sync");
            }
            return syncModel;
        }
    }
}
