using He.PipelineAssessment.UI.Features.Assessments.AssessmentList;
using He.PipelineAssessment.UI.Features.Assessments.AssessmentSummary;

namespace He.PipelineAssessment.UI.Features.Assessments
{
    //Mock class to generate static data for the front end.  Will replace with API as we progress with this task.
    public class MockAssessmentDataService
    {
        private static Random _randomSelector = new Random();
        private List<string> _projectNames = null!;
        private List<string> _projectManagers = null!;
        private List<string> _partners = null!;
        private List<string> _teams = null!;
        private List<string> _authorities = null!;
        private List<AssessmentStatus> _statuses = null!;

        public MockAssessmentDataService()
        {
            InitStubData();
        }

        public AssessmentListData GetData(AssessmentListCommand command)
        {
            return GetAssessmentListDummyData();
        }

        public AssessmentSummaryData GetData(AssessmentSummaryCommand command)
        {
            return GetSummaryDummyData(command.AssessmentId);
        }

        private AssessmentSummaryData GetSummaryDummyData(string id)
        {
            return new AssessmentSummaryData()
            {
                Id = id,
                SiteName = GetRandomProjectName(),
                Project = GetRandomProjectName(),
                Partner = GetRandomPartner(),
                ProjectManager = GetRandomProjectManager(),
                Stages = GetStages(),

            };
        }



        private AssessmentListData GetAssessmentListDummyData()
        {
            var assessments = new AssessmentListData
            {
                ListOfAssessments = new List<Assessment>()
            };
            for (int i = 0; i < 150; i++)
            {
                assessments.ListOfAssessments.Add(new Assessment
                {
                    Id = "12345_" + i,
                    ProjectName = GetRandomProjectName(),
                    ProjectManager = GetRandomProjectManager(),
                    Partner = GetRandomPartner(),
                    Team = GetRandomTeam(),
                    LocalAuthority = GetRandomLocalAuthority(),
                    DateCreated = GetDateCreated(),
                    Status = GetRandomStatus(),
                    AssessmentWorkflowId = Guid.NewGuid().ToString()
                });
            }
            return assessments;
        }


        #region Mock Data Generators


        private void InitStubData()
        {
            _partners = new List<string>()
            {
                "Sterling Park Development Ltd",
                "Ridley Homes Ltd",
                "Wyatt Homes Ltd",
                "CLL Ltd",
                "Ash Green Holdings Ltd",
                "Hanwood Park LLP",
                "YQ Developments Ltd",
                "Dartford Warbler Ltd",
                "Foxglove Investment Homes Ltd"
            };
            _authorities = new List<string>()
 {
                "Manchester",
                "Winchester",
                "Multi-Site",
                "Newcastle",
                "Liverpool",
                "Durham",
                "Hull"
            };
            _projectManagers = new List<string>()
            {
                "Joe Bloggs",
                "Jane Doe"
            };
            _teams = new List<string>()
            {
                "Team A",
                "Team B",
                "Team C",
                "Team D",
                "Team E",
                "Team F",
                "Team G",
                "Team H",
            };
            _projectNames = new List<string>()
            {
                "ECF2-Canning Town",
                "Hornsea",
                "East Kettering 2nd Phase",
                "Victory court, Camberley",
                "Exchange Court",
                "WH Multi-Site",
                "Barton Lane",
                "Slacken Lane, Talke",
                "Woodland View"
            };
            _statuses = new List<AssessmentStatus>()
            {
                AssessmentStatus.Complete,
                AssessmentStatus.Stopped,
                AssessmentStatus.In_Progress,
                AssessmentStatus.New
            };
        }

        private string GetRandomLocalAuthority()
        {
            return GetRandomResult(_authorities);
        }

        private string GetRandomProjectName()
        {
            return GetRandomResult(_projectNames);
        }

        private string GetRandomProjectManager()
        {
            return GetRandomResult(_projectManagers);
        }

        private string GetRandomPartner()
        {
            return GetRandomResult(_partners);
        }

        private DateTime GetDateCreated()
        {
            var randomMonth = _randomSelector.Next(24);
            var randomHour = _randomSelector.Next(24);
            return DateTime.Now.AddMonths(-randomMonth).AddHours(-randomHour);
        }

        private AssessmentStatus GetRandomStatus()
        {
            var randomIndex = _randomSelector.Next(_statuses.Count);
            return _statuses[randomIndex];
        }

        private string GetRandomTeam()
        {
            return GetRandomResult(_teams);
        }

        private IEnumerable<AssessmentStage> GetStages()
        {
            return new List<AssessmentStage>
           {
               new AssessmentStage()
               {
                   StageId = "12345_1",
                   StageName = "Stage 1 Eligibility",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-14),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-13),
                   Result = "Pass",
               },
               new AssessmentStage()
               {
                   StageId = "12345_2",
                   StageName = "Stage 1 Market Failure",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-13),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-13).AddMinutes(5),
                   Result = "Imperfect information",
               },
               new AssessmentStage()
               {
                   StageId = "12345_3",
                   StageName = "Stage 2 Heirarchy of Intervention",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-13).AddMinutes(5),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-11),
                   Result = "Loan",
               },
               new AssessmentStage()
               {
                   StageId = "12345_4",
                   StageName = "Stage 2 Value for Money",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-11),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-11).AddHours(2),
                   Result = "Pass",
               },
               new AssessmentStage()
               {
                   StageId = "12345_5",
                   StageName = "Stage 2 Strategic Fit",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-11).AddHours(2),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-10),
                   Result = "Medium",
               },
               new AssessmentStage()
               {
                   StageId = "12345_4",
                   StageName = "Stage 4 Deliverability",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(-10),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(8),
                   Result = "Medium",
               },
               new AssessmentStage()
               {
                   StageId = "12345_4",
                   StageName = "Stage 4 Strategic Fit",
                   Status = AssessmentSummaryStatus.Submitted,
                   StartedOn = DateTime.Now.AddDays(8),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(-6).AddHours(-2),
                   Result = "High",
               },
               new AssessmentStage()
               {
                   StageId = "12345_4",
                   StageName = "Stage 4 Value for Money",
                   Status = AssessmentSummaryStatus.Draft,
                   StartedOn = DateTime.Now.AddDays(6).AddHours(-2),
                   SubmittedBy = GetRandomProjectManager(),
                   Submitted = DateTime.Now.AddDays(4),
                   Result = "",
               },

           };
        }

        private string GetRandomResult(List<string> listOfValues)
        {
            var randomResult = _randomSelector.Next(listOfValues.Count);
            return listOfValues[randomResult];
        }

        #endregion
    }
}
