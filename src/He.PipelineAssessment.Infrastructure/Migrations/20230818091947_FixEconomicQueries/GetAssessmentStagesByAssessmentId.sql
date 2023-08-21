CREATE OR ALTER PROCEDURE [dbo].[GetAssessmentStagesByAssessmentId]
        @assessmentId int
    AS
    BEGIN
            WITH Assessment_CTE(
              WorkflowName, 
              WorkflowDefinitionId, 
              WorkflowInstanceId, 
              CurrentActivityId, 
              CurrentActivityType, 
              FirstActivityId, 
              FirstActivityType, 
              [Status], 
              CreatedDateTime, 
              SubmittedDateTime, 
              AssessmentToolWorkflowInstanceId, 
              AssessmentToolId, 
              IsFirstWorkflow,
            IsEconomistWorkflow,
              LastModifiedDateTime,
              Result, 
              SubmittedBy)
            AS
            (
                SELECT 
                     ATWFI.WorkflowName, 
                     ATWFI.WorkflowDefinitionId, 
                     ATWFI.WorkflowInstanceId,
                     ATWFI.CurrentActivityId, 
                     ATWFI.CurrentActivityType, 
                     ATWFI.FirstActivityId, 
                     ATWFI.FirstActivityType, 
                     ATWFI.[Status], 
                     ATWFI.CreatedDateTime,
                     ATWFI.SubmittedDateTime, 
                     ATWFI.Id, 
                     ATWF.AssessmentToolID, 
                     ATWF.IsFirstWorkflow, 
                ATWF.IsEconomistWorkflow,
                     ATWFI.LastModifiedDateTime,
                     ATWFI.Result,
                     ATWFI.SubmittedBy
                FROM AssessmentToolWorkFlowInstance ATWFI 
                LEFT JOIN AssessmentToolWorkflow ATWF ON ATWFI.AssessmentToolWorkflowId = AtWF.Id AND
                     (ATWF.Status IS NULL OR ATWF.Status != ''Deleted'')
                WHERE 
                     ATWFI.AssessmentId = @assessmentId  AND
                     (ATWFI.Status IS NULL OR ATWFI.Status = ''Draft'' OR ATWFI.Status = ''Submitted'')
            )
            SELECT 
              AT.Id AS AssessmentToolId, 
              AT.[Name], 
              AT.IsVisible, 
              AT.[Order], 
              Assessment_CTE.WorkflowName, 
              Assessment_CTE.WorkflowDefinitionId,
              Assessment_CTE.WorkflowInstanceId, 
              Assessment_CTE.CurrentActivityId, 
              Assessment_CTE.CurrentActivityType, 
              Assessment_CTE.FirstActivityId, 
              Assessment_CTE.FirstActivityType, 
              Assessment_CTE.[Status], 
              Assessment_CTE.CreatedDateTime, 
              Assessment_CTE.SubmittedDateTime, 
              Assessment_CTE.AssessmentToolWorkflowInstanceId, 
              Assessment_CTE.IsFirstWorkflow,  
            Assessment_CTE.IsEconomistWorkflow,
              Assessment_CTE.LastModifiedDateTime,
              Assessment_CTE.Result, 
              Assessment_CTE.SubmittedBy
              FROM AssessmentTool AT
              LEFT JOIN Assessment_CTE ON AT.Id = Assessment_CTE.AssessmentToolId
            LEFT JOIN 
            (
              SELECT 
              AST.id  AS AssessmentToolId
              FROM AssessmentTool AST
              INNER JOIN [dbo].[AssessmentToolWorkflow] astw ON ast.Id = astw.AssessmentToolId AND astw.IsEconomistWorkflow=1
              LEFT JOIN AssessmentToolInstanceNextWorkflow astinw ON astw.WorkflowDefinitionId = astinw.NextWorkflowDefinitionId AND AssessmentId=@assessmentId
              WHERE astinw.Id IS NULL
            ) as x ON x.AssessmentToolId = at.Id
            WHERE 
                      (Assessment_CTE.IsEconomistWorkflow = 1 
                      AND (Assessment_CTE.Status = ''Draft''
                      OR Assessment_CTE.Status = ''Submitted'')) 
                      -- return all economist workflows where status is Draft or Submitted
                      OR (x.AssessmentToolId is null 
                      -- return all non-economist workflows
                      and (AT.Status IS NULL OR AT.Status != ''Deleted''))
                      -- only return those which are not Deleted
            ORDER BY AT.[Order]
    END
