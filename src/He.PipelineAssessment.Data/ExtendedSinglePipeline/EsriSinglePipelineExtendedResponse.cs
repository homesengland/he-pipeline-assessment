using System.Text.Json;

namespace He.PipelineAssessment.Data.ExtendedSinglePipeline
{
    public class SinglePipelineExtendedData
    {
        public int? esri_id { get; set; }
        public int? sp_id { get; set; }
        public string? sp_reference { get; set; }
        public long? date_added { get; set; }
        public long? date_removed { get; set; }
        public string? sp_status { get; set; }
        public string? sp_business_area { get; set; }
        public string? sp_source_pipeline { get; set; }
        public string? sp_stage { get; set; }
        public string? sp_type { get; set; }
        public string? sp_support_req { get; set; }
        public string? sp_identified_prog { get; set; }
        public string? internal_reference { get; set; }
        public string? pipeline_opportunity_site_name { get; set; }
        public string? postcode { get; set; }
        public int? units_or_homes { get; set; }
        public string? sp_timescale { get; set; }
        public string? delivery_timeline { get; set; }
        public decimal? funding_ask { get; set; }
        public decimal? land_size_hectares { get; set; }
        public string? he_advocate_f_name { get; set; }
        public string? he_advocate_s_name { get; set; }
        public string? he_advocate_email { get; set; }
        public string? he_interv_lead_f_name { get; set; }
        public string? he_interv_lead_s_name { get; set; }
        public string? he_interv_lead_email { get; set; }
        public string? partner_aoi_input { get; set; }
        public string? partner_aoi_input_codes { get; set; }
        public string? local_authority { get; set; }
        public string? applicant_1 { get; set; }
        public string? status { get; set; }
        public long? date_processed { get; set; }
        public long? date_changed { get; set; }
        public string? sensitive_status { get; set; }
        public int? project_identifier { get; set; }
        public string? market_failure { get; set; }
        public string? barriers { get; set; }
        public string? la_gss_code { get; set; }
        public string? region { get; set; }
        public string? contains_brownfield { get; set; }
        public string? urban_classification { get; set; }
        public string? opportunity_overview { get; set; }
        public string? regen_proxy { get; set; }
        public string? pcs_number { get; set; }
        public string? la_name { get; set; }
        public string? la_gss_code_prev { get; set; }
        public string? proposed_intervention { get; set; }
        public decimal? affordability_ratio { get; set; }
        public string? geographical_targeting_yn { get; set; }
        public string? ttwa_grow_potential_yn { get; set; }
        public string? lep_areas { get; set; }
    }

    public class Feature
    {
        public SinglePipelineExtendedData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriSinglePipelineExtendedResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
        public bool exceededTransferLimit { get; set; } 
    }

    public class SinglePipelineExtendedDataList
    {
        public List<SinglePipelineExtendedData>? SinglePipelineData { get; set; }
        public bool ExceededTransferLimit { get; set; }

    }

}
