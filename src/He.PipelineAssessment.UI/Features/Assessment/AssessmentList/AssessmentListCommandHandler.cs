﻿using Azure.Identity;
using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;
using MediatR;

namespace He.PipelineAssessment.UI.Features.Assessments.AssessmentList
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
            var listOfAssessments = await _assessmentRepository.GetAssessments();
            var assessmentListData = GetAssessmentListFromResults(listOfAssessments);

            return assessmentListData;
        }

        private AssessmentListData GetAssessmentListFromResults(List<Assessment> listOfAssessments)
        {
            AssessmentListData assessmentLandingPageData = new AssessmentListData();
            Random random = new Random(); //Not needed in anything but the dummy data.  TODO:  Remove when no longer needed;
            foreach(var assessment in listOfAssessments)
            {
                var assessmentDisplay = new AssessmentDisplay(assessment);
                assessmentDisplay.DateCreated = GetDateCreated(random);
                assessmentDisplay.AssessmentWorkflowId = Guid.NewGuid().ToString();
                assessmentLandingPageData.ListOfAssessments.Add(assessmentDisplay);
            }
            return assessmentLandingPageData;
        }

        //TokenCredentialDiagnosticsOptions:  Dummy method to populate Date of the Assessment being created.
        private DateTime GetDateCreated(Random random)
        {
            var randomMonth = random.Next(24);
            var randomHour = random.Next(24);
            return DateTime.Now.AddMonths(-randomMonth).AddHours(-randomHour);
        }
    }
}
