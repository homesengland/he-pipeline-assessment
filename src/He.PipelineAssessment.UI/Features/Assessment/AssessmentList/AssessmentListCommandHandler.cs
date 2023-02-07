﻿using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessment.AssessmentList
{
    public class AssessmentListCommandHandler : IRequestHandler<AssessmentListCommand, AssessmentListData>
    {
        private IAssessmentRepository _assessmentRepository;

        public AssessmentListCommandHandler(IAssessmentRepository repository)
        {
            _assessmentRepository = repository;
        }
        public async Task<AssessmentListData> Handle(AssessmentListCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var listOfAssessments = await _assessmentRepository.GetAssessments();
                var assessmentListData = GetAssessmentListFromResults(listOfAssessments);

                return assessmentListData;
            }
            catch (Exception e)
            {
                List<string> errors = new List<string> { $"An error occured whilst accessing our data. Exception: {e.Message}" };
                return new AssessmentListData
                {
                    ValidationMessages = errors
                };
            }
        }

        private AssessmentListData GetAssessmentListFromResults(List<Models.Assessment> listOfAssessments)
        {
            AssessmentListData assessmentLandingPageData = new AssessmentListData();
            foreach (var assessment in listOfAssessments)
            {
                var assessmentDisplay = new AssessmentDisplay(assessment);
                assessmentDisplay.CreatedDate = DateTime.UtcNow;
                assessmentDisplay.LastModified = DateTime.UtcNow;
                assessmentDisplay.AssessmentWorkflowId = Guid.NewGuid().ToString();
                assessmentLandingPageData.ListOfAssessments.Add(assessmentDisplay);
            }
            return assessmentLandingPageData;
        }
    }
}
