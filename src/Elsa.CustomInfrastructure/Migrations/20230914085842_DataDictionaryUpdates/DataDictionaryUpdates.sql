----UPDATES
Update [QuestionDataDictionary] Set LegacyName=''ass_1_2_england'' where Id=3
Update [QuestionDataDictionary] Set LegacyName=''ass_1_6_no_leasehold_homes'' where Id=5
Update [QuestionDataDictionary] Set LegacyName=''ass_2_7_coord_fail1'' where Id=21
Update [QuestionDataDictionary] Set LegacyName=''ass_3w_option_summary'' where Id=54
Update [QuestionDataDictionary] Set LegacyName=''ass_4_eco_land_type'' where Id=91
Update [QuestionDataDictionary] Set LegacyName=''ass_4_eco_commentary'' where Id=103
Update [QuestionDataDictionary] Set LegacyName=''ass_3_13_place'' where Id=120
Update [QuestionDataDictionary] Set LegacyName=''ass_3_14_regen'' where Id=121
Update [QuestionDataDictionary] Set LegacyName=''ass_3_15_jobs'' where Id=122
Update [QuestionDataDictionary] Set LegacyName=''ass_6_loan_inf_risk_assess'' where Id=139
Update [QuestionDataDictionary] Set LegacyName=''ass_8_land_desig'' where Id=207
Update [QuestionDataDictionary] Set LegacyName=''ass_8_social_val'' where Id=223
Update [QuestionDataDictionary] Set LegacyName=''ass_8_eco_land_type'' where Id=243
Update [QuestionDataDictionary] Set LegacyName=''ass_8_eco_multi_site'' where Id=244
Update [QuestionDataDictionary] Set LegacyName=''ass_8_eco_commentary'' where Id=254

---Delete
Delete FROM [QuestionDataDictionary] where Name= ''VFM2_Area_High_Demand''											--Remove from List	VFM2_Area_High_Demand
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Delivery_Targets_Greatest_Need_Area''			--Removed from List	Strategic_Fit2_Delivery_Targets_Greatest_Need_Area
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Design_Increases_MMC''							--Removed from List	Strategic_Fit2_Design_Increases_MMC
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Placemaking_Unlocks_Brownfield''					--Removed from List	Strategic_Fit2_Placemaking_Unlocks_Brownfield
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Supports_Tenure_Diversification''--COULD NOT FIND IN CURRENT LIST --Removed from List	Strategic_Fit2_Supports_Tenure_Diversification
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Wider_Drives_Economic_Growth''					--Removed from List	Strategic_Fit2_Wider_Drives_Economic_Growth
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Contributes_Target_1Million_Homes_May2024''--COULD NOT FIND IN CURRENT LIST --Removed from List	Strategic_Fit2_Contributes_Target_1Million_Homes_May2024
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Placemaking_Targets_Priority_Place''				--Removed from List	Strategic_Fit2_Placemaking_Targets_Priority_Place
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Placemaking_Job_Creation''						--Removed from List	Strategic_Fit2_Placemaking_Job_Creation
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Wider_Unlocks_Public_Sector_Land''				--Removed from List	Strategic_Fit2_Wider_Unlocks_Public_Sector_Land
Delete FROM [QuestionDataDictionary] where Name= ''Strategic_Fit2_Intervention_Targets_High_Demand''				--Removed from List	Strategic_Fit2_Intervention_Targets_High_Demand
Delete FROM [QuestionDataDictionary] where Name= ''VFM4_Land_Value_Uplift''										--Remove from List	Land_Value_Uplift
Delete FROM [QuestionDataDictionary] where Name= ''VFM4_Health_Benefits''											--Remove from List	Health_Benefits
Delete FROM [QuestionDataDictionary] where Name= ''VFM4_Negative_Amenity_Benefit_Gross''							--Remove from List	VFM4_Negative_Amenity_Benefit_Gross

---Add new ones to 5	Strategic Fit S2
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (255,5,''ass_3_1_hou_accel'',''Strategic_Fit2_Delivery_Housing_Unlocked_Or_Accelerated'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (256,5,''ass_3_3_sme'',''Strategic_Fit2_Delivery_Supports_SME'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (257,5,''ass_3_4_mmc'',''Strategic_Fit2_Site_Design_Increases_MMC'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (258,5,''ass_3_4_mmc'',''Strategic_Fit2_Partner_Design_Increases_MMC'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (259,5,''ass_3_5_design'',''Strategic_Fit2_Design_Promotes_High_Quality_Design'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (260,5,''ass_3_6_pri_sect'',''Strategic_Fit2_Leveraging_Private_Sector_Finance'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (261,5,''ass_3_7_brownfield'',''Strategic_Fit2_Site_Placemaking_Unlocks_Brownfield'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (262,5,''ass_3_7_brownfield'',''Strategic_Fit2_Partner_Placemaking_Unlocks_Brownfield'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (263,5,''ass_3_13_pub_place'',''Strategic_Fit2_Site_Placemaking_Targets_Priority_Place'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (264,5,NULL,''Strategic_Fit2_Partner_Placemaking_Supports_Regeneration_Area'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (265,5,''ass_3_14_pub_regen'',''Strategic_Fit2_Placemaking_Facilitates_Regeneration_Activity'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (266,5,''ass_3_16_pub_sustain'',''Strategic_Fit2_Sustainable_Net_Zero_Carbon_Schemes'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (267,5,''ass_3_17_pub_sec'',''Strategic_Fit2_Site_Unlocks_Public_Sector_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (268,5,''ass_3_17_pub_sec'',''Strategic_Fit2_Partner_Unlocks_Public_Sector_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (269,5,NULL,''Strategic_Fit2_Site_Support_Diversification_And_Local_Housing_Needs'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (270,5,NULL,''Strategic_Fit2_Partner_Support_Diversification_And_Local_Housing_Needs'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (271,5,NULL,''Strategic_Fit2_Site_Intervention_Targets_High_Demand'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (272,5,NULL,''Strategic_Fit2_Partner_Intervention_Targets_High_Demand'',GetDate());

---Add new ones to 10	Strategic Fit S4
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (273,10,''ass_7_1_type'',''Strategic_Fit4_Specific_Intervention_Type'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (274,10,''ass_7_funding_ask'',''Strategic_Fit4_Funding_Ask'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (275,10,''ass_7_units'',''Strategic_Fit4_Number_Of_Units'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (276,10,''ass_7_loan_inf_1'',''Strategic_Fit4_Site_Delivery_Housing_Unlocked_Or_Accelerated'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (277,10,''ass_7_loan_inf_1'',''Strategic_Fit4_Partner_Delivery_Housing_Unlocked_Or_Accelerated'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (278,10,''ass_7_loan_inf_2_high_dem'',''Strategic_Fit4_Site_Intervention_Targets_High_Demand'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (279,10,''ass_7_loan_inf_2_high_dem'',''Strategic_Fit4_Site_LA_Intervention_Targets_High_Demand'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (280,10,''ass_7_loan_inf_2_high_dem'',''Strategic_Fit4_Partner_Intervention_Targets_High_Demand'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (281,10,''ass_7_loan_inf_3_sme'',''Strategic_Fit4_Site_Delivery_Supports_SME'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (282,10,''ass_7_loan_inf_3_sme'',''Strategic_Fit4_Partner_Delivery_Supports_SME'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (283,10,''ass_7_loan_inf_4_pdl'',''Strategic_Fit4_Site_Unlocks_Previously_Developed_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (284,10,''ass_7_loan_inf_4_pdl'',''Strategic_Fit4_Partner_Unlocks_Previously_Developed_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (285,10,''ass_7_loan_inf_5_place'',''Strategic_Fit4_Site_Regeneration_Place'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (286,10,''ass_7_loan_inf_5_place'',''Strategic_Fit4_Partner_Regeneration_Place'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (287,10,''ass_7_loan_inf_6_regen'',''Strategic_Fit4_Site_Facilitate_Economic_Growth_Regeneration_Activity'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (288,10,''ass_7_loan_inf_6_regen'',''Strategic_Fit4_Partner_Facilitate_Economic_Growth_Regeneration_Activity'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (289,10,''ass_7_loan_inf_7_employ'',''Strategic_Fit4_Site_Total_Employment_Floorspace_Created'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (290,10,NULL,''Strategic_Fit4_Site_Total_Employment_Floorspace_Per_Home'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (291,10,''ass_7_loan_inf_8_mmc'',''Strategic_Fit4_Site_Supports_Use_MMC'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (292,10,''ass_7_loan_inf_8_mmc'',''Strategic_Fit4_Partner_Supports_Use_MMC'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (293,10,''ass_7_loan_inf_9_qual'',''Strategic_Fit4_Site_High_Quality_Well_Designed_Scheme'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (294,10,''ass_7_loan_inf_9_qual'',''Strategic_Fit4_Partner_High_Quality_Well_Designed_Scheme'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (295,10,''ass_7_loan_inf_10_sd'',''Strategic_Fit4_Site_Sustainable_Design'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (296,10,''ass_7_loan_inf_10_sd'',''Strategic_Fit4_Partner_Sustainable_Design'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (297,10,''ass_7_loan_inf_11_ce'',''Strategic_Fit4_Site_Reducing_Carbon_Emissions'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (298,10,''ass_7_loan_inf_11_ce'',''Strategic_Fit4_Partner_Supports_Net_Zero_Carbon_By_2050'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (299,10,''ass_7_loan_inf_12_netz'',''Strategic_Fit4_Partner_Achieving_Net_Zero_Carbon_Steps'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (300,10,''ass_7_loan_inf_13_psl'',''Strategic_Fit4_Site_Unlocks_Public_Sector_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (301,10,''ass_7_loan_inf_13_psl'',''Strategic_Fit4_Partner_Unlocks_Public_Sector_Land'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (302,10,''ass_7_loan_inf_14_priv_inv'',''Strategic_Fit4_Total_Private_Sector_Funding'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (303,10,''ass_7_loan_inf_14_pri_f'',''Strategic_Fit4_Private_Sector_Funding_Leverage_Calculation'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (304,10,NULL,''Strategic_Fit4_Site_Supports_Diversification_And_Local_Housing_Needs'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (305,10,NULL,''Strategic_Fit4_Partner_Supports_Diversification_And_Local_Housing_Needs'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (306,10,NULL,''Strategic_Fit4_Partner_Environmental_Social_Governance_Strategy'',GetDate());

---Add new ones to 12	Value For Money S4
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (307,12,''ass_8_npsv_c'',''VFM4_NPSV_C_Rounded'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (308,12,''ass_8_npsv_c_full'',''VFM4_NPSV_C_Unrounded'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (309,12,''ass_8_val_up'',''VFM4_Do_Something_Land_Value_Uplift'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (310,12,''ass_8_don_val_up'',''VFM4_Do_Nothing_Land_Value_Uplift'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (311,12,''ass_8_health_pv'',''VFM4_Do_Something_Health_Benefits'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (312,12,''ass_8_don_health_pv'',''VFM4_Do_Nothing_Health_Benefits'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (313,12,''ass_8_neg_am_pv'',''VFM4_Do_Something_Negative_Amenity_Benefit_Gross'',GetDate());
Insert into [QuestionDataDictionary] ([Id],[QuestionDataDictionaryGroupId],[LegacyName],[Name],[CreatedDateTime]) values (314,12,''ass_8_don_neg_am_pv'',''VFM4_Do_Nothing_Negative_Amenity_Benefit_Gross'',GetDate());