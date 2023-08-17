IF OBJECT_ID(''tempdb..#tempgroup'') IS NOT NULL DROP TABLE #tempgroup

CREATE TABLE #tempgroup(
	[Id] [int],
	[Name] [nvarchar](450),
	[CreatedDateTime] [datetime2](7),
	[LastModifiedDateTime] [datetime2](7))

insert into #tempgroup
    values
           (1,''Eligibility'',GETDATE(), GETDATE()),
           (2,''Hierarchy of Intervention'',GETDATE(), GETDATE()),
           (3,''Market Failure Economics'',GETDATE(), GETDATE()),
           (4,''Market Failure'',GETDATE(), GETDATE()),
           (5,''Strat Fit 2'',GETDATE(), GETDATE()),
           (6,''VFM Eco 2'',GETDATE(), GETDATE()),
           (7,''VFM 2'',GETDATE(), GETDATE()),
           (8,''SWOT Analysis'' ,GETDATE(), GETDATE()),
           (9,''Del S4'',GETDATE(), GETDATE()),
           (10,''Strat Fit 4'',GETDATE(), GETDATE()),
           (11,''VFM EC S4'',GETDATE(), GETDATE()),
		   (12,''VFM S4'',GETDATE(), GETDATE()),
		   (13,''AssessSummary'',GETDATE(), GETDATE())

MERGE [dbo].[QuestionDataDictionaryGroup] AS Target
    USING #tempgroup	AS Source
    ON Source.Id = Target.Id
    
    -- For Inserts
    WHEN NOT MATCHED BY Target THEN
        INSERT ([id]
          ,[name]
          ,[createddatetime]
          ,[lastmodifieddatetime]) 
        VALUES (Source.Id, Source.Name, Source.CreatedDateTime, Source.LastModifiedDateTime)
    
    -- For Updates
    WHEN MATCHED THEN UPDATE SET
        Target.Id	= Source.Id,
        Target.Name	= Source.Name,
        Target.LastModifiedDateTime	= Source.LastModifiedDateTime;


IF OBJECT_ID(''tempdb..#temp'') IS NOT NULL DROP TABLE #temp

CREATE TABLE #temp
(
    [Id] [int], 
	[DataDictionaryGroupId] [int],
	[LegacyName] [nvarchar](250),
    [Name] [nvarchar](250),
	[Type] [nvarchar](250)
)

Insert into #temp values (1,1,''ass_1_3_funding_no_progress'',''Eligibility_No_Progress_Without_Funding'',''multi-select'')
Insert into #temp values (2,1,''ass_1_2_engalnd'',''Eligibility_Development_In_England'',''multi-select'')
Insert into #temp values (3,1,''ass_1_4_over_5'',''Eligibility_Over_5_Homes'',''multi-select'')
Insert into #temp values (4,1,''ass_1_6_noleaseholdhomes'',''Eligibility_No_Leasehold_Homes'',''multi-select'')
Insert into #temp values (5,1,''ass_1_7_uk_company'',''Eligibility_Registered_UK_Company'', ''multi-select'')
Insert into #temp values (8,1,''Schemes must demonstrate that they will support the development of new housing (this includes development of mixed-use sites)'',''Eligibility_Support_New_Housing'',''multi-select'')
Insert into #temp values (9,1,''Units need to be uninhabitable at the point of application '',''Eligibility_Units_Currently_Uninhabitable'',''multi-select'')
Insert into #temp values (10,4,''ass_2_skip_reason'',''Market_Failure_Skip_Reason'',''multi-select'')
Insert into #temp values (11,4,''ass_2_2_pub_sect_more'',''Market_Failure_Public_Sector_Will_Deliver_More'',''multi-select'')
Insert into #temp values (12,4,''ass_2_3_mp'',''Market_Failure_Market_Power1_Other_Developers_Opportunity'',''multi-select'')
Insert into #temp values (13,4,''ass_2_4_mp'',''Market_Failure_Market_Power2_Unattractive_To_Other_Developers'',''multi-select'')
Insert into #temp values (14,4,''ass_2_5_imp'',''Market_Failure_Imperfect_Info1_Private_Approached'',''multi-select'')
Insert into #temp values (15,4,''ass_2_6_imp'',''Market_Failure_Imperfect_Info2_Private_Finance_Inaccessible '',''multi-select'')
Insert into #temp values (16,4,''ass_2_7_coord_failure1'',''Market_Failure_Coordination_Failure1'',''multi-select'')
Insert into #temp values (17,4,''ass_2_8_pub_good1'',''Market_Failure_Public_Good1_Type'',''multi-select'')
Insert into #temp values (18,4,''ass_2_8_pub_good2'',''Market_Failure_Public_Good2_Holding_Back_Delivery'',''multi-select'')
Insert into #temp values (19,4,''ass_2_9_ext_1'',''Market_Failure_Externalities1_Additional_Benefit'',''multi-select'')
Insert into #temp values (20,3,''ass_2_eco_result'',''Market_Failure_Economics_Result'',''multi-select'')
Insert into #temp values (21,3,''ass_2_eco_2'',''Market_Failure_Economics_Key_Points'', ''textarea'')
Insert into #temp values (22,2,''ass_5_pos_inter'',''Hierachy_Proposed_Intervention_Type'',''multi-select'')
Insert into #temp values (29,2,''ass_5_loan_commercial'',''Hierachy_Loan_Commercial'',''multi-select'')
Insert into #temp values (30,2,''ass_5_loan_ano_partner'',''Hierachy_Loan_Another_Party'',''multi-select'')
Insert into #temp values (33,8,''ass_3sw_result'',''SWOT_Intervention_Result'',''multi-select'')
Insert into #temp values (34,8,''ass_3sw_options_summary'',''SWOT_Commentary'',''multi-select'')
Insert into #temp values (35,7,''ass_4_pj_mid_point_del'',''VFM2_Mid_Delivery_Point'',''multi-select'')
Insert into #temp values (36,7,''ass_4_pj_years_del_no_he'',''VFM2_Years_Delayed_Without_HE'',''whole number'')
Insert into #temp values (37,7,''ass_4_local_authority'',''VFM2_Local_Authority'',''text'')
Insert into #temp values (38,7,''ass_4_homes'',''VFM2_Number_Homes'',''number'')
Insert into #temp values (39,7,''ass_4_afford_homes'',''VFM2_Number_Affordable_Homes'',''whole number'')
Insert into #temp values (40,7,''ass_4_invest'',''VFM2_Funding_Ask'',''currency'')
Insert into #temp values (41,7,''ass_4_land_type'',''VFM2_Land_Type'',''calculated-option'')
Insert into #temp values (45,7,''ass_4_max_ipu'',''VFM2_Maximum_Loan'',''calculation'')
Insert into #temp values (46,7,''ass_4_addition'',''VFM2_Additionality'',''calculation'')
Insert into #temp values (69,6,''ass_4_eco_sensitivity'',''VFM2_Economics_Sensitivity_Analysis'',''multi-select'')
Insert into #temp values (70,6,''ass_4_eco_npsv'',''VFM2_Economics_NPSV'',''number'')
Insert into #temp values (71,6,''ass_4_eco_npsv_c'',''VFM2_Economics_NPSV_C'',''number'')
Insert into #temp values (72,6,''ass_4_eco_bcr'',''VFM2_Economics_BCR'',''number'')
Insert into #temp values (73,6,''ass_4_eco_result'',''VFM2_Economics_Result'',''multi-select'')
Insert into #temp values (74,6,''ass_4_eco_commentray'',''VFM2_Economics_Commentary'',''textarea'')
Insert into #temp values (76,5,''ass_3_1_hou_accel'',''Strategic_Fit2_Delivery_Housing_Unlocked_Or_Accelerated'',''multi-select'')
Insert into #temp values (77,5,''ass_3_2_unlo_hous'',''Strategic_Fit2_Delivery_Targets_Greatest_Need_Area'',''multi-select'')
Insert into #temp values (78,5,''ass_3_3_sme'',''Strategic_Fit2_Delivery_Supports_SME'' ,''multi-select'')
Insert into #temp values (79,5,''ass_3_4_mmc'',''Strategic_Fit2_Design_Increases_MMC'',''multi-select'')
Insert into #temp values (80,5,''ass_3_5_design'',''Strategic_Fit2_Design_Promotes_High_Quality_Design'',''multi-select'')
Insert into #temp values (82,5,''ass_3_7_brownfield'',''Strategic_Fit2_Placemaking_Unlocks_Brownfield'',''multi-select'')
Insert into #temp values (84,5,''ass_3_11_eco_gro_appr'',''Strategic_Fit2_Wider_Drives_Economic_Growth'',''multi-select'')
Insert into #temp values (86,5,''ass_3_13_pub_place'',''Strategic_Fit2_Placemaking_Targets_Priority_Place'',''multi-select'')
Insert into #temp values (87,5,''ass_3_14_pub_regen'',''Strategic_Fit2_Placemaking_Facilitates_Regeneration_Activity'',''multi-select'')
Insert into #temp values (88,5,''ass_3_15_pub_jobs'',''Strategic_Fit2_Placemaking_Job_Creation'',''multi-select'')
Insert into #temp values (90,5,''ass_3_17_pub_sec'',''Strategic_Fit2_Wider_Unlocks_Public_Sector_Land'',''multi-select'')
Insert into #temp values (95,5,''Increasing Supply. Enable the market to deliver a net increase of new homes, and the speed at which they are delivered. Intervention targeting an area of high demand / affordability pressures?'',''Strategic_Fit2_Intervention_Targets_High_Demand'',''multi-select'')
Insert into #temp values (98,9,''ass_6_1_type'',''Deliverability_Intervention_Product_Type'',''calculated-option'')
Insert into #temp values (99,9,''ass_6_1_fac_amount'',''Deliverability_Investment_Amount'',''currency'')
Insert into #temp values (100,9,''ass_6_1_homes'',''Deliverability_Number_Of_Units'',''number'')
Insert into #temp values (101,9,''ass_6_1_loan_inf_risk_assess'',''Deliverability_Risk_Assessment'',''multi-select'')
Insert into #temp values (105,9,''ass_6_loan_inf_1_market'',''Deliverability_Manage_Market_Conditions'',''multi-select'')
Insert into #temp values (106,9,''ass_6_loan_inf_2_market'',''Deliverability_Market_State'',''multi-select'')
Insert into #temp values (107,9,''ass_6_loan_inf_3_pre_risk'',''Deliverability_Level_Pre_Delivery_Risk'',''multi-select'')
Insert into #temp values (108,9,''ass_6_loan_inf_4_exit'',''Deliverability_Strength_Of_Exit_Strategy'',''multi-select'')
Insert into #temp values (109,9,''ass_6_loan_inf_5_complex'',''Deliverability_Site_Complexity'',''multi-select'')
Insert into #temp values (110,9,''ass_6_loan_inf_6_ipu'',''Deliverability_IPU_Within_Regional_Target'',''multi-select'')
Insert into #temp values (111,9,''ass_6_loan_inf_7_bud'',''Deliverability_Affordable_Within_Budget'',''multi-select'')
Insert into #temp values (112,9,''ass_6_loan_inf_8_pck'',''Deliverability_Funding_Package_Complexity'',''multi-select'')
Insert into #temp values (113,9,''ass_6_loan_inf_9_loan'',''Deliverability_IPU_Within_Regional_Target'',''multi-select'')
Insert into #temp values (114,9,''ass_6_loan_inf_10_crr'',''Deliverability_Applicant_CRR_Rating'',''calculated-option'')
Insert into #temp values (115,9,''ass_6_loan_inf_11_cover'',''Deliverability_Costs_Overrun_Management'',''multi-select'')
Insert into #temp values (116,9,''ass_6_loan_inf_12_man_a'',''Deliverability_Monitoring_Arrangement'',''multi-select'')
Insert into #temp values (117,9,''ass_6_loan_inf_13_ent_str'',''Deliverability_Entity_Structure_Complexity'',''multi-select'')
Insert into #temp values (118,9,''ass_6_loan_inf_14_mgt_t'',''Deliverability_Management_Team_Experience'',''multi-select'')
Insert into #temp values (119,9,''ass_6_loan_inf_15_hds'',''Deliverability_Strength_Of_Delivery_Strategy'',''multi-select'')
Insert into #temp values (156,12,''ass_8_pcs_number'',''VFM4_PCS_Number'',''text'')
Insert into #temp values (157,12,''ass_8_pcs_name'',''VFM4_PCS_Project_Name'',''text'')
Insert into #temp values (158,12,''ass_8_local_authority'',''VFM4_Local_Authority'',''text'')
Insert into #temp values (159,12,''ass_8_land_design'',''VFM4_Land_Type'',''calculated-option'')
Insert into #temp values (166,12,''ass_8_npv_rec'',''VFM4_Present_Value_Receipts'',''calculation'')
Insert into #temp values (167,12,''ass_8_npv_exp'',''VFM4_Present_Value_Expenditure'',''calculation'')
Insert into #temp values (168,12,''ass_8_ipu'',''VFM4_Loan_Amount_Per_Unit'',''calculation'')
Insert into #temp values (169,12,''ass_8_additionality'',''VFM4_Additionality'',''calculation'')
Insert into #temp values (170,12,''ass_8_lvu'',''VFM4_Land_Value_Uplift_Per_Unit'',''calculation'')
Insert into #temp values (171,12,''ass_8_npsv'',''VFM4_Do_Something_NPSV'',''calculation'')
Insert into #temp values (172,12,''ass_8_don_npsv'',''VFM4_Do_Nothing_NPSV'',''calculation'')
Insert into #temp values (175,12,''ass_8_social_value'',''VFM4_Social_Value_Result'',''calculation'')
Insert into #temp values (177,12,''ass_8_val_up'',''VFM4_Land_Value_Uplift'',''calculation'')
Insert into #temp values (178,12,''ass_8_health_pv'',''VFM4_Health_Benefits'',''calculation'')
Insert into #temp values (179,12,''ass_8_neg_am_pv'',''VFM4_Negative_Amenity_Benefit_Gross'',''calculation'')
Insert into #temp values (188,11,''ass_8_eco_why'',''VFM4_Economics_Support_Reason'',''multi-select'')
Insert into #temp values (189,11,''ass_8_eco_homes'',''VFM4_Economics_Number_Homes'',''number'')
Insert into #temp values (191,11,''ass_8_eco_invest'',''VFM4_Economics_Funding_Ask'' ,''currency'')
Insert into #temp values (192,11,''ass_8_eco_landtype'',''VFM4_Economics_Land_Type'',''calculated-option'')
Insert into #temp values (193,11,''ass_8_eco_multi_site'',''VFM4_Economics_Multi_Site'',''multi-select'')
Insert into #temp values (194,11,''ass_8_eco_site_voa'',''VFM4_Economics_Site_Specific_Or_VOA_Data'',''multi-select'')
Insert into #temp values (195,11,''ass_8_eco_lvu_voa'',''VFM4_Economics_Land_Value_Uplift_VOA'',''number'')
Insert into #temp values (196,11,''ass_8_eco_lvu_site'',''VFM4_Economics_Land_Value_Uplift_Site'',''number'')
Insert into #temp values (197,11,''ass_8_eco_addition'',''VFM4_Economics_Additionality'',''number'')
Insert into #temp values (198,11,''ass_8_eco_non_mon_imp'',''VFM4_Economics_Non_Monetised_Impact'',''multi-select'')
Insert into #temp values (199,11,''ass_8_eco_sensitivity'',''VFM4_Economics_Sensitivity_Analysis'',''multi-select'')
Insert into #temp values (200,11,''ass_8_eco_npsv'',''VFM4_Economics_NPSV'',''number'')
Insert into #temp values (201,11,''ass_8_eco_npsv_c'',''VFM4_Economics_NPSV_C'',''number'')
Insert into #temp values (202,11,''ass_4_eco_result'',''VFM4_Economics_Result'',''multi-select'')
Insert into #temp values (203,11,''ass_4_eco_commentray'',''VFM4_Economics_Commentary'',''multi-select'')


 MERGE [dbo].[QuestionDataDictionary] AS Target
    USING #temp	AS Source
    ON Source.Id = Target.Id
    
    -- For Inserts
    WHEN NOT MATCHED BY Target THEN
        INSERT ([Id]
		   ,[QuestionDataDictionaryGroupId]
           ,[Name]
           ,[LegacyName]
           ,[Type]
           ,[Description]
           ,[CreatedDateTime]) 
       VALUES (
	   Source.Id,
	   Source.DataDictionaryGroupId,
	   Source.LegacyName,
	   Source.LegacyName,
	   Source.Type,
	   Source.Type,
	   GETDATE())
    
    -- For Updates
    WHEN MATCHED THEN UPDATE SET
        Target.QuestionDataDictionaryGroupId = Source.DataDictionaryGroupId,
        Target.LegacyName	= Source.LegacyName,
        Target.Name	= Source.LegacyName,
        Target.Type	= Source.Type,
        Target.Description	= Source.Type

    -- For Removing Old Values
WHEN NOT MATCHED BY SOURCE THEN DELETE;

