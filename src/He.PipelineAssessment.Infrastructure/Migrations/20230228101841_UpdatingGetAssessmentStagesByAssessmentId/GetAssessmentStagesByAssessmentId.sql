
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
		ATWF.IsFirstWorkflow, LastModifiedDateTime,Result, SubmittedBy
        FROM AssessmentToolWorkFlowInstance ATWFI 
        LEFT JOIN AssessmentToolWorkflow ATWF ON ATWFI.WorkflowdefinitionId = AtWF.WorkflowdefinitionId
        WHERE ATWFI.AssessmentId = @assessmentId
    )
    SELECT AT.Id AS AssessmentToolId, [Name], IsVisible, [Order], Assessment_CTE.WorkflowName, Assessment_CTE.WorkflowDefinitionId,
	Assessment_CTE.WorkflowInstanceId, Assessment_CTE.CurrentActivityId, Assessment_CTE.CurrentActivityType, Assessment_CTE.[Status], 
	Assessment_CTE.CreatedDateTime, Assessment_CTE.SubmittedDateTime, Assessment_CTE.AssessmentToolWorkflowInstanceId, Assessment_CTE.IsFirstWorkflow,  Assessment_CTE.LastModifiedDateTime,
	Result, SubmittedBy
	FROM AssessmentTool AT
    LEFT JOIN Assessment_CTE ON AT.Id = Assessment_CTE.AssessmentToolId
    ORDER BY AT.[Order]
END