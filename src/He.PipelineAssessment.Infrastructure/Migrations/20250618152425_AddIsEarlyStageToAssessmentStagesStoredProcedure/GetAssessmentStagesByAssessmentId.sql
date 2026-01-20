CREATE OR ALTER PROCEDURE [dbo].[GetAssessmentStagesByAssessmentId]
            @assessmentId int
        AS
        BEGIN
    	IF OBJECT_ID(''tempdb..#INSTANCES'') IS NOT NULL DROP TABLE #INSTANCES
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
			ATW.IsEarlyStage,
    		ATWI.LastModifiedDateTime,
    		ATWI.Result, 
    		ATWI.SubmittedBy,
    		ATWI.WorkflowName,
    		ATWI.IsVariation
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
    		IIF(#INSTANCES.IsVariation = 1, AT.[Order] + 500, AT.[Order]) AS [Order],
    		#INSTANCES.WorkflowDefinitionId, 
    		#INSTANCES.WorkflowInstanceId, 
    		#INSTANCES.CurrentActivityId, 
    		#INSTANCES.CurrentActivityType, 
    		#INSTANCES.FirstActivityId, 
    		#INSTANCES.FirstActivityType, 
    		#INSTANCES.[Status], 
    		#INSTANCES.CreatedDateTime, 
    		#INSTANCES.SubmittedDateTime, 
    		#INSTANCES.AssessmentToolWorkflowInstanceId, 
    		#INSTANCES.IsFirstWorkflow,  
    		#INSTANCES.IsEconomistWorkflow,
			#Instances.IsEarlyStage,
    		#INSTANCES.LastModifiedDateTime,
    		#INSTANCES.Result, 
    		#INSTANCES.SubmittedBy,
    		#INSTANCES.WorkflowName,
    		CAST(IIF(#INSTANCES.IsVariation IS NULL, 0, #INSTANCES.IsVariation) AS bit) AS IsVariation
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
    		IIF(ATINW.IsVariation = 1, AT.[Order] + 500, AT.[Order]) AS [Order],
    		ATINW.NextWorkflowDefinitionId, 
    		NULL AS WorkflowInstanceId, 
    		NULL AS CurrentActivityId, 
    		NULL AS CurrentActivityType, 
    		NULL AS FirstActivityId, 
    		NULL AS FirstActivityType, 
    		NULL AS [Status], 
    		NULL AS CreatedDateTime, 
    		NULL AS SubmittedDateTime, 
    		NULL AS AssessmentToolWorkflowInstanceId, 
    		NULL AS IsFirstWorkflow,  
    		NULL AS IsEconomistWorkflow,
			NULL AS IsEarlyStage,
    		NULL AS LastModifiedDateTime,
    		NULL AS Result, 
    		NULL AS SubmittedBy,
    		NULL AS WorkflowName,
    		CAST(IIF(ATINW.IsVariation IS NULL, 0, ATINW.IsVariation) AS bit) AS IsVariation
    	FROM AssessmentToolInstanceNextWorkflow ATINW
    	INNER JOIN AssessmentToolWorkflow ATW ON ATINW.NextWorkflowDefinitionId = ATW.WorkflowDefinitionId AND (ATW.Status IS NULL OR ATW.Status != ''Deleted'')
    	INNER JOIN AssessmentTool AT ON ATW.AssessmentToolId = AT.Id AND (AT.Status IS NULL OR AT.Status != ''Deleted'')
    	LEFT JOIN #INSTANCES ON AT.Id = #INSTANCES.AssessmentToolId AND AT.[Order] = #INSTANCES.[Order] 
    	WHERE ATINW.AssessmentId = @assessmentId

    	UNION
    	-- Any Economist Workflow Stages completed or in progress
    	SELECT 	
    		AT.Id AS AssessmentToolId, 
    		AT.Name, 
    		AT.IsVisible, 
    		AT.[Order],
    		#INSTANCES.WorkflowDefinitionId, 
    		#INSTANCES.WorkflowInstanceId, 
    		#INSTANCES.CurrentActivityId, 
    		#INSTANCES.CurrentActivityType, 
    		#INSTANCES.FirstActivityId, 
    		#INSTANCES.FirstActivityType, 
    		#INSTANCES.[Status], 
    		#INSTANCES.CreatedDateTime, 
    		#INSTANCES.SubmittedDateTime, 
    		#INSTANCES.AssessmentToolWorkflowInstanceId, 
    		#INSTANCES.IsFirstWorkflow, 
			#INSTANCES.IsEarlyStage, 
    		#INSTANCES.IsEconomistWorkflow,
    		#INSTANCES.LastModifiedDateTime,
    		#INSTANCES.Result, 
    		#INSTANCES.SubmittedBy,
    		#INSTANCES.WorkflowName,
    		CAST(IIF(#INSTANCES.IsVariation IS NULL, 0, #INSTANCES.IsVariation) AS bit) AS IsVariation
    	FROM AssessmentTool AT
    	INNER JOIN #INSTANCES ON AT.Id = #INSTANCES.AssessmentToolId
    	WHERE (AT.Status IS NULL OR AT.Status != ''Deleted'') 
    		AND ((SELECT COUNT(*) FROM AssessmentToolWorkflow AST WHERE AST.AssessmentToolId = AT.Id AND AST.IsEconomistWorkflow = 1) > 0)
    	)

        ORDER BY [Order] ASC, [Status] DESC, [SubmittedDateTime] ASC
    END