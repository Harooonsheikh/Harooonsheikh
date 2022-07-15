Instructions for XML Creation Template :
======================================== 
1-	Constant Value		constant=”true”

2-	Default Value		default-value=”value”

3-	Show Hide Node		show-node=”true/false” also support conditional expression

4-	Multiple files based template 
	file-source=”file-path” data-source=”object-property”

5-	Custom Attributes (node name must be customAttributes)

6-	Mapping of specific node from customAttributes list via key-name 
	6.1- Add custom-attribute-value="true" attribute on node and write KeyName as node inner text e.g.
		<min-order-quantity default-value="1" custom-attribute-value="true">minOrderQuantity</min-order-quantity>
	OR
	6.2- (sourceObject.CustomAttributes.Find(s =>;s.Key.Equals("key-name")).Value) 
	Note: 6.1 is prefered because it run-time cost is low 6.2 create delay but for condition 6.2 is prefered e.g.
	<searchable-flag default-value="true" expression="true">sourceObject.CustomAttributes.Find(s =&gt;s.Key.Equals("searchable")).Value == "0" ? "false":"true"</searchable-flag>

7-	List based repeated area 
	repeat=”true/false” data-source=”list-name”

8-	Must use sourceObject before any property in expression e.g. 
	sourceObject.taxvat == null ? &quot;0&quot; : sourceObject.taxvat

9-	Attribute Expression also applicable e.g. 
	attribute-id="{{ConfigurationHelper.TaxGroup}}"'

10-	Must use data-object with application expression if you are using any object property in expression e.g.
	<shared-variation-attribute data-object="ErpProductDimensionSet~DimensionKey" attribute-id='{{ConfigurationHelper.DimensionSets.Find(d => d.ErpValue.Equals(sourceObject.DimensionKey)).ComValue}}' />

11-	To get any ApplicationSettings table's value use ConfigurationHelper.Key e.g.
	For Attribute Value:
	<product-lineitem tax-group-id="{{ConfigurationHelper.TaxGroup}}"></product-lineitem>
	For Node Data Value:
	<product-lineitem> 
		<tax-group>ConfigurationHelper.TaxGroup</tax-group>
	</product-lineitem>

12-	