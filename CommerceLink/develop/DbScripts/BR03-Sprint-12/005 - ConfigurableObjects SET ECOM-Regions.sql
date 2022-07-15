
--'WEU'
update ConfigurableObject
set Description = 'WEU'
where ComValue in ('admin', 'en_be', 'nl_nl', 'en_nl', 'fr_be', 'nl_be', 'en_ee', 'en_pt', 'pt_pt', 'en_mt', 'en_bg', 'en_al', 'mcp', 'en_pl', 'pl_pl', 'en_es', 'es_es', 'en_fr', 'fr_fr', 'en_it', 'it_it', 'en_lu', 'fr_lu', 'en_gb', 'en_ie', 'en_at', 'de_at', 'en_by', 'en_chf', 'fr_chf', 'de_chf', 'it_chf', 'en_hr', 'en_cy', 'tr_cy', 'en_cz', 'cs_cz', 'en_dk', 'da_dk', 'en_dkk', 'da_dkk', 'en_gbp', 'en_de', 'de_de', 'en_gr', 'en_hu', 'hu_hu', 'en_is', 'is_is', 'en_il', 'en_lv', 'en_lt', 'lt_lt', 'en_no', 'nn_no', 'en_ro', 'en_ru', 'ru_ru', 'en_rs', 'en_sk', 'en_si', 'it_si', 'en_za', 'en_se', 'sv_se', 'en_tr', 'tr_tr', 'en_afreur', 'en_afrusd', 'en_defeur', 'en_fi', 'fi_fi', 'en_ua', 'en_hrh', 'hr_hrh') and entitytype = 7;


-- 'SEA'
update ConfigurableObject
set Description = 'SEA'
where ComValue in ('en_kr', 'ko_kr', 'en_jp', 'ja_jp', 'en_sa', 'en_ae', 'en_apac', 'en_men') and entitytype = 7;

-- 'EU2's
update ConfigurableObject
set Description = 'EU2'
where ComValue in ('en_cr', 'es_cr', 'en_do', 'es_do', 'en_ec', 'es_ec', 'en_pa', 'es_pa', 'en_pr', 'es_pr', 'en_uy ', 'es_uy', 'en_ar', 'es_ar', 'en_arp', 'es_arp', 'en_us', 'es_us', 'en_ca', 'fr_ca', 'en_lat') and entitytype = 7;