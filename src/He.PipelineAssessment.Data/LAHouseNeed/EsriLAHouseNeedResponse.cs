namespace He.PipelineAssessment.Data.LaHouseNeed
{
    public class LaHouseNeedData
    {
        public int objectid { get; set; }
        public string? country { get; set; }
        public string? la_name { get; set; }
        public string? alt_name { get; set; }
        public string? greatest_need { get; set; }
        public string? local_need { get; set; }
        public string? gss_code { get; set; }
        public string? hdt_test { get; set; }
        public string? deprived_third { get; set; }
        public string? high_demand { get; set; }
    }


    public class Feature
    {
        public LaHouseNeedData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriLAHouseNeedResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
