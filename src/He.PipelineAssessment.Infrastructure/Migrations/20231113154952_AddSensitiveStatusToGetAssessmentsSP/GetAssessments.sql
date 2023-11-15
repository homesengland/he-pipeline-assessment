CREATE OR ALTER PROCEDURE [dbo].[GetAssessments]
AS
BEGIN
	SELECT 
	[Id],
	[SpId],
	[SiteName],
	[Counterparty],
	[Reference],
	[ProjectManager],
	[ProjectManagerEmail],
	[LocalAuthority],
	[FundingAsk],
	[NumberOfHomes],
	[CreatedDateTime],
	[SensitiveStatus],
	(
		SELECT TOP 1 [LastModifiedDateTime] 
		FROM [AssessmentToolWorkflowInstance] ATWI 
		WHERE a.Id = ATWI.AssessmentId AND (ATWI.Status IS NULL OR ATWI.Status = ''Draft'' OR ATWI.Status = ''Submitted'')
		ORDER BY [LastModifiedDateTime] DESC
	) AS [LastModifiedDateTime],
	COALESCE((
		SELECT TOP 1 AT.Name 
		FROM [AssessmentToolWorkflowInstance] ATWI 
		INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = ATWI.WorkflowDefinitionId AND (ATW.Status !=''Deleted'' OR ATW.Status IS NULL)
		INNER JOIN AssessmentTool AT ON AT.Id = ATW.AssessmentToolId AND (AT.Status !=''Deleted'' OR AT.Status IS NULL)
		WHERE a.Id = ATWI.AssessmentId AND (ATWI.Status IS NULL OR ATWI.Status = ''Draft'')
		ORDER BY ATWI.[LastModifiedDateTime] DESC
	),
	(      
		SELECT TOP 1 ''Completed''
        FROM [AssessmentToolWorkflowInstance] ATWI 
        INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = ATWI.WorkflowDefinitionId AND (ATW.Status !=''Deleted'' OR ATW.Status IS NULL)
        INNER JOIN AssessmentTool AT ON AT.Id = ATW.AssessmentToolId AND (AT.Status !=''Deleted'' OR AT.Status IS NULL)
        WHERE a.Id = ATWI.AssessmentId AND ATW.IsLast = 1 AND (ATWI.Status = ''Submitted'')
		AND ((SELECT COUNT(*) FROM [AssessmentToolWorkflowInstance] ATWII WHERE ATWII.AssessmentId = a.Id AND (ATWI.Status IS NULL OR ATWI.Status = ''Draft'')) = 0)
		AND ((SELECT COUNT(*) FROM [AssessmentToolInstanceNextWorkflow] ATINW WHERE ATINW.AssessmentId = a.Id) = 0)
	),
	(
		SELECT TOP 1 AT.Name 
		FROM [AssessmentToolWorkflowInstance] ATWI 
		INNER JOIN AssessmentToolWorkflow ATW ON ATW.WorkflowDefinitionId = ATWI.WorkflowDefinitionId AND (ATW.Status !=''Deleted'' OR ATW.Status IS NULL)
		INNER JOIN AssessmentTool AT ON AT.Id = ATW.AssessmentToolId AND (AT.Status !=''Deleted'' OR AT.Status IS NULL)
		WHERE a.Id = ATWI.AssessmentId AND (ATWI.Status IS NULL OR ATWI.Status = ''Submitted'')
		ORDER BY ATWI.[LastModifiedDateTime] DESC
	),
	''New'') AS [Status]
	FROM [dbo].[Assessment] a 
END