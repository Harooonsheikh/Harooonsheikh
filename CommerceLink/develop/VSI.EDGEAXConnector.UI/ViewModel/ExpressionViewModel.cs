using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using VSI.EDGEAXConnector.Common;

namespace VSI.EDGEAXConnector.UI.ViewModel
{
    public class ExpressionViewModel :BaseViewModel
    {
        private TransformerProperty _property;

        public TransformerProperty Property
        {
            get { return _property; }
            set
            {
                _property = value;
                RaisePropertyChanged("Property");
            }
        }
        public ObservableCollection<PropertyInfo> SourceProperties { get; set; }
        public PropertyInfo SelectedSourceProperty { get; set; }
        
    }
}
