namespace He.PipelineAssessment.UI.Features.Admin.AssessmentToolManagement.Commands
{ 
        public class CreateAssessmentToolData
        {
            public List<string> ValidationMessages { get; set; } = new List<string>();
            public bool IsValid { get { return ValidationMessages != null && ValidationMessages.Count == 0; } }
            public CreateAssessmentToolDto AssessmentToolDto { get; set; } = null!;
        }

        public class CreateAssessmentToolDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Order { get; set; }
           
        }
        
    
}
