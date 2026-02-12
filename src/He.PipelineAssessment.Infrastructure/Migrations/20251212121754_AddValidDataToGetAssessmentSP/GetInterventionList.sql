CREATE OR ALTER PROCEDURE [dbo].[GetInterventionList]
AS
BEGIN
	SELECT 
	i.Id,
	a.Reference,
	at.Name + '' - '' + atw.Name as WorkflowName,
	i.RequestedBy,
	i.Administrator,
	i.Status,
	i.DecisionType,
	i.CreatedDateTime,
	i.LastModifiedDateTime,
	a.SensitiveStatus,
	a.ProjectManager,
	a.ValidData
	FROM dbo.AssessmentIntervention i
	JOIN dbo.AssessmentToolWorkflowInstance atfi ON i.AssessmentToolWorkflowInstanceId = atfi.Id
	JOIN dbo.Assessment a ON atfi.AssessmentId = a.Id
	JOIN dbo.AssessmentToolWorkflow atw ON atfi.AssessmentToolWorkflowId = atw.Id
	JOIN dbo.AssessmentTool at ON atw.AssessmentToolId = at.Id
END