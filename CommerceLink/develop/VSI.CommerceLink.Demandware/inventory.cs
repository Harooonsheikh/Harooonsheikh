﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.34209
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// 
// This source code was auto-generated by xsd, Version=4.0.30319.33440.
// 
namespace VSI.CommerceLink.Demandware.Inventory {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31", IsNullable=false)]
    public partial class inventory {
        
        private complexTypeInventoryList[] inventorylistField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("inventory-list")]
        public complexTypeInventoryList[] inventorylist {
            get {
                return this.inventorylistField;
            }
            set {
                this.inventorylistField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.InventoryList", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    public partial class complexTypeInventoryList {
        
        private complexTypeHeader headerField;
        
        private complexTypeInventoryRecord[] recordsField;
        
        /// <remarks/>
        public complexTypeHeader header {
            get {
                return this.headerField;
            }
            set {
                this.headerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("record", IsNullable=false)]
        public complexTypeInventoryRecord[] records {
            get {
                return this.recordsField;
            }
            set {
                this.recordsField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Header", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    [System.Xml.Serialization.XmlRootAttribute("header", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31", IsNullable=false)]
    public partial class complexTypeHeader {
        
        private bool defaultinstockField;
        
        private string descriptionField;
        
        private bool usebundleinventoryonlyField;
        
        private bool usebundleinventoryonlyFieldSpecified;
        
        private bool onorderField;
        
        private bool onorderFieldSpecified;
        
        private sharedTypeCustomAttribute[] customattributesField;
        
        private string listidField;
        
        private simpleTypeImportMode modeField;
        
        private bool modeFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("default-instock")]
        public bool defaultinstock {
            get {
                return this.defaultinstockField;
            }
            set {
                this.defaultinstockField = value;
            }
        }
        
        /// <remarks/>
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("use-bundle-inventory-only")]
        public bool usebundleinventoryonly {
            get {
                return this.usebundleinventoryonlyField;
            }
            set {
                this.usebundleinventoryonlyField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool usebundleinventoryonlySpecified {
            get {
                return this.usebundleinventoryonlyFieldSpecified;
            }
            set {
                this.usebundleinventoryonlyFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("on-order")]
        public bool onorder {
            get {
                return this.onorderField;
            }
            set {
                this.onorderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool onorderSpecified {
            get {
                return this.onorderFieldSpecified;
            }
            set {
                this.onorderFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("custom-attributes")]
        [System.Xml.Serialization.XmlArrayItemAttribute("custom-attribute", IsNullable=false)]
        public sharedTypeCustomAttribute[] customattributes {
            get {
                return this.customattributesField;
            }
            set {
                this.customattributesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("list-id")]
        public string listid {
            get {
                return this.listidField;
            }
            set {
                this.listidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public simpleTypeImportMode mode {
            get {
                return this.modeField;
            }
            set {
                this.modeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool modeSpecified {
            get {
                return this.modeFieldSpecified;
            }
            set {
                this.modeFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="sharedType.CustomAttribute", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    public partial class sharedTypeCustomAttribute {
        
        private string[] valueField;
        
        private string[] textField;
        
        private string attributeidField;
        
        private string langField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("value")]
        public string[] value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string[] Text {
            get {
                return this.textField;
            }
            set {
                this.textField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("attribute-id")]
        public string attributeid {
            get {
                return this.attributeidField;
            }
            set {
                this.attributeidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(Form=System.Xml.Schema.XmlSchemaForm.Qualified, Namespace="http://www.w3.org/XML/1998/namespace")]
        public string lang {
            get {
                return this.langField;
            }
            set {
                this.langField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="simpleType.ImportMode", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    public enum simpleTypeImportMode {
        
        /// <remarks/>
        delete,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.InventoryRecord", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    [System.Xml.Serialization.XmlRootAttribute("record", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31", IsNullable=false)]
    public partial class complexTypeInventoryRecord {
        
        private decimal allocationField;
        
        private bool allocationFieldSpecified;
        
        private System.DateTime allocationtimestampField;
        
        private bool allocationtimestampFieldSpecified;
        
        private bool perpetualField;
        
        private bool perpetualFieldSpecified;
        
        private simpleTypeInventoryRecordPreorderBackorderHandling preorderbackorderhandlingField;
        
        private bool preorderbackorderhandlingFieldSpecified;
        
        private decimal preorderbackorderallocationField;
        
        private bool preorderbackorderallocationFieldSpecified;
        
        private System.DateTime instockdateField;
        
        private bool instockdateFieldSpecified;
        
        private System.DateTime instockdatetimeField;
        
        private bool instockdatetimeFieldSpecified;
        
        private decimal atsField;
        
        private bool atsFieldSpecified;
        
        private decimal onorderField;
        
        private bool onorderFieldSpecified;
        
        private decimal turnoverField;
        
        private bool turnoverFieldSpecified;
        
        private sharedTypeCustomAttribute[] customattributesField;
        
        private string productidField;
        
        private simpleTypeImportMode modeField;
        
        private bool modeFieldSpecified;
        
        /// <remarks/>
        public decimal allocation {
            get {
                return this.allocationField;
            }
            set {
                this.allocationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool allocationSpecified {
            get {
                return this.allocationFieldSpecified;
            }
            set {
                this.allocationFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("allocation-timestamp")]
        public System.DateTime allocationtimestamp {
            get {
                return this.allocationtimestampField;
            }
            set {
                this.allocationtimestampField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool allocationtimestampSpecified {
            get {
                return this.allocationtimestampFieldSpecified;
            }
            set {
                this.allocationtimestampFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public bool perpetual {
            get {
                return this.perpetualField;
            }
            set {
                this.perpetualField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool perpetualSpecified {
            get {
                return this.perpetualFieldSpecified;
            }
            set {
                this.perpetualFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("preorder-backorder-handling")]
        public simpleTypeInventoryRecordPreorderBackorderHandling preorderbackorderhandling {
            get {
                return this.preorderbackorderhandlingField;
            }
            set {
                this.preorderbackorderhandlingField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool preorderbackorderhandlingSpecified {
            get {
                return this.preorderbackorderhandlingFieldSpecified;
            }
            set {
                this.preorderbackorderhandlingFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("preorder-backorder-allocation")]
        public decimal preorderbackorderallocation {
            get {
                return this.preorderbackorderallocationField;
            }
            set {
                this.preorderbackorderallocationField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool preorderbackorderallocationSpecified {
            get {
                return this.preorderbackorderallocationFieldSpecified;
            }
            set {
                this.preorderbackorderallocationFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("in-stock-date", DataType="date")]
        public System.DateTime instockdate {
            get {
                return this.instockdateField;
            }
            set {
                this.instockdateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool instockdateSpecified {
            get {
                return this.instockdateFieldSpecified;
            }
            set {
                this.instockdateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("in-stock-datetime")]
        public System.DateTime instockdatetime {
            get {
                return this.instockdatetimeField;
            }
            set {
                this.instockdatetimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool instockdatetimeSpecified {
            get {
                return this.instockdatetimeFieldSpecified;
            }
            set {
                this.instockdatetimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal ats {
            get {
                return this.atsField;
            }
            set {
                this.atsField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool atsSpecified {
            get {
                return this.atsFieldSpecified;
            }
            set {
                this.atsFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("on-order")]
        public decimal onorder {
            get {
                return this.onorderField;
            }
            set {
                this.onorderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool onorderSpecified {
            get {
                return this.onorderFieldSpecified;
            }
            set {
                this.onorderFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public decimal turnover {
            get {
                return this.turnoverField;
            }
            set {
                this.turnoverField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool turnoverSpecified {
            get {
                return this.turnoverFieldSpecified;
            }
            set {
                this.turnoverFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("custom-attributes")]
        [System.Xml.Serialization.XmlArrayItemAttribute("custom-attribute", IsNullable=false)]
        public sharedTypeCustomAttribute[] customattributes {
            get {
                return this.customattributesField;
            }
            set {
                this.customattributesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("product-id")]
        public string productid {
            get {
                return this.productidField;
            }
            set {
                this.productidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public simpleTypeImportMode mode {
            get {
                return this.modeField;
            }
            set {
                this.modeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool modeSpecified {
            get {
                return this.modeFieldSpecified;
            }
            set {
                this.modeFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="simpleType.InventoryRecord.PreorderBackorderHandling", Namespace="http://www.demandware.com/xml/impex/inventory/2007-05-31")]
    public enum simpleTypeInventoryRecordPreorderBackorderHandling {
        
        /// <remarks/>
        none,
        
        /// <remarks/>
        preorder,
        
        /// <remarks/>
        backorder,
    }
}