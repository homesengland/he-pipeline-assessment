using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalFigs
{
    public class RegionalFigsData
    {
        public int? object_id { get; set; }
        public string? region { get; set; }
        public long? date_calculated { get; set; }
        public string? appraisal_year { get; set; }
        public double? net_ann_ben_social_rent { get; set; }
        public double? net_ann_ben_afford_rent { get; set; }
        public double? net_ann_ben_support_hous { get; set; }
        public double? net_avg_soc_aff_rent { get; set; }
        public double? avg_house_price { get; set; }
    }

    public class Feature
    {
        public RegionalFigsData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriRegionalFigsResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
        public bool exceededTransferLimit { get; set; }
    }
}
