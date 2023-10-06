using He.PipelineAssessment.Data.SinglePipeline;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace He.PipelineAssessment.Data.RegionalIPU
{
    public class RegionalIPUData
    {
        public int object_id { get; set; }
        public string? region { get; set; }
        public double? ipu { get; set; }
        public string? product { get; set; }

    }

    public class Feature
    {
        public RegionalIPUData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriRegionalIPUResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
        public bool exceededTransferLimit { get; set; }
    }
}
