using He.PipelineAssessment.Data.VFM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.BIL.VOALandValues
{
    public class VoaLandValues
    {
        public string gss_code { get; set; } = null!;
        public string? la_name { get; set; }
        public double? residential_value { get; set; }
        public double? industrial_value { get; set; }
        public string? region { get; set; }
        public double? units { get; set; }
        public double? gia_sqm { get; set; }
        public double? resi_land_unit { get; set; }
        public double? stand_house_unit { get; set; }
        public double? high_dens_unit { get; set; }
        public double? stat_year { get; set; }
    }

    public class Feature
    {
        public VoaLandValues attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class VoaLandValuesResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
