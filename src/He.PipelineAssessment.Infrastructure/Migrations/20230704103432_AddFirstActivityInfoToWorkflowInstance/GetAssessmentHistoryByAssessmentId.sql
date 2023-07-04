CREATE OR ALTER   PROCEDURE [dbo].[GetAssessmentHistoryByAssessmentId]
    @assessmentId int
AS
BEGIN
     SELECT 
		AT.IsVisible, 
		AT.[Order], 
		AT.Name +'' - ''+ ATWFI.WorkflowName as [Name],
		ATWFI.WorkflowName, 
		ATWFI.WorkflowDefinitionId, 
		ATWFI.WorkflowInstanceId,
		ATWFI.CurrentActivityId, 
		ATWFI.CurrentActivityType, 
		ATWFI.FirstActivityId, 
		ATWFI.FirstActivityType, 
		ATWFI.[Status], 
		ATWFI.CreatedDateTime,
		ATWFI.SubmittedDateTime, 
		ATWFI.Id as AssessmentToolWorkflowInstanceId, 
		ATWF.AssessmentToolID, 
		ATWF.IsFirstWorkflow, 
		ATWFI.LastModifiedDateTime,
		ATWFI.Result,
		ATWFI.SubmittedBy
        FROM AssessmentToolWorkFlowInstance ATWFI 
        INNER JOIN AssessmentToolWorkflow ATWF ON ATWFI.AssessmentToolWorkflowId = AtWF.Id 	AND (ATWF.Status IS NULL OR ATWF.Status != ''Deleted'')
		INNER JOIN AssessmentTool AT ON AT.Id = ATWF.AssessmentToolId 
        WHERE 
		ATWFI.AssessmentId = @assessmentId  AND
		(ATWFI.Status != ''Draft'' AND ATWFI.Status != ''Submitted'')
END