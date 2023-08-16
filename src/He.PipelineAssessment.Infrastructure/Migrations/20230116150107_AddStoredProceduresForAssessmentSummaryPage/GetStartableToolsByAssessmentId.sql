CREATE OR ALTER PROCEDURE [dbo].[GetStartableToolsByAssessmentId] 
    @assessmentId int
AS
BEGIN
    SELECT ATW.AssessmentToolId, ATW.WorkflowDefinitionId, ATW.IsFirstWorkflow FROM AssessmentToolInstanceNextWorkflow NW
    INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = NW.NextWorkflowDefinitionId
    WHERE NW.AssessmentId = @assessmentId AND NW.IsStarted = 0
    UNION ALL
    SELECT AssessmentToolId, WorkflowDefinitionId, IsFirstWorkflow FROM AssessmentToolWorkflow 
    WHERE IsFirstWorkflow = 1
END