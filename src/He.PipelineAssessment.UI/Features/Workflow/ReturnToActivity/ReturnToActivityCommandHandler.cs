using Elsa.CustomWorkflow.Sdk;
using Elsa.CustomWorkflow.Sdk.HttpClients;
using Elsa.CustomWorkflow.Sdk.Models.Workflow;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Authorization;
using MediatR;
using System.Linq.Expressions;
using System.Text.Json;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace He.PipelineAssessment.UI.Features.Workflow.ReturnToActivity
{
    public class ReturnToActivityCommandHandler : IRequestHandler<ReturnToActivityCommand, ReturnToActivityCommandResponse?>
    {
        private readonly IElsaServerHttpClient _elsaServerHttpClient;
        private readonly ILogger<ReturnToActivityCommandHandler> _logger;
        private readonly IAssessmentRepository _assessmentRepository;
        private readonly IRoleValidation _roleValidation;

        public ReturnToActivityCommandHandler(
            IElsaServerHttpClient elsaServerHttpClient,
            IAssessmentRepository assessmentRepository,
            ILogger<ReturnToActivityCommandHandler> logger,
            IRoleValidation roleValidation

            )
        {
            _elsaServerHttpClient = elsaServerHttpClient;
            _logger = logger;
            _assessmentRepository = assessmentRepository;
            _roleValidation = roleValidation;
        }

        public async Task<ReturnToActivityCommandResponse?> Handle(ReturnToActivityCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (request.WorkflowInstanceId != null)
                {
                    var assessmentWorkflowInstance = await _assessmentRepository.GetAssessmentToolWorkflowInstance(request.WorkflowInstanceId);

                    if (!await _roleValidation.ValidateRole(assessmentWorkflowInstance!.AssessmentId, assessmentWorkflowInstance!.WorkflowDefinitionId))
                    {
                        throw new UnauthorizedAccessException($"You do not have permission to access this resource.");
                    }

                    var data = new ReturnToActivityData
                    {
                        WorkflowInstanceId = request.WorkflowInstanceId,
                        ActivityId = request.ActivityId
                    };

                    var response = await _elsaServerHttpClient.ReturnToActivity(data);
                    if (response != null)
                    {
                        return new ReturnToActivityCommandResponse()
                        {
                            WorkflowInstanceId = request.WorkflowInstanceId,
                            ActivityId = response.Data.ActivityId,
                            ActivityType = response.Data.ActivityType
                        };
                    }
                    else
                    {
                        _logger.LogError($"Failed to return to activity, response from elsa server client is null. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}.");
                        throw new ApplicationException("Failed to return to activity.");
                    }
                }
                else
                {
                    _logger.LogError($"Failed to return to activity, workflow instance id was null. WorkflowInstanceId: {request.WorkflowInstanceId}.");
                    throw new ApplicationException("Failed to return to activity.");
                }
            }
            catch (UnauthorizedAccessException e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
            catch ( Exception e )
            {
                _logger.LogError(e, e.Message);
                throw new ApplicationException($"Failed to return to activity. ActivityId: {request.ActivityId} WorkflowInstanceId: {request.WorkflowInstanceId}");
            }
        }
    }
}
