namespace He.PipelineAssessment.Data.VFM
{
    public class VFMCalculationData
    {
        public int objectid { get; set; }
        public string? gss_code { get; set; }
        public string? la_name { get; set; }
        public string? alt_la_name { get; set; }
        public decimal? lvu_greenfield { get; set; }
        public decimal? lvu_brownfield { get; set; }
        public decimal? lvu_average { get; set; }
        public decimal? ob_lvu { get; set; }
        public decimal? discount_lvu { get; set; }
        public decimal? neg_amen_imp_unit { get; set; }
        public decimal? area_model_addition { get; set; }
        public decimal? high_dem_adju { get; set; }
        public decimal? max_loan_unit_greenfield { get; set; }
        public decimal? max_loan_unit_brownfield { get; set; }
        public decimal? max_loan_unit_average { get; set; }
        public decimal? greenfield_disben { get; set; }
        public decimal? n_units { get; set; }
        public decimal? high_dem_adju_bil_ft { get; set; }
        public string? adopt_local_plan_5_y { get; set; }
        public string? hdt_check { get; set; }
        public decimal? bil_ft_mar_dev_disp { get; set; }
        public decimal? bil_ft_high_dem_disp { get; set; }
        public decimal? hp_increase { get; set; }
        public decimal? hp_year { get; set; }
        public decimal? afford_current { get; set; }
        public decimal? afford_year { get; set; }
        public decimal? net_add_pc { get; set; }
        public decimal? net_add_year { get; set; }
        public string? region { get; set; }
        public decimal? Shape__Area { get; set; }
        public decimal? Shape__Length { get; set; }
        public decimal? la_cost_per_unit_60 { get; set; }
    }


    public class Feature
    {
        public VFMCalculationData attributes { get; set; } = null!;
    }

    public class Field
    {
        public string name { get; set; } = null!;
        public string alias { get; set; } = null!;
        public string type { get; set; } = null!;
        public int? length { get; set; }
    }

    public class EsriVFMResponse
    {
        public string objectIdFieldName { get; set; } = null!;
        public string globalIdFieldName { get; set; } = null!;
        public List<Field> fields { get; set; } = null!;
        public List<Feature> features { get; set; } = null!;
    }
}
