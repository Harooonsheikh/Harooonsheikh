using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;

namespace VSI.EDGEAXConnector.Common
{
    public class Transformer : INotifyPropertyChanged
    {
        public Transformer()
        {
            this.Properties = new ObservableCollection<TransformerProperty>();
            this.ChildMaps = new List<Transformer>();
        }

        public Type SourceClass { get; set; }
        public Type DestinationClass { get; set; }
        public List<Transformer> ChildMaps { get; set; }
        public ObservableCollection<TransformerProperty> Properties { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class TransformerProperty : INotifyPropertyChanged
    {
        public string Comment { get; set; }
        private CustomConditionalValue _customConditionalValue;
        private BooleanValue _booleanValue;
        private ConstantValue _constantValue;
        private PropertyInfo _destinationProperty;
        private bool _isComplex;
        private bool _isCustomLogic;
        private bool _isComment;

        public bool IsComment
        {
            get { return _isComment; }
            set 
            { 
                _isComment = value;
                RaisePropertyChanged("IsComment");
            }
        }
        private PropertyInfo _sourceProperty;

        public TransformerProperty(PropertyInfo propertyInfo)
        {
            this.DestinationProperty = propertyInfo;
            this.ConstantValue = new ConstantValue();
            this.BooleanValue = new BooleanValue();
            this.CustomConditionalValue = new CustomConditionalValue();
            this.Comment = string.Empty;
            this.IsComment = false;
        }

        public PropertyInfo SourceProperty
        {
            get { return _sourceProperty; }
            set
            {
                _sourceProperty = value;
                RaisePropertyChanged("SourceProperty");
            }
        }

        public PropertyInfo DestinationProperty
        {
            get { return _destinationProperty; }
            set
            {
                _destinationProperty = value;
                RaisePropertyChanged("DestinationProperty");
            }
        }

        public bool IsCustomLogic
        {
            get { return _isCustomLogic; }
            set
            {
                _isCustomLogic = value;
                RaisePropertyChanged("IsCustomLogic");
            }
        }

        public bool IsComplex
        {
            get { return _isComplex; }
            set
            {
                _isComplex = value;
                RaisePropertyChanged("IsComplex");
            }
        }

        public ConstantValue ConstantValue
        {
            get { return _constantValue; }
            set
            {
                _constantValue = value;
                RaisePropertyChanged("ConstantValue");
            }
        }

        public BooleanValue BooleanValue
        {
            get { return _booleanValue; }
            set
            {
                _booleanValue = value;
                RaisePropertyChanged("BooleanValue");
            }
        }

        public CustomConditionalValue CustomConditionalValue
        {
            get { return _customConditionalValue; }
            set
            {
                _customConditionalValue = value;
                RaisePropertyChanged("CutomConditionalValue");
            }
        } 

        public MapTypes MapType { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }

    public class ConstantValue
    {
        public string Value { get; set; }
        public bool UseAsDefault { get; set; }
        public bool IsKeyMapping { get; set; }
        public bool IsConstant { get; set; }
    }

    public class BooleanValue
    {
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }
        public bool IsBoolean { get; set; }
    }

    public class CustomConditionalValue : INotifyPropertyChanged
    {
        public string OperatorValue { get; set; }
        public string ConditionValue { get; set; }
        public string TrueValue { get; set; }
        public string FalseValue { get; set; }
        public bool IsCustomCondition { get; set; }
        public bool IsAdvancedExpression { get; set; }
        public string AdvancedExpression { get; set; }
        public bool IsTrueSourceProperty { get; set; }
        public bool IsFalseSourceProperty { get; set; }

        private PropertyInfo _trueSourceProperty;
        private PropertyInfo _falseSourceProperty;

        public PropertyInfo TrueSourceProperty
        {
            get { return _trueSourceProperty; }
            set
            {
                _trueSourceProperty = value;
                RaiseCustomPropertyChanged("TrueSourceProperty");
            }
        }

        public PropertyInfo FalseSourceProperty
        {
            get { return _falseSourceProperty; }
            set
            {
                _falseSourceProperty = value;
                RaiseCustomPropertyChanged("FalseSourceProperty");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void RaiseCustomPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}