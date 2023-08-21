IF OBJECT_ID(''tempdb..#tempgroup'') IS NOT NULL DROP TABLE #tempgroup

UPDATE [dbo].[Question] SET QuestionDataDictionaryId = NULL

DELETE FROM [QuestionDataDictionary]

CREATE TABLE #tempgroup(
              [Id] [int],
              [Name] [nvarchar](450),
              [CreatedDateTime] [datetime2](7),
              [LastModifiedDateTime] [datetime2](7))

insert into #tempgroup
    values
           (1,''Eligibility S1'',GETDATE(), GETDATE()),
           (3,''Market Failure Economics S1'',GETDATE(), GETDATE()),
           (4,''Market Failure S1'',GETDATE(), GETDATE()),
           (2,''Hierarchy of Intervention S2'',GETDATE(), GETDATE()),
           (5,''Strategic Fit S2'',GETDATE(), GETDATE()),
           (6,''Value For Money Economist S2'',GETDATE(), GETDATE()),
           (7,''Value For Money S2'',GETDATE(), GETDATE()),
           (8,''SWOT Analysis S3'' ,GETDATE(), GETDATE()),
           (9,''Deliverability S4'',GETDATE(), GETDATE()),
           (10,''Strategic Fit S4'',GETDATE(), GETDATE()),
           (11,''Value For Money Economist S4'',GETDATE(), GETDATE()),
           (12,''Value For Money S4'',GETDATE(), GETDATE())

MERGE [dbo].[QuestionDataDictionaryGroup] AS Target
    USING #tempgroup    AS Source
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
        Target.Id     = Source.Id,
        Target.Name            = Source.Name,
        Target.LastModifiedDateTime           = Source.LastModifiedDateTime

   WHEN NOT MATCHED BY SOURCE THEN DELETE;

IF OBJECT_ID(''tempdb..#temp'') IS NOT NULL DROP TABLE #temp

CREATE TABLE #temp
(
    [Id] [int], 
              [DataDictionaryGroupId] [int],
              [LegacyName] [nvarchar](250),
    [Name] [nvarchar](250),
              [Type] [nvarchar](250)
)

Insert into #temp values (2,1,''ass_1_3_funding_no_progress'',''Eligibility_No_Progress_Without_Funding'','''')
Insert into #temp values (3,1,''ass_1_2_engalnd'',''Eligibility_Development_In_England'','''')
Insert into #temp values (4,1,''ass_1_4_over_5'',''Eligibility_Over_5_Homes'','''')
Insert into #temp values (5,1,''ass_1_6_noleaseholdhomes'',''Eligibility_No_Leasehold_Homes'','''')
Insert into #temp values (6,1,''ass_1_7_uk_company'',''Eligibility_Registered_UK_Company'','''')
Insert into #temp values (10,1,NULL,''Eligibility_Support_New_Housing'','''')
Insert into #temp values (11,1,NULL,''Eligibility_Units_Currently_Uninhabitable'','''')
Insert into #temp values (15,4,''ass_2_skip_reason'',''Market_Failure_Skip_Reason'','''')
Insert into #temp values (16,4,''ass_2_2_pub_sect_more'',''Market_Failure_Public_Sector_Will_Deliver_More'','''')
Insert into #temp values (17,4,''ass_2_3_mp'',''Market_Failure_Market_Power1_Other_Developers_Opportunity'','''')
Insert into #temp values (18,4,''ass_2_4_mp'',''Market_Failure_Market_Power2_Unattractive_To_Other_Developers'','''')
Insert into #temp values (19,4,''ass_2_5_imp'',''Market_Failure_Imperfect_Info1_Private_Approached'','''')
Insert into #temp values (20,4,''ass_2_6_imp'',''Market_Failure_Imperfect_Info2_Private_Finance_Inaccessible '','''')
Insert into #temp values (21,4,''ass_2_7_coord_failure1'',''Market_Failure_Coordination_Failure1'','''')
Insert into #temp values (22,4,''ass_2_8_pub_good1'',''Market_Failure_Public_Good1_Type'','''')
Insert into #temp values (23,4,''ass_2_8_pub_good2'',''Market_Failure_Public_Good2_Holding_Back_Delivery'','''')
Insert into #temp values (24,4,''ass_2_9_ext_1'',''Market_Failure_Externalities1_Additional_Benefit'','''')
Insert into #temp values (30,3,''ass_2_eco_result'',''Market_Failure_Economics_Result'','''')
Insert into #temp values (31,3,''ass_2_eco_2'',''Market_Failure_Economics_Key_Points'','''')
Insert into #temp values (37,2,''ass_5_pos_inter'',''Hierachy_Proposed_Intervention_Type'','''')
Insert into #temp values (44,2,''ass_5_loan_commercial'',''Hierachy_Loan_Commercial'','''')
Insert into #temp values (45,2,''ass_5_loan_ano_partner'',''Hierachy_Loan_Another_Party'','''')
Insert into #temp values (53,8,''ass_3sw_result'',''SWOT_Intervention_Result'','''')
Insert into #temp values (54,8,''ass_3sw_options_summary'',''SWOT_Commentary'','''')
Insert into #temp values (60,7,''ass_4_pj_mid_point_del'',''VFM2_Mid_Delivery_Point'','''')
Insert into #temp values (61,7,''ass_4_pj_years_del_no_he'',''VFM2_Years_Delayed_Without_HE'','''')
Insert into #temp values (62,7,''ass_4_local_authority'',''VFM2_Local_Authority'','''')
Insert into #temp values (63,7,''ass_4_homes'',''VFM2_Number_Homes'','''')
Insert into #temp values (64,7,''ass_4_afford_homes'',''VFM2_Number_Affordable_Homes'','''')
Insert into #temp values (65,7,''ass_4_invest'',''VFM2_Funding_Ask'','''')
Insert into #temp values (66,7,''ass_4_land_type'',''VFM2_Land_Type'','''')
Insert into #temp values (70,7,''ass_4_max_ipu'',''VFM2_Maximum_Loan'','''')
Insert into #temp values (71,7,''ass_4_addition'',''VFM2_Additionality'','''')
Insert into #temp values (72,7,''ass_4_npsv'',''VFM2_Do_Something_NPSV'','''')
Insert into #temp values (73,7,''ass_4_don_npsv'',''VFM2_Do_Nothing_NPSV'','''')
Insert into #temp values (74,7,''ass_4_result'',''VFM2_Social_Value_Result'','''')
Insert into #temp values (81,7,NULL,''VFM2_Area_High_Demand'','''')
Insert into #temp values (82,7,NULL,''VFM2_Percentage_Affordable'','''')
Insert into #temp values (83,7,NULL,''VFM2_NPSV_C'','''')
Insert into #temp values (87,6,''ass_4_eco_why'',''VFM2_Economics_Support_Reason'','''')
Insert into #temp values (88,6,''ass_4_eco_homes'',''VFM2_Economics_Number_Homes'','''')
Insert into #temp values (89,6,''ass_4_eco_afford_homes'',''VFM2_Economics_Number_Affordable_Homes'','''')
Insert into #temp values (90,6,''ass_4_eco_invest'',''VFM2_Economics_Funding_Ask'','''')
Insert into #temp values (91,6,''ass_4_eco_landtype'',''VFM2_Economics_Land_Type'','''')
Insert into #temp values (92,6,''ass_4_eco_multi_site'',''VFM2_Economics_Multi_Site'','''')
Insert into #temp values (93,6,''ass_4_eco_site_voa'',''VFM2_Economics_Site_Specific_Or_VOA_Data'','''')
Insert into #temp values (94,6,''ass_4_eco_lvu_voa'',''VFM2_Economics_Land_Value_Uplift_VOA'','''')
Insert into #temp values (95,6,''ass_4_eco_lvu_site'',''VFM2_Economics_Land_Value_Uplift_Site'','''')
Insert into #temp values (96,6,''ass_4_eco_addition'',''VFM2_Economics_Additionality'','''')
Insert into #temp values (97,6,''ass_4_eco_non_mon_imp'',''VFM2_Economics_Non_Monetised_Impact'','''')
Insert into #temp values (98,6,''ass_4_eco_sensitivity'',''VFM2_Economics_Sensitivity_Analysis'','''')
Insert into #temp values (99,6,''ass_4_eco_npsv'',''VFM2_Economics_NPSV'','''')
Insert into #temp values (100,6,''ass_4_eco_npsv_c'',''VFM2_Economics_NPSV_C'','''')
Insert into #temp values (101,6,''ass_4_eco_bcr'',''VFM2_Economics_BCR'','''')
Insert into #temp values (102,6,''ass_4_eco_result'',''VFM2_Economics_Result'','''')
Insert into #temp values (103,6,''ass_4_eco_commentray'',''VFM2_Economics_Commentary'','''')
Insert into #temp values (110,5,''ass_3_1_hou_accel'',''Strategic_Fit2_Delivery_Housing_Unlocked_Or_Accelerated'','''')
Insert into #temp values (111,5,''ass_3_2_unlo_hous'',''Strategic_Fit2_Delivery_Targets_Greatest_Need_Area'','''')
Insert into #temp values (112,5,''ass_3_3_sme'',''Strategic_Fit2_Delivery_Supports_SME'','''')
Insert into #temp values (113,5,''ass_3_4_mmc'',''Strategic_Fit2_Design_Increases_MMC'','''')
Insert into #temp values (114,5,''ass_3_5_design'',''Strategic_Fit2_Design_Promotes_High_Quality_Design'','''')
Insert into #temp values (116,5,''ass_3_7_brownfield'',''Strategic_Fit2_Placemaking_Unlocks_Brownfield'','''')
Insert into #temp values (118,5,''ass_3_11_eco_gro_appr'',''Strategic_Fit2_Wider_Drives_Economic_Growth'','''')
Insert into #temp values (120,5,''ass_3_13_pub_place'',''Strategic_Fit2_Placemaking_Targets_Priority_Place'','''')
Insert into #temp values (121,5,''ass_3_14_pub_regen'',''Strategic_Fit2_Placemaking_Facilitates_Regeneration_Activity'','''')
Insert into #temp values (122,5,''ass_3_15_pub_jobs'',''Strategic_Fit2_Placemaking_Job_Creation'','''')
Insert into #temp values (124,5,''ass_3_17_pub_sec'',''Strategic_Fit2_Wider_Unlocks_Public_Sector_Land'','''')
Insert into #temp values (130,5,NULL,''Strategic_Fit2_Intervention_Targets_High_Demand'','''')
Insert into #temp values (136,9,''ass_6_1_type'',''Deliverability_Intervention_Product_Type'','''')
Insert into #temp values (137,9,''ass_6_1_fac_amount'',''Deliverability_Investment_Amount'','''')
Insert into #temp values (138,9,''ass_6_1_homes'',''Deliverability_Number_Of_Units'','''')
Insert into #temp values (139,9,''ass_6_1_loan_inf_risk_assess'',''Deliverability_Risk_Assessment'','''')
Insert into #temp values (143,9,''ass_6_loan_inf_1_market'',''Deliverability_Manage_Market_Conditions'','''')
Insert into #temp values (144,9,''ass_6_loan_inf_2_market'',''Deliverability_Market_State'','''')
Insert into #temp values (145,9,''ass_6_loan_inf_3_pre_risk'',''Deliverability_Level_Pre_Delivery_Risk'','''')
Insert into #temp values (146,9,''ass_6_loan_inf_4_exit'',''Deliverability_Strength_Of_Exit_Strategy'','''')
Insert into #temp values (147,9,''ass_6_loan_inf_5_complex'',''Deliverability_Site_Complexity'','''')
Insert into #temp values (148,9,''ass_6_loan_inf_6_ipu'',''Deliverability_IPU_Within_Regional_Target'','''')
Insert into #temp values (149,9,''ass_6_loan_inf_7_bud'',''Deliverability_Affordable_Within_Budget'','''')
Insert into #temp values (150,9,''ass_6_loan_inf_8_pck'',''Deliverability_Funding_Package_Complexity'','''')
Insert into #temp values (151,9,''ass_6_loan_inf_9_loan'',''Deliverability_Max_Loan_Plus_Recycling_Plus_Other_Debt'','''')
Insert into #temp values (152,9,''ass_6_loan_inf_10_crr'',''Deliverability_Applicant_CRR_Rating'','''')
Insert into #temp values (153,9,''ass_6_loan_inf_11_cover'',''Deliverability_Costs_Overrun_Management'','''')
Insert into #temp values (154,9,''ass_6_loan_inf_12_man_a'',''Deliverability_Monitoring_Arrangement'','''')
Insert into #temp values (155,9,''ass_6_loan_inf_13_ent_str'',''Deliverability_Entity_Structure_Complexity'','''')
Insert into #temp values (156,9,''ass_6_loan_inf_14_mgt_t'',''Deliverability_Management_Team_Experience'','''')
Insert into #temp values (157,9,''ass_6_loan_inf_15_hds'',''Deliverability_Strength_Of_Delivery_Strategy'','''')
Insert into #temp values (204,12,''ass_8_pcs_number'',''VFM4_PCS_Number'','''')
Insert into #temp values (205,12,''ass_8_pcs_name'',''VFM4_PCS_Project_Name'','''')
Insert into #temp values (206,12,''ass_8_local_authority'',''VFM4_Local_Authority'','''')
Insert into #temp values (207,12,''ass_8_land_design'',''VFM4_Land_Type'','''')
Insert into #temp values (214,12,''ass_8_npv_rec'',''VFM4_Present_Value_Receipts'','''')
Insert into #temp values (215,12,''ass_8_npv_exp'',''VFM4_Present_Value_Expenditure'','''')
Insert into #temp values (216,12,''ass_8_ipu'',''VFM4_Loan_Amount_Per_Unit'','''')
Insert into #temp values (217,12,''ass_8_additionality'',''VFM4_Additionality'','''')
Insert into #temp values (218,12,''ass_8_lvu'',''VFM4_Land_Value_Uplift_Per_Unit'','''')
Insert into #temp values (219,12,''ass_8_npsv'',''VFM4_Do_Something_NPSV'','''')
Insert into #temp values (220,12,''ass_8_don_npsv'',''VFM4_Do_Nothing_NPSV'','''')
Insert into #temp values (223,12,''ass_8_social_value'',''VFM4_Social_Value_Result'','''')
Insert into #temp values (225,12,''ass_8_val_up'',''VFM4_Land_Value_Uplift'','''')
Insert into #temp values (226,12,''ass_8_health_pv'',''VFM4_Health_Benefits'','''')
Insert into #temp values (227,12,''ass_8_neg_am_pv'',''VFM4_Negative_Amenity_Benefit_Gross'','''')
Insert into #temp values (239,11,''ass_8_eco_why'',''VFM4_Economics_Support_Reason'','''')
Insert into #temp values (240,11,''ass_8_eco_homes'',''VFM4_Economics_Number_Homes'','''')
Insert into #temp values (241,11,''ass_8_eco_afford_homes'',''VFM4_Economics_Number_Affordable_Homes'','''')
Insert into #temp values (242,11,''ass_8_eco_invest'',''VFM4_Economics_Funding_Ask'','''')
Insert into #temp values (243,11,''ass_8_eco_landtype'',''VFM4_Economics_Land_Type'','''')
Insert into #temp values (244,11,''ass_8_eco_multi_site'',''VFM4_Economics_Multi_Site'','''')
Insert into #temp values (245,11,''ass_8_eco_site_voa'',''VFM4_Economics_Site_Specific_Or_VOA_Data'','''')
Insert into #temp values (246,11,''ass_8_eco_lvu_voa'',''VFM4_Economics_Land_Value_Uplift_VOA'','''')
Insert into #temp values (247,11,''ass_8_eco_lvu_site'',''VFM4_Economics_Land_Value_Uplift_Site'','''')
Insert into #temp values (248,11,''ass_8_eco_addition'',''VFM4_Economics_Additionality'','''')
Insert into #temp values (249,11,''ass_8_eco_non_mon_imp'',''VFM4_Economics_Non_Monetised_Impact'','''')
Insert into #temp values (250,11,''ass_8_eco_sensitivity'',''VFM4_Economics_Sensitivity_Analysis'','''')
Insert into #temp values (251,11,''ass_8_eco_npsv'',''VFM4_Economics_NPSV'','''')
Insert into #temp values (252,11,''ass_8_eco_npsv_c'',''VFM4_Economics_NPSV_C'','''')
Insert into #temp values (253,11,''ass_4_eco_result'',''VFM4_Economics_Result'','''')
Insert into #temp values (254,11,''ass_4_eco_commentray'',''VFM4_Economics_Commentary'','''')


MERGE [dbo].[QuestionDataDictionary] AS Target
    USING #temp AS Source
    ON Source.Id = Target.Id
    
    -- For Inserts
    WHEN NOT MATCHED BY Target THEN
        INSERT ([Id]
                                ,[QuestionDataDictionaryGroupId]
           ,[Name]
           ,[LegacyName]
           ,[Type]
           ,[Description]
           ,[CreatedDateTime]
                                ,[LastModifiedDateTime]) 
       VALUES (
                 Source.Id,
                 Source.DataDictionaryGroupId,
                 Source.Name,
                 Source.LegacyName,
                 Source.Type,
                 Source.Type,
                 GETDATE(),
                 GETDATE())
    
    -- For Updates
    WHEN MATCHED THEN UPDATE SET
        Target.QuestionDataDictionaryGroupId = Source.DataDictionaryGroupId,
        Target.LegacyName = Source.LegacyName,
        Target.Name            = Source.Name,
        Target.Type = Source.Type,
        Target.Description  = Source.Type

    -- For Removing Old Values
WHEN NOT MATCHED BY SOURCE THEN DELETE;
