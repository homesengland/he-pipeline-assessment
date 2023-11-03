using He.PipelineAssessment.Data.BIL.VOALandValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.BIL.VOAAgriculturalLandValues
{
    public class VoaAgricultureLandValues
    {
        public string lep_area { get; set; } = null!;
        public double? agricultural { get; set; }
        public double? stat_year { get; set; }
        public double? objectId { get; set; }
    }

    public class Feature
    {
        public VoaAgricultureLandValues attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class VoaAgricultureLandValuesResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
