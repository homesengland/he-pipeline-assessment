using He.Notification.Client;
using He.PipelineAssessment.Infrastructure.Repository.StoredProcedure;
using He.PipelineAssessment.Models.ViewModels;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListRequestHandler : IRequestHandler<AssessmentListRequest, List<AssessmentDataViewModel>>
    {
        private readonly IStoredProcedureRepository _storedProcedureRepository;
        private readonly ILogger<AssessmentListRequestHandler> _logger;
        private readonly INotificationService _notificationService;

        public AssessmentListRequestHandler(IStoredProcedureRepository storedProcedureRepository, ILogger<AssessmentListRequestHandler> logger, INotificationService notificationService)
        {
            _storedProcedureRepository = storedProcedureRepository;
            _logger = logger;
            _notificationService = notificationService;
        }
        public async Task<List<AssessmentDataViewModel>> Handle(AssessmentListRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var dbAssessments = await _storedProcedureRepository.GetAssessments();

                if (request.IsChronJobRequest)
                {
                    await PASAssessmentsEmailHandler(dbAssessments);
                }
                var filteredAssessments = dbAssessments.Where(x =>
                    !x.IsSensitiveRecord() || (x.IsSensitiveRecord() &&
                                               (request.CanViewSensitiveRecords ||
                                                request.Username == x.ProjectManager)));

                return filteredAssessments.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException("Unable to get list of assessments.");
            }
        }

        private async Task PASAssessmentsEmailHandler(List<AssessmentDataViewModel> assessmentDataViewModels)
        {
            try
            {
                var totalProjects = assessmentDataViewModels.Count();

                var newProjects = assessmentDataViewModels.Where(x => x.CreatedDateTime > DateTime.Now.AddDays(-1)
                                                                   && x.CreatedDateTime <= DateTime.Now)
                                                           .Count();

                var updatedProjects = assessmentDataViewModels.Where(x => x.LastModifiedDateTime > DateTime.Now.AddDays(-1) && x.LastModifiedDateTime <= DateTime.Now
                    && !(x.CreatedDateTime <= DateTime.Now && x.CreatedDateTime > DateTime.Now.AddDays(-1) && x.CreatedDateTime <= DateTime.Now)).Count();


                Dictionary<String, dynamic> personalisation = new Dictionary<String, dynamic>
            {
                {"NewProjects", newProjects},
                {"UpdatedProjects", updatedProjects},
                {"Date", DateTime.Now},
                {"TotalProjects", totalProjects},
                { "recipientEmail","avinash.bobba@homesengland.gov.uk" }

            };

                DynamicEmailModel<Dictionary<String, dynamic>> sendEmailRequest = new DynamicEmailModel<Dictionary<String, dynamic>>()
                {
                    TemplateId = "486e4950-fd2e-4a4f-99cf-6c2e54c9675a",
                    Personalisation = personalisation,
                };
                var response = await this._notificationService.SendEmail(sendEmailRequest);

                if (String.IsNullOrEmpty(response.NotificationId))
                {
                    _logger.LogInformation($"Email Send Successfully on : {DateTime.Now}");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
            }

        }
    }
}
