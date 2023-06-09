﻿CREATE OR ALTER PROCEDURE [dbo].[GetAssessmentInterventionListByAssessmentId]
    @assessmentId int
AS
BEGIN
   SELECT 
	i.Id,
	a.Reference,
	atfi.WorkflowName,
	i.RequestedBy,
	i.Administrator,
	i.Status,
	i.DecisionType,
	i.CreatedDateTime,
	i.LastModifiedDateTime
	FROM dbo.AssessmentIntervention i
	JOIN dbo.AssessmentToolWorkflowInstance atfi ON i.AssessmentToolWorkflowInstanceId = atfi.Id
	JOIN dbo.Assessment a ON atfi.AssessmentId = a.Id and a.Id=@assessmentId
    ORDER BY i.CreatedDateTime
END


