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
namespace VSI.CommerceLink.Demandware.Customer {
    using System.Xml.Serialization;
    
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType=true, Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31", IsNullable=false)]
    public partial class customers {
        
        private complexTypeCustomer[] customerField;
        
        private complexTypeCustomerGroupAssignment[] groupassignmentField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("customer")]
        public complexTypeCustomer[] customer {
            get {
                return this.customerField;
            }
            set {
                this.customerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("group-assignment")]
        public complexTypeCustomerGroupAssignment[] groupassignment {
            get {
                return this.groupassignmentField;
            }
            set {
                this.groupassignmentField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Customer", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    [System.Xml.Serialization.XmlRootAttribute("customer", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31", IsNullable=false)]
    public partial class complexTypeCustomer {
        
        private complexTypeCredentials credentialsField;
        
        private complexTypeProfile profileField;
        
        private complexTypeAddress[] addressesField;
        
        private complexTypeCustomerGroup[] customergroupsField;
        
        private string noteField;
        
        private string customernoField;
        
        private simpleTypeImportMode modeField;
        
        private bool modeFieldSpecified;
        
        /// <remarks/>
        public complexTypeCredentials credentials {
            get {
                return this.credentialsField;
            }
            set {
                this.credentialsField = value;
            }
        }
        
        /// <remarks/>
        public complexTypeProfile profile {
            get {
                return this.profileField;
            }
            set {
                this.profileField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("address", IsNullable=false)]
        public complexTypeAddress[] addresses {
            get {
                return this.addressesField;
            }
            set {
                this.addressesField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlArrayAttribute("customer-groups")]
        [System.Xml.Serialization.XmlArrayItemAttribute("customer-group", IsNullable=false)]
        public complexTypeCustomerGroup[] customergroups {
            get {
                return this.customergroupsField;
            }
            set {
                this.customergroupsField = value;
            }
        }
        
        /// <remarks/>
        public string note {
            get {
                return this.noteField;
            }
            set {
                this.noteField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("customer-no")]
        public string customerno {
            get {
                return this.customernoField;
            }
            set {
                this.customernoField = value;
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
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Credentials", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public partial class complexTypeCredentials {
        
        private string loginField;
        
        private complexTypePassword passwordField;
        
        private bool enabledflagField;
        
        private bool enabledflagFieldSpecified;
        
        private string passwordquestionField;
        
        private string passwordanswerField;
        
        private string provideridField;
        
        private string externalidField;
        
        /// <remarks/>
        public string login {
            get {
                return this.loginField;
            }
            set {
                this.loginField = value;
            }
        }
        
        /// <remarks/>
        public complexTypePassword password {
            get {
                return this.passwordField;
            }
            set {
                this.passwordField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("enabled-flag")]
        public bool enabledflag {
            get {
                return this.enabledflagField;
            }
            set {
                this.enabledflagField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool enabledflagSpecified {
            get {
                return this.enabledflagFieldSpecified;
            }
            set {
                this.enabledflagFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("password-question")]
        public string passwordquestion {
            get {
                return this.passwordquestionField;
            }
            set {
                this.passwordquestionField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("password-answer")]
        public string passwordanswer {
            get {
                return this.passwordanswerField;
            }
            set {
                this.passwordanswerField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("provider-id")]
        public string providerid {
            get {
                return this.provideridField;
            }
            set {
                this.provideridField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("external-id")]
        public string externalid {
            get {
                return this.externalidField;
            }
            set {
                this.externalidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Password", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public partial class complexTypePassword {
        
        private bool encryptedField;
        
        private simpleTypeEncryptionScheme encryptionSchemeField;
        
        private bool encryptionSchemeFieldSpecified;
        
        private string valueField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool encrypted {
            get {
                return this.encryptedField;
            }
            set {
                this.encryptedField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public simpleTypeEncryptionScheme encryptionScheme {
            get {
                return this.encryptionSchemeField;
            }
            set {
                this.encryptionSchemeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool encryptionSchemeSpecified {
            get {
                return this.encryptionSchemeFieldSpecified;
            }
            set {
                this.encryptionSchemeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlTextAttribute()]
        public string Value {
            get {
                return this.valueField;
            }
            set {
                this.valueField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="simpleType.EncryptionScheme", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public enum simpleTypeEncryptionScheme {
        
        /// <remarks/>
        md5,
        
        /// <remarks/>
        scrypt,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.CustomerGroup", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public partial class complexTypeCustomerGroup {
        
        private string groupidField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("group-id")]
        public string groupid {
            get {
                return this.groupidField;
            }
            set {
                this.groupidField = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Address", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public partial class complexTypeAddress {
        
        private string salutationField;
        
        private string titleField;
        
        private string firstnameField;
        
        private string secondnameField;
        
        private string lastnameField;
        
        private string suffixField;
        
        private string companynameField;
        
        private string jobtitleField;
        
        private string address1Field;
        
        private string address2Field;
        
        private string suiteField;
        
        private string postboxField;
        
        private string cityField;
        
        private string postalcodeField;
        
        private string statecodeField;
        
        private string countrycodeField;
        
        private string phoneField;
        
        private sharedTypeCustomAttribute[] customattributesField;
        
        private string addressidField;
        
        private bool preferredField;
        
        private bool preferredFieldSpecified;
        
        /// <remarks/>
        public string salutation {
            get {
                return this.salutationField;
            }
            set {
                this.salutationField = value;
            }
        }
        
        /// <remarks/>
        public string title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("first-name")]
        public string firstname {
            get {
                return this.firstnameField;
            }
            set {
                this.firstnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("second-name")]
        public string secondname {
            get {
                return this.secondnameField;
            }
            set {
                this.secondnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("last-name")]
        public string lastname {
            get {
                return this.lastnameField;
            }
            set {
                this.lastnameField = value;
            }
        }
        
        /// <remarks/>
        public string suffix {
            get {
                return this.suffixField;
            }
            set {
                this.suffixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("company-name")]
        public string companyname {
            get {
                return this.companynameField;
            }
            set {
                this.companynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("job-title")]
        public string jobtitle {
            get {
                return this.jobtitleField;
            }
            set {
                this.jobtitleField = value;
            }
        }
        
        /// <remarks/>
        public string address1 {
            get {
                return this.address1Field;
            }
            set {
                this.address1Field = value;
            }
        }
        
        /// <remarks/>
        public string address2 {
            get {
                return this.address2Field;
            }
            set {
                this.address2Field = value;
            }
        }
        
        /// <remarks/>
        public string suite {
            get {
                return this.suiteField;
            }
            set {
                this.suiteField = value;
            }
        }
        
        /// <remarks/>
        public string postbox {
            get {
                return this.postboxField;
            }
            set {
                this.postboxField = value;
            }
        }
        
        /// <remarks/>
        public string city {
            get {
                return this.cityField;
            }
            set {
                this.cityField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("postal-code")]
        public string postalcode {
            get {
                return this.postalcodeField;
            }
            set {
                this.postalcodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("state-code")]
        public string statecode {
            get {
                return this.statecodeField;
            }
            set {
                this.statecodeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("country-code")]
        public string countrycode {
            get {
                return this.countrycodeField;
            }
            set {
                this.countrycodeField = value;
            }
        }
        
        /// <remarks/>
        public string phone {
            get {
                return this.phoneField;
            }
            set {
                this.phoneField = value;
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
        [System.Xml.Serialization.XmlAttributeAttribute("address-id")]
        public string addressid {
            get {
                return this.addressidField;
            }
            set {
                this.addressidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public bool preferred {
            get {
                return this.preferredField;
            }
            set {
                this.preferredField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool preferredSpecified {
            get {
                return this.preferredFieldSpecified;
            }
            set {
                this.preferredFieldSpecified = value;
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="sharedType.CustomAttribute", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
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
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.Profile", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public partial class complexTypeProfile {
        
        private string salutationField;
        
        private string titleField;
        
        private string firstnameField;
        
        private string secondnameField;
        
        private string lastnameField;
        
        private string suffixField;
        
        private string companynameField;
        
        private string jobtitleField;
        
        private string emailField;
        
        private string phonehomeField;
        
        private string phonebusinessField;
        
        private string phonemobileField;
        
        private string faxField;
        
        private System.DateTime birthdayField;
        
        private bool birthdayFieldSpecified;
        
        private string genderField;
        
        private System.DateTime creationdateField;
        
        private bool creationdateFieldSpecified;
        
        private System.DateTime lastlogintimeField;
        
        private bool lastlogintimeFieldSpecified;
        
        private System.DateTime lastvisittimeField;
        
        private bool lastvisittimeFieldSpecified;
        
        private string preferredlocaleField;
        
        private sharedTypeCustomAttribute[] customattributesField;
        
        /// <remarks/>
        public string salutation {
            get {
                return this.salutationField;
            }
            set {
                this.salutationField = value;
            }
        }
        
        /// <remarks/>
        public string title {
            get {
                return this.titleField;
            }
            set {
                this.titleField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("first-name")]
        public string firstname {
            get {
                return this.firstnameField;
            }
            set {
                this.firstnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("second-name")]
        public string secondname {
            get {
                return this.secondnameField;
            }
            set {
                this.secondnameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("last-name")]
        public string lastname {
            get {
                return this.lastnameField;
            }
            set {
                this.lastnameField = value;
            }
        }
        
        /// <remarks/>
        public string suffix {
            get {
                return this.suffixField;
            }
            set {
                this.suffixField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("company-name")]
        public string companyname {
            get {
                return this.companynameField;
            }
            set {
                this.companynameField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("job-title")]
        public string jobtitle {
            get {
                return this.jobtitleField;
            }
            set {
                this.jobtitleField = value;
            }
        }
        
        /// <remarks/>
        public string email {
            get {
                return this.emailField;
            }
            set {
                this.emailField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("phone-home")]
        public string phonehome {
            get {
                return this.phonehomeField;
            }
            set {
                this.phonehomeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("phone-business")]
        public string phonebusiness {
            get {
                return this.phonebusinessField;
            }
            set {
                this.phonebusinessField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("phone-mobile")]
        public string phonemobile {
            get {
                return this.phonemobileField;
            }
            set {
                this.phonemobileField = value;
            }
        }
        
        /// <remarks/>
        public string fax {
            get {
                return this.faxField;
            }
            set {
                this.faxField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="date")]
        public System.DateTime birthday {
            get {
                return this.birthdayField;
            }
            set {
                this.birthdayField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool birthdaySpecified {
            get {
                return this.birthdayFieldSpecified;
            }
            set {
                this.birthdayFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        public string gender {
            get {
                return this.genderField;
            }
            set {
                this.genderField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("creation-date")]
        public System.DateTime creationdate {
            get {
                return this.creationdateField;
            }
            set {
                this.creationdateField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool creationdateSpecified {
            get {
                return this.creationdateFieldSpecified;
            }
            set {
                this.creationdateFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("last-login-time")]
        public System.DateTime lastlogintime {
            get {
                return this.lastlogintimeField;
            }
            set {
                this.lastlogintimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastlogintimeSpecified {
            get {
                return this.lastlogintimeFieldSpecified;
            }
            set {
                this.lastlogintimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("last-visit-time")]
        public System.DateTime lastvisittime {
            get {
                return this.lastvisittimeField;
            }
            set {
                this.lastvisittimeField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool lastvisittimeSpecified {
            get {
                return this.lastvisittimeFieldSpecified;
            }
            set {
                this.lastvisittimeFieldSpecified = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("preferred-locale")]
        public string preferredlocale {
            get {
                return this.preferredlocaleField;
            }
            set {
                this.preferredlocaleField = value;
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
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="simpleType.ImportMode", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    public enum simpleTypeImportMode {
        
        /// <remarks/>
        delete,
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "4.0.30319.33440")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(TypeName="complexType.CustomerGroupAssignment", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31")]
    [System.Xml.Serialization.XmlRootAttribute("group-assignment", Namespace="http://www.demandware.com/xml/impex/customer/2006-10-31", IsNullable=false)]
    public partial class complexTypeCustomerGroupAssignment {
        
        private simpleTypeImportMode modeField;
        
        private bool modeFieldSpecified;
        
        private string groupidField;
        
        private string customernoField;
        
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
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("group-id")]
        public string groupid {
            get {
                return this.groupidField;
            }
            set {
                this.groupidField = value;
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute("customer-no")]
        public string customerno {
            get {
                return this.customernoField;
            }
            set {
                this.customernoField = value;
            }
        }
    }
}
