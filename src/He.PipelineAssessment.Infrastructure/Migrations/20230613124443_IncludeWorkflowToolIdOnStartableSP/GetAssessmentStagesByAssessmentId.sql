
CREATE OR ALTER PROCEDURE [dbo].[GetAssessmentStagesByAssessmentId]
    @assessmentId int
AS
BEGIN
    WITH Assessment_CTE (WorkflowName, WorkflowDefinitionId, WorkflowInstanceId, CurrentActivityId, CurrentActivityType, [Status], CreatedDateTime, SubmittedDateTime, 
	AssessmentToolWorkflowInstanceId, AssessmentToolId, IsFirstWorkflow,LastModifiedDateTime,Result, SubmittedBy )
    AS
    (
        SELECT ATWFI.WorkflowName, ATWFI.WorkflowDefinitionId, ATWFI.WorkflowInstanceId, ATWFI.CurrentActivityId, ATWFI.CurrentActivityType, ATWFI.[Status], ATWFI.CreatedDateTime,
		ATWFI.SubmittedDateTime, ATWFI.Id, ATWF.AssessmentToolID, 
		ATWF.IsFirstWorkflow, ATWFI.LastModifiedDateTime,ATWFI.Result, ATWFI.SubmittedBy
        FROM AssessmentToolWorkFlowInstance ATWFI 
        LEFT JOIN AssessmentToolWorkflow ATWF ON ATWFI.AssessmentToolWorkflowId = AtWF.Id 
        WHERE ATWFI.AssessmentId = @assessmentId AND (ATWF.Status IS NULL OR ATWF.Status != ''Deleted'')
    )
    SELECT AT.Id AS AssessmentToolId, AT.[Name], AT.IsVisible, AT.[Order], Assessment_CTE.WorkflowName, Assessment_CTE.WorkflowDefinitionId,
	Assessment_CTE.WorkflowInstanceId, Assessment_CTE.CurrentActivityId, Assessment_CTE.CurrentActivityType, Assessment_CTE.[Status], 
	Assessment_CTE.CreatedDateTime, Assessment_CTE.SubmittedDateTime, Assessment_CTE.AssessmentToolWorkflowInstanceId, Assessment_CTE.IsFirstWorkflow,  Assessment_CTE.LastModifiedDateTime,
	Result, SubmittedBy
	FROM AssessmentTool AT
    LEFT JOIN Assessment_CTE ON AT.Id = Assessment_CTE.AssessmentToolId
    WHERE AT.Status IS NULL OR AT.Status != ''Deleted''
    ORDER BY AT.[Order]
END