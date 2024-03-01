using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.SinglePipeline.Sync;
using MediatR;


namespace He.PipelineAssessment.UI.Integration.Project
{
    public class UpsertProjectCommandHandler : IRequestHandler<UpsertProjectCommand, int>
    {
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ISyncCommandHandlerHelper _syncCommandHandlerHelper;
        private readonly ILogger<SyncCommandHandler> _logger;

        public UpsertProjectCommandHandler(
            IAssessmentRepository assessmentRepository,
           ISyncCommandHandlerHelper syncCommandHandlerHelper,
            ILogger<SyncCommandHandler> logger)
        {
            _assessmentRepository = assessmentRepository;
            _syncCommandHandlerHelper = syncCommandHandlerHelper;
            _logger = logger;
        }

        public string GetValueOrDefault(string value, string defaultValue = "-")
        {
            string result = string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            return result;
        }

        public async Task<int> Handle(UpsertProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var destinationAssessments = await _assessmentRepository.GetAssessments();
                Models.Assessment assessmentRecord = destinationAssessments.FirstOrDefault(p => p.SpId == request.ProjectData.ProjectId);
                bool recordExists = (assessmentRecord != null);

                if (assessmentRecord == null)
                {
                    assessmentRecord = new Models.Assessment()
                    {
                        SpId = request.ProjectData.ProjectId,
                        Status = "New"
                    };
                }

                assessmentRecord.BusinessArea = GetValueOrDefault(request.ProjectData.BusinessArea);
                assessmentRecord.Counterparty = GetValueOrDefault(request.ProjectData.Counterparties);
                assessmentRecord.FundingAsk = request.ProjectData.FundingAsk;
                assessmentRecord.LandType = request.ProjectData.LandType;
                assessmentRecord.LocalAuthority = GetValueOrDefault(request.ProjectData.LocalAuthority);
                assessmentRecord.NumberOfHomes = request.ProjectData.NumberOfHomes;
                assessmentRecord.ProjectManager = GetValueOrDefault(request.ProjectData.ProjectOwner);
                assessmentRecord.ProjectManagerEmail = GetValueOrDefault(request.ProjectData.ProjectOwnerEmail);
                assessmentRecord.Reference = GetValueOrDefault(request.ProjectData.PcsReference);
                assessmentRecord.SensitiveStatus = GetValueOrDefault(request.ProjectData.SensitiveStatus);
                assessmentRecord.SiteName = GetValueOrDefault(request.ProjectData.SiteName);

                if (!recordExists)
                {
                    var assessmentsToBeAdded = new List<Models.Assessment>() {
                        assessmentRecord
                    };

                    await _assessmentRepository.CreateAssessments(assessmentsToBeAdded);
                }
                else
                {
                    await _assessmentRepository.SaveChanges();
                }

                var insertedRecord = _assessmentRepository.GetAssessments().Result.Single(p => p.SpId == request.ProjectData.ProjectId);
                return insertedRecord.Id;
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (ApplicationException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Failed to start workflow");
            }
        }
    }
}
