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
	(
		select top 1 [LastModifiedDateTime] 
		from [AssessmentToolWorkflowInstance] atwi 
		where a.Id = atwi.AssessmentId 
		order by [LastModifiedDateTime] desc
	) as [LastModifiedDateTime],
	ISNULL((
		select top 1 at.Name 
		from [AssessmentToolWorkflowInstance] atwi 
		inner join AssessmentToolWorkflow atw on atw.WorkflowDefinitionId = atwi.WorkflowDefinitionId
		inner join AssessmentTool at on at.Id = atw.AssessmentToolId
		where a.Id = atwi.AssessmentId 
		order by atwi.[LastModifiedDateTime] desc
	),''New'') as [Status]
	FROM [dbo].[Assessment] a 
END



