namespace He.PipelineAssessment.Data.SinglePipeline
{
    public class SinglePipelineData
    {
        public int? esri_id { get; set; }
        public int? sp_id { get; set; }
        public string? sp_reference { get; set; }
        public long? date_added { get; set; }
        public string? date_removed { get; set; }
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
        public int? funding_ask { get; set; }
        public string? land_size_hectares { get; set; }
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
        public string? opportunity_overview { get; set; }
        public string? confirmed_intervention { get; set; }
        public string? land_type { get; set; }
        public string? planning_stage { get; set; }
        public string? milestone_project_concept { get; set; }
        public string? milestone_contract { get; set; }
        public string? milestone_start { get; set; }
        public string? milestone_project_concept_fc { get; set; }
        public string? milestone_contract_fc { get; set; }
        public string? milestone_start_fc { get; set; }
        public string? project_identifier { get; set; }
        public string? milestone_soc_fc { get; set; }
        public string? milestone_soc { get; set; }
        public string? milestone_business_case_fc { get; set; }
        public string? milestone_business_case { get; set; }
        public string? milestone_hmt_approval { get; set; }
        public string? milestone_hmt_submitted_fc { get; set; }
        public string? milestone_hmt_submitted { get; set; }
        public string? milestone_hmt_approval_fc { get; set; }
        public string? delegation_stage { get; set; }
        public string? delegation_level { get; set; }
        public string? rdel_funding_ask { get; set; }
        public string? proposed_intervention { get; set; }
        public string? multi_funct_team { get; set; }
    }

    public class Feature
    {
        public SinglePipelineData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriSinglePipelineResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
