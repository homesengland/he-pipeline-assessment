CREATE OR ALTER PROCEDURE [dbo].[GetEconomistAssessments]

AS
BEGIN
SELECT 
    a.[Id],
    a.[SpId],
    a.[SiteName],
    a.[Counterparty],
    a.[Reference],
    a.[ProjectManager],
    a.[ProjectManagerEmail],
    a.[LocalAuthority],
    a.[FundingAsk],
    a.[NumberOfHomes],
    a.[ValidData],
    a.[CreatedDateTime],
    a.[InternalReference],
    (
    	select top 1 
    	atwi.[LastModifiedDateTime] 
    	from [AssessmentToolWorkflowInstance] atwi 
    	where a.Id = atwi.AssessmentId 
    	order by 
    	[LastModifiedDateTime] desc
    ) as [LastModifiedDateTime],
    economistAssessments.EconomistWorkflowStatus as [Status],
    a.SensitiveStatus
    FROM [dbo].[Assessment] a 
    INNER JOIN 
    (
    	SELECT
    	ATWI.AssessmentId as AssessmentId , AT.Name + '' Started'' as EconomistWorkflowStatus
    	FROM [dbo].[AssessmentToolWorkflowInstance] ATWI
    	INNER JOIN [dbo].AssessmentToolWorkflow ATW ON ATWI.WorkflowDefinitionId = ATW.WorkflowDefinitionId AND ATW.IsEconomistWorkflow=1
		INNER JOIN [dbo].AssessmentTool AT ON (ATW.AssessmentToolId = AT.Id AND (AT.Status IS NULL OR AT.Status != ''Deleted''))
    	WHERE ATWI.Status = ''Draft''

    	UNION

    	SELECT 
    	ATWINW.AssessmentId as AssessmentId , AT.Name + '' Not Started'' as EconomistWorkflowStatus
    	FROM [dbo].[AssessmentToolInstanceNextWorkflow] ATWINW
    	INNER JOIN [dbo].AssessmentToolWorkflow ATW ON ATWINW.NextWorkflowDefinitionId = ATW.WorkflowDefinitionId AND ATW.IsEconomistWorkflow=1
    	LEFT JOIN [dbo].[AssessmentToolWorkflowInstance] ATWI on ATWI.AssessmentId = ATWINW.AssessmentId AND  ATWINW.NextWorkflowDefinitionId = ATWI.WorkflowDefinitionId
		INNER JOIN [dbo].AssessmentTool AT ON (ATW.AssessmentToolId = AT.Id AND (AT.Status IS NULL OR AT.Status != ''Deleted''))
    	WHERE ATWI.AssessmentId is null

    ) as  economistAssessments ON a.Id = economistAssessments.AssessmentId
    GROUP BY
    a.[Id],
    a.[SpId],
    a.[SiteName],
    a.[Counterparty],
    a.[Reference],
    a.[ProjectManager],
    a.[ProjectManagerEmail],
    a.[LocalAuthority],
    a.[FundingAsk],
    a.[NumberOfHomes],
    a.[ValidData],
    a.[CreatedDateTime],
    a.[InternalReference],
    economistAssessments.EconomistWorkflowStatus,
    a.SensitiveStatus
END 
