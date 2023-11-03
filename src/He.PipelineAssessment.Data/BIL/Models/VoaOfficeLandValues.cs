using He.PipelineAssessment.Data.BIL.VOALandValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.BIL.VOAOfficeLandValues
{
    public class VoaOfficeLandValues
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
        public VoaOfficeLandValues attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class VoaOfficeLandValuesResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }

}
