namespace He.PipelineAssessment.Data.PCSProfile
{
    public class PCSProfileData
    {
        public int object_id { get; set; }
        public string project_identifier { get; set; } = null!;
        public string project_name { get; set; } = null!;
        public decimal? year_0_expend_fc { get; set; }
        public decimal? year_1_expend_fc { get; set; }
        public decimal? year_2_expend_fc { get; set; }
        public decimal? year_3_expend_fc { get; set; }
        public decimal? year_4_expend_fc { get; set; }
        public decimal? year_5_expend_fc { get; set; }
        public decimal? year_6_expend_fc { get; set; }
        public decimal? year_7_expend_fc { get; set; }
        public decimal? year_8_expend_fc { get; set; }
        public decimal? year_9_expend_fc { get; set; }
        public decimal? year_10_expend_fc { get; set; }
        public decimal? year_11_expend_fc { get; set; }
        public decimal? year_12_expend_fc { get; set; }
        public decimal? year_13_expend_fc { get; set; }
        public decimal? year_14_expend_fc { get; set; }
        public decimal? year_15_expend_fc { get; set; }

        public decimal? total_expend_fc { get; set; }
        public decimal? total_expend_fc_sr { get; set; }
        public decimal? total_expend_ac { get; set; }
        public decimal? year_0_receipt_fc { get; set; }
        public decimal? year_1_receipt_fc { get; set; }
        public decimal? year_2_receipt_fc { get; set; }
        public decimal? year_3_receipt_fc { get; set; }
        public decimal? year_4_receipt_fc { get; set; }
        public decimal? year_5_receipt_fc { get; set; }
        public decimal? year_6_receipt_fc { get; set; }
        public decimal? year_7_receipt_fc { get; set; }
        public decimal? year_8_receipt_fc { get; set; }
        public decimal? year_9_receipt_fc { get; set; }
        public decimal? year_10_receipt_fc { get; set; }
        public decimal? year_11_receipt_fc { get; set; }
        public decimal? year_12_receipt_fc { get; set; }
        public decimal? year_13_receipt_fc { get; set; }
        public decimal? year_14_receipt_fc { get; set; }
        public decimal? year_15_receipt_fc { get; set; }
        public decimal? total_receipt_fc { get; set; }
        public decimal? total_receipt_fc_sr { get; set; }
        public decimal? total_receipt_ac { get; set; }
        public decimal? year_0_comp_fc { get; set; }
        public decimal? year_0_comp_act { get; set; }
        public decimal? year_0_comp_rem_fc { get; set; }
        public decimal? year_1_comp_fc { get; set; }
        public decimal? year_2_comp_fc { get; set; }
        public decimal? year_3_comp_fc { get; set; }
        public decimal? year_4_comp_fc { get; set; }
        public decimal? year_5_comp_fc { get; set; }
        public decimal? year_6_comp_fc { get; set; }
        public decimal? year_7_comp_fc { get; set; }
        public decimal? year_8_comp_fc { get; set; }
        public decimal? year_9_comp_fc { get; set; }
        public decimal? year_10_comp_fc { get; set; }
        public decimal? year_11_comp_fc { get; set; }
        public decimal? year_12_comp_fc { get; set; }
        public decimal? total_comp_fc { get; set; }
        public decimal? total_comp_fc_sr { get; set; }
        public decimal? future_years_comp_fc { get; set; }
        public decimal? total_comp_ac { get; set; }
        public decimal? year_0_af_comp_fc { get; set; }
        public decimal? year_0_af_comp_act { get; set; }
        public decimal? year_0_af_comp_rem_fc { get; set; }
        public decimal? year_1_af_comp_fc { get; set; }
        public decimal? year_2_af_comp_fc { get; set; }
        public decimal? year_3_af_comp_fc { get; set; }
        public decimal? year_4_af_comp_fc { get; set; }
        public decimal? year_5_af_comp_fc { get; set; }
        public decimal? year_6_af_comp_fc { get; set; }
        public decimal? year_7_af_comp_fc { get; set; }
        public decimal? year_8_af_comp_fc { get; set; }
        public decimal? year_9_af_comp_fc { get; set; }
        public decimal? year_10_af_comp_fc { get; set; }
        public decimal? year_11_af_comp_fc { get; set; }
        public decimal? year_12_af_comp_fc { get; set; }
        public decimal? total_af_comp_fc { get; set; }
        public decimal? future_years_af_comp_fc { get; set; }
        public decimal? total_af_comp_fc_sr { get; set; }
        public decimal? total_af_comp_ac { get; set; }
        public string? fund_recovery { get; set; }
    }


    public class Feature
    {
        public PCSProfileData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriPCSProfileResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
