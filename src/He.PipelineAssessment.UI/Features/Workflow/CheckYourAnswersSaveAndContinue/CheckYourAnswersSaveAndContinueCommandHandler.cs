using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Authorization;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Workflow.CheckYourAnswersSaveAndContinue
{
    public class CheckYourAnswersSaveAndContinueCommandHandler : IRequestHandler<CheckYourAnswersSaveAndContinueCommand,
        CheckYourAnswersSaveAndContinueCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IUserProvider _userProvider;
        private readonly IRoleValidation _roleValidation;
        private readonly ILogger<CheckYourAnswersSaveAndContinueCommandHandler> _logger;

        public CheckYourAnswersSaveAndContinueCommandHandler(IElsaServerHttpClient elsaServerHttpClient,
            IAssessmentRepository assessmentRepository,
            IUserProvider userProvider, IRoleValidation roleValidation,
            ILogger<CheckYourAnswersSaveAndContinueCommandHandler> logger)
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _assessmentRepository = assessmentRepository;
            _userProvider = userProvider;
            _roleValidation = roleValidation;
            _logger = logger;
        }

        public async Task<CheckYourAnswersSaveAndContinueCommandResponse?> Handle(
            CheckYourAnswersSaveAndContinueCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (!await _roleValidation.ValidateRole(request.AssessmentId, request.WorkflowDefinitionId))
                {
                    throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                }
                else
                {
                    var checkYourAnswersSaveAndContinueCommandDto = new CheckYourAnswersSaveAndContinueCommandDto()
                    {
                        ActivityId = request.Data.ActivityId,
                        WorkflowInstanceId = request.Data.WorkflowInstanceId
                    };
                    var response =
                        await _elsaServerHttpClient.CheckYourAnswersSaveAndContinue(
                            checkYourAnswersSaveAndContinueCommandDto);

                    if (response != null)
                    {
                        var currentAssessmentToolWorkflowInstance =
                            await _assessmentRepository.GetAssessmentToolWorkflowInstance(response.Data
                                .WorkflowInstanceId);
                        if (currentAssessmentToolWorkflowInstance != null &&
                            currentAssessmentToolWorkflowInstance.Status !=
                            AssessmentToolWorkflowInstanceConstants.Submitted)
                        {
                            currentAssessmentToolWorkflowInstance.CurrentActivityId = response.Data.NextActivityId;
                            currentAssessmentToolWorkflowInstance.CurrentActivityType = response.Data.ActivityType;
                            await _assessmentRepository.SaveChanges();

                            CheckYourAnswersSaveAndContinueCommandResponse result =
                                new CheckYourAnswersSaveAndContinueCommandResponse()
                                {
                                    ActivityId = response.Data.NextActivityId,
                                    ActivityType = response.Data.ActivityType,
                                    WorkflowInstanceId = response.Data.WorkflowInstanceId,
                                    IsAuthorised = true
                                };
                            return await Task.FromResult(result);
                        }
                    }
                    else
                    {
                        _logger.LogError(
                            $"Unable to submit workflow. AssessmentId: {request.AssessmentId} WorkflowDefinitionId:{request.WorkflowDefinitionId}");
                        throw new ApplicationException(
                            $"Unable to submit workflow. AssessmentId: {request.AssessmentId} WorkflowDefinitionId:{request.WorkflowDefinitionId}");
                    }
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException(
                    $"Unable to submit workflow. AssessmentId: {request.AssessmentId} WorkflowDefinitionId:{request.WorkflowDefinitionId}");

            }
            return null;
        }
    }

}
