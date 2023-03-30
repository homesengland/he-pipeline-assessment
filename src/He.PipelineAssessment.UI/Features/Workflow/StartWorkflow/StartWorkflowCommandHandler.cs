using Azure.Core;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using He.PipelineAssessment.UI.Features.Workflow.LoadQuestionScreen;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.StartWorkflow
{
    public class StartWorkflowCommandHandler : IRequestHandler<StartWorkflowCommand, LoadQuestionScreenRequest?>
    {

        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly ILogger<StartWorkflowCommandHandler> _logger;
        public StartWorkflowCommandHandler(IElsaServerHttpClient elsaServerHttpClient, IAssessmentRepository assessmentRepository, IUserProvider userProvider, ILogger<StartWorkflowCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _logger = logger;
        }

        public async Task<LoadQuestionScreenRequest?> Handle(StartWorkflowCommand request, CancellationToken cancellationToken)
        {
            var isRoleExist = await ValidateRole(request.AssessmentId);

            if(!isRoleExist)
            {
                return new LoadQuestionScreenRequest()
                {
                    IsCorrectBusinessArea = false
                };
            }
            try
            {
                var dto = new StartWorkflowCommandDto()
                {
                    WorkflowDefinitionId = request.WorkflowDefinitionId,
                    CorrelationId = request.CorrelationId
                };
                var response = await _elsaServerHttpClient.PostStartWorkflow(dto);

                if (response != null)
                {
                    var result = new LoadQuestionScreenRequest()
                    {
                        ActivityId = response.Data.NextActivityId,
                        WorkflowInstanceId = response.Data.WorkflowInstanceId,
                        ActivityType = response.Data.ActivityType,
                        IsCorrectBusinessArea = true
                    };

                    var assessmentStage = AssessmentStage(request, response);

                    await _assessmentRepository.CreateAssessmentToolWorkflowInstance(assessmentStage);

                    return await Task.FromResult(result);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message);
                return null;
            }

        }

        private static AssessmentToolWorkflowInstance AssessmentStage(StartWorkflowCommand request, WorkflowNextActivityDataDto response)
        {
            var assessmentStage = new AssessmentToolWorkflowInstance();
            assessmentStage.WorkflowInstanceId = response.Data.WorkflowInstanceId;
            assessmentStage.AssessmentId = request.AssessmentId;
            assessmentStage.Status = AssessmentStageConstants.Draft;
            assessmentStage.WorkflowName = response.Data.WorkflowName;
            assessmentStage.WorkflowDefinitionId = request.WorkflowDefinitionId;
            assessmentStage.CurrentActivityId = response.Data.NextActivityId;
            assessmentStage.CurrentActivityType = response.Data.ActivityType;
            return assessmentStage;
        }

        private async Task<bool> IsRoleExist(int assessmentId)
        {
            bool isRoleExist = false;
            var assessment = await _assessmentRepository.GetAssessment(assessmentId);

            if (assessment != null)
            {
                switch (assessment?.BusinessArea)
                {
                    case "MPP":
                        isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorMPP);
                        return isRoleExist;
                    case "Investment":
                        isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorInvestment);
                        return isRoleExist;
                    case "Development":
                        isRoleExist = _userProvider.CheckUserRole(Constants.AppRole.PipelineAssessorDevelopment);
                        return isRoleExist;
                    default: return isRoleExist;
                }
            }

            return isRoleExist;
        }
    }
}
