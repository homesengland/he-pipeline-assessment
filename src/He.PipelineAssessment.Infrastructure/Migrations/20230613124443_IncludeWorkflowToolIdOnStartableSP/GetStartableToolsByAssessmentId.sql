CREATE OR ALTER PROCEDURE [dbo].[GetStartableToolsByAssessmentId] 
    @assessmentId int
AS
BEGIN
    SELECT ATW.AssessmentToolId, ATW.Id as AssessmentToolWorkflowId, ATW.WorkflowDefinitionId, ATW.IsFirstWorkflow 
    FROM AssessmentToolInstanceNextWorkflow NW
    INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = NW.NextWorkflowDefinitionId
    WHERE NW.AssessmentId = @assessmentId AND 
    NW.IsStarted = 0 AND 
    (ATW.Status IS NULL OR ATW.Status != ''Deleted'')

    UNION ALL

    SELECT AssessmentToolId, Id as AssessmentToolWorkflowId, WorkflowDefinitionId, IsFirstWorkflow 
    FROM AssessmentToolWorkflow 
    WHERE IsFirstWorkflow = 1 AND 
    (Status IS NULL OR Status != ''Deleted'')
END