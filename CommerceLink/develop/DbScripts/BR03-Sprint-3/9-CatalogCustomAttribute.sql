update MappingTemplate set [XML] = N'<Items>
  <item repeat="true" data-source="ErpCatalog~Products" product-id="ErpProduct~ItemId" mode="ErpProduct~Mode">
    <sku>ErpProduct~SKU</sku>
    <store_view_code>ErpProduct~StoreViewCode</store_view_code>
    <attribute_set_code>TeamViewer</attribute_set_code>
    <product_type expression="true">sourceObject.IsMasterProduct ? "configurable" : "virtual"</product_type>
    <tmv_product_type expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "TMVProductType").Value) ? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "TMVProductType").Value == "0" ? "Others" : sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "TMVProductType").Value == "1" ? "Primary" : sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "TMVProductType").Value == "2" ? "Add-Ons" : string.Empty) : string.Empty</tmv_product_type>
    <categories>All</categories>
    <product_websites />
    <name expression="true"> sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Ecom Product Name").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Ecom Product Name").Value : sourceObject.ProductName</name>
    <description>ErpProduct~Description</description>
    <short_description expression="true"> sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Ecom Product Name").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Ecom Product Name").Value : sourceObject.ProductName</short_description>
    <weight />
    <product_online>1</product_online>
    <default_unit_of_measure expression="true">sourceObject.Rules != null ? sourceObject.Rules.DefaultUnitOfMeasure : string.Empty</default_unit_of_measure>
    <tax_class_name>Taxable Goods</tax_class_name>
    <visibility expression="true">sourceObject.IsMasterProduct ? "Catalog, Search" : "Catalog"</visibility>
    <price>ErpProduct~Price</price>
    <url_key>ErpProduct~SKU</url_key>
    <display_product_options_in>Block after Info Column</display_product_options_in>
    <gift_message_available>Use config</gift_message_available>
    <additional_attributes expression="true">sourceObject.IsMasterProduct ? string.Empty : "billing_intervals=" + sourceObject.ColorId + ",offer_types=" + sourceObject.StyleId</additional_attributes>
    <erp_attributes expression="true"> String.IsNullOrEmpty(sourceObject.Locale) ? ( sourceObject.CustomAttributes != null ? (string.Join(",", sourceObject.CustomAttributes.Select(att=&gt;att.Key + "=" +  (att.Value!=null? (att.Key == "mobile_description"? "&lt;![CDATA[" +att.Value+ "]]&gt;" :(att.Key == "plp_description"? "&lt;![CDATA[" +att.Value+ "]]&gt;" :att.Value)):string.Empty)))) : string.Empty ) : string.Empty </erp_attributes>
    <offer_months>ErpProduct~ReplenishmentWeight</offer_months>
    <qty>0.0000</qty>
    <out_of_stock_qty>0</out_of_stock_qty>
    <use_config_min_qty>1</use_config_min_qty>
    <use_config_backorders>1</use_config_backorders>
    <is_qty_decimal>0</is_qty_decimal>
    <allow_backorders>0</allow_backorders>
    <use_config_backorders>1</use_config_backorders>
    <min_cart_qty>ErpProduct~LowestQty</min_cart_qty>
    <use_config_min_sale_qty>1</use_config_min_sale_qty>
    <max_cart_qty>ErpProduct~HighestQty</max_cart_qty>
    <use_config_max_sale_qty>1</use_config_max_sale_qty>
    <is_in_stock>1</is_in_stock>
    <notify_on_stock_below>1.0000</notify_on_stock_below>
    <use_config_notify_stock_qty>1</use_config_notify_stock_qty>
    <manage_stock>0</manage_stock>
    <use_config_manage_stock>1</use_config_manage_stock>
    <use_config_qty_increments>1</use_config_qty_increments>
    <qty_increments>1.0000</qty_increments>
    <use_config_enable_qty_inc>1</use_config_enable_qty_inc>
    <enable_qty_increments>0</enable_qty_increments>
    <is_decimal_divided>0</is_decimal_divided>
    <deferred_stock_update>1</deferred_stock_update>
    <use_config_deferred_stock_update>1</use_config_deferred_stock_update>
    <related_skus />
    <crosssell_skus expression="true">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.AddOn).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</crosssell_skus>
    <upsell_skus expression="true">String.IsNullOrEmpty(sourceObject.Locale) ? (  (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.UpsellTypeId == ErpUpsellType.Upsell).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</upsell_skus>
    <configurable_variations expression="true">String.IsNullOrEmpty(sourceObject.Locale) ? ( sourceObject.IsMasterProduct &amp;&amp; sourceObject.Variants!=null?string.Join(",", sourceObject.Variants.Select(v=&gt;"sku=" + v.SKU + ",billing_intervals=" + v.ColorId + ",offer_types=" + v.StyleId)):string.Empty) : string.Empty </configurable_variations>
    <configurable_variation_labels expression="true">String.IsNullOrEmpty(sourceObject.Locale) ? (sourceObject.IsMasterProduct? "billing_intervals=Billing Intervals,offer_types=Offer Types" : string.Empty) : string.Empty</configurable_variation_labels>
    <cl_update_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.Update).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_update_skus>
    <cl_upgrade_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.Upgrade).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_upgrade_skus>
    <cl_migrate_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.Migration).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_migrate_skus>
    <cl_switch_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.Switch).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_switch_skus>
    <cl_related_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.Related).OrderBy(ui =&gt; ui.Priority).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_related_skus>
    <cl_multi_main_skus expression="true" show-node="String.IsNullOrEmpty(sourceObject.Locale)">String.IsNullOrEmpty(sourceObject.Locale) ? ( (sourceObject.UpsellItems != null &amp;&amp; sourceObject.UpsellItems.Count &gt; 0) ? string.Join(",", sourceObject.UpsellItems.Where(ui =&gt; ui.TMVCrosssellType == ErpTMVCrosssellType.MultiMain).OrderBy(ui =&gt; ui.Priority).Select(ui =&gt; ui.LinkedProductSKU)) : string.Empty) : string.Empty</cl_multi_main_skus>
    <cl_bundle_skus show-node="String.IsNullOrEmpty(sourceObject.Locale)">
      <bundle_sku repeat="true" data-source="ErpProduct~UpsellItems" show-node="sourceObject.TMVCrosssellType == ErpTMVCrosssellType.Bundle">
        <sku expression="true">sourceObject.TMVCrosssellType == ErpTMVCrosssellType.Bundle ? sourceObject.LinkedProductSKU : string.Empty</sku>
        <quantity expression="true">sourceObject.TMVCrosssellType == ErpTMVCrosssellType.Bundle ? sourceObject.TMVCrosssellBundleQuantity : 0</quantity>
      </bundle_sku>
    </cl_bundle_skus>
    <cl_qty_text expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Text").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Text").Value : string.Empty</cl_qty_text>
    <cl_qty_breakpoints expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Breakpoints").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Breakpoints").Value : string.Empty</cl_qty_breakpoints>
    <cl_qty_label_text expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Label").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Quantity Label").Value : string.Empty</cl_qty_label_text>
    <cl_split_item expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_split_item").Value) ? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_split_item").Value == "True" ? "Yes" : "No" ) : string.Empty</cl_split_item>
    <cl_config_body_content expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_config_body_content").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_config_body_content").Value : string.Empty</cl_config_body_content>
    <cl_config_header_content expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_config_header_content").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_config_header_content").Value : string.Empty</cl_config_header_content>
    <cart_description expression="true">sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cart_description").Value) ? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cart_description").Value : string.Empty</cart_description>
    <is_addon expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_addon").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_addon").Value == "True" ? "Yes" : "No" ) : string.Empty
	</is_addon>
    <is_show_qty_selector expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_show_qty_selector").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_show_qty_selector").Value == "True" ? "Yes" : "No" ) : string.Empty
	</is_show_qty_selector>
    <concurrent_users expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Concurrent Users").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "Concurrent Users").Value
			: "0"
	</concurrent_users>
    <is_channel expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_channel").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_channel").Value == "True" ? "Yes" : "No" )
			: string.Empty
	</is_channel>
    <is_main_product expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_main_product").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_main_product").Value == "True" ? "Yes" : "No" )
			: string.Empty
	</is_main_product>
    <cl_qty_single_qty_text expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_qty_single_qty_text").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_qty_single_qty_text").Value
			: string.Empty
	</cl_qty_single_qty_text>
    <cl_show_qty_summary expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_show_qty_summary").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_show_qty_summary").Value == "True" ? "Yes" : "No" )
			: string.Empty
	</cl_show_qty_summary>
    <is_cart_remove expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_cart_remove").Value)
			? (sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "is_cart_remove").Value == "True" ? "Yes" : "No" )
			: string.Empty
	</is_cart_remove>
    <logo_link_url expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "logo_link_url").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "logo_link_url").Value
			: string.Empty
	</logo_link_url>
    <plp_description expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "plp_description").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "plp_description").Value
			: string.Empty
	</plp_description>
    <mobile_description expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "mobile_description").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "mobile_description").Value
			: string.Empty
	</mobile_description>
    <cl_addon_header_text expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_addon_header_text").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_addon_header_text").Value
			: string.Empty
	</cl_addon_header_text>
    <cl_addon_body_text expression="true">
		sourceObject.CustomAttributes != null &amp;&amp; !string.IsNullOrWhiteSpace(sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_addon_body_text").Value)
			? sourceObject.CustomAttributes.FirstOrDefault(att =&gt; att.Key == "cl_addon_body_text").Value
			: string.Empty
	</cl_addon_body_text>
  </item>
</Items>' where MappingTemplateTypeId = 1 and SourceEntity = 'ErpCatalog'