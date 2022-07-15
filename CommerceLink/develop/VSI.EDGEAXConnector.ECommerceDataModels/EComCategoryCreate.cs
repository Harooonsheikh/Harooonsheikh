using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VSI.EDGEAXConnector.ECommerceDataModels
{
   
    public partial class EomCatalogCategoryEntityCreate : object, System.ComponentModel.INotifyPropertyChanged
    {

        private string nameField;

        private int is_activeField;

        private bool is_activeFieldSpecified;

        private int positionField;

        private bool positionFieldSpecified;

        private string[] available_sort_byField;

        private string custom_designField;

        private int custom_design_applyField;

        private bool custom_design_applyFieldSpecified;

        private string custom_design_fromField;

        private string custom_design_toField;

        private string custom_layout_updateField;

        private string default_sort_byField;

        private string descriptionField;

        private string display_modeField;

        private int is_anchorField;

        private bool is_anchorFieldSpecified;

        private int landing_pageField;

        private bool landing_pageFieldSpecified;

        private string meta_descriptionField;

        private string meta_keywordsField;

        private string meta_titleField;

        private string page_layoutField;

        private string url_keyField;

        private int include_in_menuField;

        private bool include_in_menuFieldSpecified;

        /// <remarks/>
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
                this.RaisePropertyChanged("name");
            }
        }

        /// <remarks/>
        public int is_active
        {
            get
            {
                return this.is_activeField;
            }
            set
            {
                this.is_activeField = value;
                this.RaisePropertyChanged("is_active");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool is_activeSpecified
        {
            get
            {
                return this.is_activeFieldSpecified;
            }
            set
            {
                this.is_activeFieldSpecified = value;
                this.RaisePropertyChanged("is_activeSpecified");
            }
        }

        /// <remarks/>
        public int position
        {
            get
            {
                return this.positionField;
            }
            set
            {
                this.positionField = value;
                this.RaisePropertyChanged("position");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool positionSpecified
        {
            get
            {
                return this.positionFieldSpecified;
            }
            set
            {
                this.positionFieldSpecified = value;
                this.RaisePropertyChanged("positionSpecified");
            }
        }

        /// <remarks/>
        public string[] available_sort_by
        {
            get
            {
                return this.available_sort_byField;
            }
            set
            {
                this.available_sort_byField = value;
                this.RaisePropertyChanged("available_sort_by");
            }
        }

        /// <remarks/>
        public string custom_design
        {
            get
            {
                return this.custom_designField;
            }
            set
            {
                this.custom_designField = value;
                this.RaisePropertyChanged("custom_design");
            }
        }

        /// <remarks/>
        public int custom_design_apply
        {
            get
            {
                return this.custom_design_applyField;
            }
            set
            {
                this.custom_design_applyField = value;
                this.RaisePropertyChanged("custom_design_apply");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool custom_design_applySpecified
        {
            get
            {
                return this.custom_design_applyFieldSpecified;
            }
            set
            {
                this.custom_design_applyFieldSpecified = value;
                this.RaisePropertyChanged("custom_design_applySpecified");
            }
        }

        /// <remarks/>
        public string custom_design_from
        {
            get
            {
                return this.custom_design_fromField;
            }
            set
            {
                this.custom_design_fromField = value;
                this.RaisePropertyChanged("custom_design_from");
            }
        }

        /// <remarks/>
        public string custom_design_to
        {
            get
            {
                return this.custom_design_toField;
            }
            set
            {
                this.custom_design_toField = value;
                this.RaisePropertyChanged("custom_design_to");
            }
        }

        /// <remarks/>
        public string custom_layout_update
        {
            get
            {
                return this.custom_layout_updateField;
            }
            set
            {
                this.custom_layout_updateField = value;
                this.RaisePropertyChanged("custom_layout_update");
            }
        }

        /// <remarks/>
        public string default_sort_by
        {
            get
            {
                return this.default_sort_byField;
            }
            set
            {
                this.default_sort_byField = value;
                this.RaisePropertyChanged("default_sort_by");
            }
        }

        /// <remarks/>
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }

        /// <remarks/>
        public string display_mode
        {
            get
            {
                return this.display_modeField;
            }
            set
            {
                this.display_modeField = value;
                this.RaisePropertyChanged("display_mode");
            }
        }

        /// <remarks/>
        public int is_anchor
        {
            get
            {
                return this.is_anchorField;
            }
            set
            {
                this.is_anchorField = value;
                this.RaisePropertyChanged("is_anchor");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool is_anchorSpecified
        {
            get
            {
                return this.is_anchorFieldSpecified;
            }
            set
            {
                this.is_anchorFieldSpecified = value;
                this.RaisePropertyChanged("is_anchorSpecified");
            }
        }

        /// <remarks/>
        public int landing_page
        {
            get
            {
                return this.landing_pageField;
            }
            set
            {
                this.landing_pageField = value;
                this.RaisePropertyChanged("landing_page");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool landing_pageSpecified
        {
            get
            {
                return this.landing_pageFieldSpecified;
            }
            set
            {
                this.landing_pageFieldSpecified = value;
                this.RaisePropertyChanged("landing_pageSpecified");
            }
        }

        /// <remarks/>
        public string meta_description
        {
            get
            {
                return this.meta_descriptionField;
            }
            set
            {
                this.meta_descriptionField = value;
                this.RaisePropertyChanged("meta_description");
            }
        }

        /// <remarks/>
        public string meta_keywords
        {
            get
            {
                return this.meta_keywordsField;
            }
            set
            {
                this.meta_keywordsField = value;
                this.RaisePropertyChanged("meta_keywords");
            }
        }

        /// <remarks/>
        public string meta_title
        {
            get
            {
                return this.meta_titleField;
            }
            set
            {
                this.meta_titleField = value;
                this.RaisePropertyChanged("meta_title");
            }
        }

        /// <remarks/>
        public string page_layout
        {
            get
            {
                return this.page_layoutField;
            }
            set
            {
                this.page_layoutField = value;
                this.RaisePropertyChanged("page_layout");
            }
        }

        /// <remarks/>
        public string url_key
        {
            get
            {
                return this.url_keyField;
            }
            set
            {
                this.url_keyField = value;
                this.RaisePropertyChanged("url_key");
            }
        }

        /// <remarks/>
        public int include_in_menu
        {
            get
            {
                return this.include_in_menuField;
            }
            set
            {
                this.include_in_menuField = value;
                this.RaisePropertyChanged("include_in_menu");
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.SoapIgnoreAttribute()]
        public bool include_in_menuSpecified
        {
            get
            {
                return this.include_in_menuFieldSpecified;
            }
            set
            {
                this.include_in_menuFieldSpecified = value;
                this.RaisePropertyChanged("include_in_menuSpecified");
            }
        }

        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;

        protected void RaisePropertyChanged(string propertyName)
        {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null))
            {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
   
}
