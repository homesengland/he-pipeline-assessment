using He.PipelineAssessment.Infrastructure.Repository;
using He.PipelineAssessment.Models;

namespace He.PipelineAssessment.UI.Features.Assessments
{
    //Mock class to generate static data for the front end.  Will replace with API as we progress with this task.
    public class AssessmentStubData
    {
        private static Random _randomSelector = new Random();
        private List<string> _sites = null!;
        private List<string> _projectManagers = null!;
        private List<string> _counterParties = null!;
        private List<string> _referenceSeed = null!;
        private List<string> _statuses = null!;
        private int numberOfAssessments = 50;

        public AssessmentStubData()
        {
            InitStubData();
        }

        public List<Assessment> GetAssessments()
        {
            List<Assessment> listOfAssessments = new List<Assessment>();
            for(int i = 0; i < numberOfAssessments; i++)
            {
                string projectManager = GetRandomProjectManager();
                Assessment assessment = new Assessment
                {
                    SpId = _randomSelector.Next(i, 20000),
                    SiteName = GetRandomSiteName(),
                    ProjectManager = projectManager,
                    ProjectManagerEmail = projectManager + "@homesengland.gov.uk",
                    Counterparty = GetRandomCounterParty(),
                    Reference = GetRandomReference(),
                    Status = GetRandomStatus(),
                };
                listOfAssessments.Add(assessment); 
            }
            return listOfAssessments;
        }


        #region Mock Data Generators


        private void InitStubData()
        {
            _counterParties = new List<string>()
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
            _projectManagers = new List<string>()
            {
                "Joe Bloggs",
                "Jane Doe"
            };
            _sites = new List<string>()
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
            _statuses = new List<string>()
            {
                "Complete",
                "In Progress",
                "New",
            };
            _referenceSeed = new List<string>()
            {
                "ABC",
                "XYZ",
                "EHP",
                "BTP",
                "YCH"
            };
        }


        private string GetRandomSiteName()
        {
            return GetRandomResult(_sites);
        }

        private string GetRandomProjectManager()
        {
            return GetRandomResult(_projectManagers);
        }

        private string GetRandomCounterParty()
        {
            return GetRandomResult(_counterParties);
        }

        private string GetRandomStatus()
        {
            var randomIndex = _randomSelector.Next(_statuses.Count);
            return _statuses[randomIndex];
        }

        private string GetRandomResult(List<string> listOfValues)
        {
            var randomResult = _randomSelector.Next(listOfValues.Count);
            return listOfValues[randomResult];
        }

        private string GetRandomReference()
        {
            var randomSeedIndex = _randomSelector.Next(_referenceSeed.Count());
            string randomSeed = _referenceSeed[randomSeedIndex];
            string guidString = Guid.NewGuid().ToString();
            return randomSeed + "_" + guidString.Substring(0, 6).ToUpper();
        }

        #endregion
    }
}
