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
a.[CreatedDateTime],
(
	select top 1 
	atwi.[LastModifiedDateTime] 
	from [AssessmentToolWorkflowInstance] atwi 
	where a.Id = atwi.AssessmentId 
	order by 
	[LastModifiedDateTime] desc
) as [LastModifiedDateTime],
economistAssessments.EconomistWorkflowStatus as [Status]
FROM [dbo].[Assessment] a 
INNER JOIN 
(
	SELECT 
	ATWI.AssessmentId as AssessmentId , ''Economist Tool Started'' as EconomistWorkflowStatus
	FROM [dbo].[AssessmentToolWorkflowInstance] ATWI
	INNER JOIN [dbo].AssessmentToolWorkflow ATW ON ATWI.WorkflowDefinitionId = ATW.WorkflowDefinitionId AND ATW.IsEconomistWorkflow=1
	WHERE ATWI.Status = ''Draft''

	UNION

	SELECT 
	ATWINW.AssessmentId as AssessmentId , ''Economist Tool Not Started'' as EconomistWorkflowStatus
	FROM [dbo].[AssessmentToolInstanceNextWorkflow] ATWINW
	INNER JOIN [dbo].AssessmentToolWorkflow ATW ON ATWINW.NextWorkflowDefinitionId = ATW.WorkflowDefinitionId AND ATW.IsEconomistWorkflow=1
	WHERE ATWINW.IsStarted=0

) as  economistAssessments ON a.Id = economistAssessments.AssessmentId
Group by
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
a.[CreatedDateTime],
economistAssessments.EconomistWorkflowStatus
END 
