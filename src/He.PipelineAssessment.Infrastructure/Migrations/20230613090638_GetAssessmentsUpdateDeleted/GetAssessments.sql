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
		inner join AssessmentToolWorkflow atw on atw.WorkflowDefinitionId = atwi.WorkflowDefinitionId and (atw.Status !=''Deleted'' OR atw.Status is null)
		inner join AssessmentTool at on at.Id = atw.AssessmentToolId and (at.Status !=''Deleted'' OR at.Status is null)
		where a.Id = atwi.AssessmentId 
		order by atwi.[LastModifiedDateTime] desc
	),''New'') as [Status]
	FROM [dbo].[Assessment] a 
END



