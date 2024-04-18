namespace He.PipelineAssessment.Data.VoaLandValues.Models.Agriculture
{
    public class AgricultureLandValues
    {
        public string lep_area { get; set; } = null!;
        public double? agricultual { get; set; }
        public double? stat_year { get; set; }
        public double? objectId { get; set; }
    }

    public class Feature
    {
        public AgricultureLandValues attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class AgricultureLandValuesResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}


