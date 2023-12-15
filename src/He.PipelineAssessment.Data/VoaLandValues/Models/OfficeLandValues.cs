

using He.PipelineAssessment.Data.ExtendedSinglePipeline;

namespace He.PipelineAssessment.Data.VoaLandValues.Models.Office
{
    public class OfficeLandValues
    {
        public string lep_area { get; set; } = null!;
        public double? office_sq_m { get; set; }
        public double? common_area_sqm { get; set; }
        public double? land_edge_centre { get; set; }
        public double? land_out_town { get; set; }
        public double? office_sq_m_gia { get; set; }
        public double? common_area_sqm_gia { get; set; }
        public double? stat_year { get; set; }
        public double? object_id { get; set; }
    }

    public class Feature
    {
        public OfficeLandValues attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class OfficeLandValuesResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}


