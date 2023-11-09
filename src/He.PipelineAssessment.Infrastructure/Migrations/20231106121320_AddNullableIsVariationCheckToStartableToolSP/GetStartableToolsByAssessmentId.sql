CREATE OR ALTER PROCEDURE [dbo].[GetStartableToolsByAssessmentId] 
    @assessmentId int
AS
BEGIN
    SELECT 
	ATW.AssessmentToolId, 
	ATW.Id as AssessmentToolWorkflowId, 
	ATW.WorkflowDefinitionId, 
	ATW.IsFirstWorkflow, 
	CAST(IIF(NW.IsVariation IS NULL, 0, NW.IsVariation) AS bit) AS IsVariation
    FROM AssessmentToolInstanceNextWorkflow NW
    INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = NW.NextWorkflowDefinitionId
    WHERE NW.AssessmentId = @assessmentId AND 
    (ATW.Status IS NULL OR ATW.Status != ''Deleted'')

    UNION ALL

    SELECT 
	ATW.AssessmentToolId, 
	ATW.Id as AssessmentToolWorkflowId, 
	ATW.WorkflowDefinitionId, 
	ATW.IsFirstWorkflow, 
	CAST(IIF(ATWI.IsVariation IS NULL, 0, ATWI.IsVariation) AS bit) AS IsVariation
    FROM AssessmentToolWorkflow ATW
	LEFT JOIN AssessmentToolWorkflowInstance ATWI on ATW.Id = ATWI.AssessmentToolWorkflowId and ATWI.AssessmentId = @assessmentId
    WHERE ATW.IsFirstWorkflow = 1 AND 
    (ATW.Status IS NULL OR ATW.Status != ''Deleted'') AND
	ATWI.Id is null
END