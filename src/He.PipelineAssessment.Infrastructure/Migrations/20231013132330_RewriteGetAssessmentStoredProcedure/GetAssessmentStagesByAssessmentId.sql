CREATE OR ALTER PROCEDURE [dbo].[GetAssessmentStagesByAssessmentId]
        @assessmentId int
    AS
    BEGIN
	-- Get all Assessment Tool Workflow Instances with a Draft or Submitted Status, with all data needed to populate the Assessment Summary page
	-- Create a temporary table #INSTANCES to which the remaining queries will JOIN
	SELECT 
		AT.Id AS AssessmentToolId, 
		AT.Name, 
		AT.IsVisible, 
		AT.[Order], 
		ATWI.WorkflowDefinitionId, 
		ATWI.WorkflowInstanceId, 
		ATWI.CurrentActivityId, 
		ATWI.CurrentActivityType, 
		ATWI.FirstActivityId, 
		ATWI.FirstActivityType, 
		ATWI.[Status], 
		ATWI.CreatedDateTime, 
		ATWI.SubmittedDateTime, 
		ATWI.Id AS AssessmentToolWorkflowInstanceId, 
		ATW.IsFirstWorkflow,  
		ATW.IsEconomistWorkflow,
		ATWI.LastModifiedDateTime,
		ATWI.Result, 
		ATWI.SubmittedBy
		INTO #INSTANCES 
		FROM AssessmentToolWorkflowInstance ATWI
		INNER JOIN AssessmentToolWorkflow ATW ON ATWI.AssessmentToolWorkflowId = ATW.Id AND (ATW.Status IS NULL OR ATW.Status != ''Deleted'')
		INNER JOIN AssessmentTool AT ON ATW.AssessmentToolId = AT.Id AND (AT.Status IS NULL OR AT.Status != ''Deleted'')
		WHERE (ATWI.Status IS NULL OR ATWI.Status = ''Draft'' OR ATWI.Status = ''Submitted'') AND ATWI.AssessmentId = @assessmentId
	(
	-- ALL Stages which are not Economist
	SELECT 
		AT.Id AS AssessmentToolId, 
		AT.Name, 
		AT.IsVisible, 
		AT.[Order],
		#INSTANCES.*  
	FROM AssessmentTool AT
	LEFT JOIN #INSTANCES ON AT.Id = #INSTANCES.AssessmentToolId
	WHERE (AT.Status IS NULL OR AT.Status != ''Deleted'') 
		AND ((SELECT COUNT(*) FROM AssessmentToolWorkflow AST WHERE AST.AssessmentToolId = AT.Id AND AST.IsEconomistWorkflow != 1) > 0)

	UNION
	-- ALL Stages for the current Assessment, which have an AssessmentToolInstanceNextWorkflow record - potentially startable
	SELECT 	
		AT.Id AS AssessmentToolId, 
		AT.Name, 
		AT.IsVisible, 
		AT.[Order],
		#INSTANCES.*
	FROM AssessmentToolInstanceNextWorkflow ATINW
	INNER JOIN AssessmentToolWorkflow ATW ON ATINW.NextWorkflowDefinitionId = ATW.WorkflowDefinitionId AND (ATW.Status IS NULL OR ATW.Status != ''Deleted'')
	INNER JOIN AssessmentTool AT ON ATW.AssessmentToolId = AT.Id AND (AT.Status IS NULL OR AT.Status != ''Deleted'')
	LEFT JOIN #INSTANCES ON AT.Id = #INSTANCES.AssessmentToolId
	WHERE ATINW.AssessmentId = @assessmentId

	UNION
	-- Any Economist Workflow Stages completed or in progress
	SELECT 	
		AT.Id AS AssessmentToolId, 
		AT.Name, 
		AT.IsVisible, 
		AT.[Order],
		#INSTANCES.*
	FROM AssessmentTool AT
	INNER JOIN #INSTANCES ON AT.Id = #INSTANCES.AssessmentToolId
	WHERE (AT.Status IS NULL OR AT.Status != ''Deleted'') 
		AND ((SELECT COUNT(*) FROM AssessmentToolWorkflow AST WHERE AST.AssessmentToolId = AT.Id AND AST.IsEconomistWorkflow = 1) > 0)
	)

	DROP TABLE #INSTANCES

END
