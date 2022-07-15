using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using VSI.EDGEAXConnector.Common;
using VSI.EDGEAXConnector.UI.ViewModel;

namespace VSI.EDGEAXConnector.UI
{
    /// <summary>
    ///     Interaction logic for NullWindow.xaml
    /// </summary>
    public partial class NullWindow : Window
    {
        private readonly List<PropertyInfo> sourceBackupProperties;

        public ObservableCollection<PropertyInfo> ObLstSourceProperties = new ObservableCollection<PropertyInfo>();
        public ObservableCollection<PropertyInfo> ObLstDestinationProperties = new ObservableCollection<PropertyInfo>();

        private string strDestinationClassName = string.Empty;
        private string strSourceClassName = string.Empty;

        public NullWindow()
        {
            InitializeComponent();
        }

        public NullWindow(TransformerProperty property, List<PropertyInfo> properties, Type destinationClass, Type sourceClass, List<Type> destEntities, List<Type> srcEntities)
        {
            InitializeComponent();
            this.ViewModel = new ExpressionViewModel
            {
                Property = property,
                SourceProperties = new ObservableCollection<PropertyInfo>(properties),
                SelectedSourceProperty = property.SourceProperty
            };
            this.DataContext = this.ViewModel;
            sourceBackupProperties = properties;
            cboBoolSource.ItemsSource =
                this.ViewModel.SourceProperties.Where(p => p.PropertyType.Name == typeof (Boolean).Name).ToList();
            this.Title = "Mapping for " + property.DestinationProperty.Name + " (" +
                         property.DestinationProperty.PropertyType.Name + ")";

            cboCustomSource.ItemsSource = cboCustomTrueSource.ItemsSource = cboCustomFalseSource.ItemsSource =
                this.ViewModel.SourceProperties.Where(p => p.PropertyType.Name == typeof(Boolean).Name ||
                    p.PropertyType.Name == typeof(String).Name || p.PropertyType.Name == typeof(Int32).Name ||
                    p.PropertyType.Name == typeof(Int64).Name || p.PropertyType.Name == typeof(Decimal).Name ||
                    p.PropertyType.Name == typeof(Double).Name || p.PropertyType.Name == typeof(Single).Name ||
                    p.PropertyType.Name == typeof(Char).Name || p.PropertyType.Name == typeof(Byte).Name ||
                    p.PropertyType.Name == typeof(Int16).Name).ToList();

            strDestinationClassName = destinationClass.Name;
            strSourceClassName = sourceClass.Name;

            cboDest.ItemsSource = destEntities;
            lstDestProperties.ItemsSource = destinationClass.GetProperties().OrderBy(p=>p.Name).ToList();
            cboSrc.ItemsSource = srcEntities;
            lstSrcProperties.ItemsSource = this.ViewModel.SourceProperties;
        }

        private ExpressionViewModel ViewModel { get; set; }

        private void txtValue_LostFocus(object sender, RoutedEventArgs e)
        {
            var tbox = (TextBox) sender;

            var destType = this.ViewModel.Property.DestinationProperty.PropertyType;
            var convertedValue = ConvertTo(tbox.Text, destType);
            if (Convert.ToString(convertedValue) == "-111")
            {
                tbox.Style = (Style) FindResource("textBoxErrorStyle");
                tbox.ToolTip = tbox.Text + " cannot be assiged to " + this.ViewModel.Property.DestinationProperty.Name;
                tbox.UpdateLayout();
                //Property.ShowError = Visibility.Visible;
            }
            else
            {
                if (this.ViewModel.Property.ConstantValue.UseAsDefault && cboSourceProp.SelectedItem == null)
                {
                    MessageBox.Show("Please select the source property from dropdown");
                }
                else if (this.ViewModel.Property.ConstantValue.IsKeyMapping && cboSourcePropforKeys.SelectedItem == null)
                {
                    MessageBox.Show("Please select the source property from dropdown");
                }
                else
                {
                    tbox.Style = (Style) FindResource("textBoxNormalStyle");
                    tbox.UpdateLayout();
                    this.ViewModel.Property.SourceProperty = cboSourceProp.SelectedItem as PropertyInfo;
                }
                //Property.ShowError = Visibility.Collapsed;
            }
        }

        private void txtConstant_LostFocus(object sender, RoutedEventArgs e)
        {
            var tbox = (TextBox) sender;

            var destType = this.ViewModel.Property.DestinationProperty.PropertyType;
            var convertedValue = ConvertTo(tbox.Text, destType);
            if (Convert.ToString(convertedValue) == "-111")
            {
                tbox.Style = (Style) FindResource("textBoxErrorStyle");
                tbox.ToolTip = tbox.Text + " cannot be assiged to " + this.ViewModel.Property.DestinationProperty.Name;
                tbox.UpdateLayout();
                //Property.ShowError = Visibility.Visible;
            }
            else
            {
                tbox.Style = (Style) FindResource("textBoxNormalStyle");
                tbox.UpdateLayout();
                this.ViewModel.Property.ConstantValue.Value = tbox.Text;
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            this.ViewModel.Property.BooleanValue.IsBoolean = false;
            this.ViewModel.Property.CustomConditionalValue.IsCustomCondition = false;
            this.ViewModel.Property.IsCustomLogic = false;
            this.ViewModel.RaisePropertyChanged("CutomConditionalValue");

            this.ViewModel.Property.ConstantValue.UseAsDefault =
            this.ViewModel.Property.ConstantValue.IsConstant =
            this.ViewModel.Property.ConstantValue.IsKeyMapping = false;

            if (tabDefaultValue.IsSelected)
            {
                if(txtValue.Text.Length > 0)
                    this.ViewModel.Property.ConstantValue.UseAsDefault = true;
                this.ViewModel.Property.ConstantValue.IsConstant =
                this.ViewModel.Property.ConstantValue.IsKeyMapping = false;
            }
            else if (tabConstantValue.IsSelected)
            {
                if (txtConstant.Text.Length > 0)
                    this.ViewModel.Property.ConstantValue.IsConstant = true;
                this.ViewModel.Property.ConstantValue.UseAsDefault =
                this.ViewModel.Property.ConstantValue.IsKeyMapping = false;

                this.ViewModel.Property.SourceProperty = null;
            }
            else if (tabAttributeKey.IsSelected)
            {
                if (txtKey.Text.Length > 0)
                    this.ViewModel.Property.ConstantValue.IsKeyMapping = true;
                this.ViewModel.Property.ConstantValue.UseAsDefault =
                this.ViewModel.Property.ConstantValue.IsConstant = false;

                //var filteredList = this.ViewModel.SourceProperties.Where(
                //    p =>
                //        p.PropertyType.IsGenericType &&
                //        (
                //        //p.PropertyType.GetGenericArguments()
                //        //    .FirstOrDefault()
                //        //    .Name.Equals(typeof (CommerceProperty).Name) ||
                //         p.PropertyType.GetGenericArguments()
                //             .FirstOrDefault()
                //             .Name.Equals(typeof (KeyValuePair<string, string>).Name))).ToList();
                //this.ViewModel.SourceProperties = new ObservableCollection<PropertyInfo>(filteredList);
                //this.ViewModel.RaisePropertyChanged("SourceProperties");
            }
            else if (tabBooleanValue.IsSelected)
            {
                this.ViewModel.Property.BooleanValue.IsBoolean = true;
                this.ViewModel.Property.MapType = MapTypes.BooleanValues;
            }
            else if (tabCustomeConditionalValue.IsSelected)
            {
                this.ViewModel.Property.CustomConditionalValue.IsCustomCondition = true;
                this.ViewModel.RaisePropertyChanged("CutomConditionalValue");
                this.ViewModel.Property.MapType = MapTypes.CustomExpression;
                this.ViewModel.Property.IsCustomLogic = true;
            }
            else
            {
                this.ViewModel.SourceProperties = new ObservableCollection<PropertyInfo>(sourceBackupProperties);
                this.ViewModel.RaisePropertyChanged("SourceProperties");
            }

            Close();
        }

        public static dynamic ConvertTo(dynamic source, Type dest)
        {
            try
            {
                return Convert.ChangeType(source, dest);
            }
            catch
            {
                return -111;
            }
        }
        /*
        private void option_OnChecked(object sender, RoutedEventArgs e)
        {            
            if (rbAttributeKey.IsChecked == true)
            {
                //var filteredList = this.ViewModel.SourceProperties.Where(
                //    p =>
                //        p.PropertyType.IsGenericType &&
                //        (
                //        //p.PropertyType.GetGenericArguments()
                //        //    .FirstOrDefault()
                //        //    .Name.Equals(typeof (CommerceProperty).Name) ||
                //         p.PropertyType.GetGenericArguments()
                //             .FirstOrDefault()
                //             .Name.Equals(typeof (KeyValuePair<string, string>).Name))).ToList();
                //this.ViewModel.SourceProperties = new ObservableCollection<PropertyInfo>(filteredList);
                //this.ViewModel.RaisePropertyChanged("SourceProperties");
            }
            else if (rbBooleanKey.IsChecked == true)
            {
                this.ViewModel.Property.MapType = MapTypes.BooleanValues;
            }
            else
            {
                this.ViewModel.SourceProperties = new ObservableCollection<PropertyInfo>(sourceBackupProperties);
                this.ViewModel.RaisePropertyChanged("SourceProperties");
            }
        }
        */
        
        private void chkTrueSourceProperty_Click(object sender, RoutedEventArgs e)
        {
            if (chkTrueSourceProperty.IsChecked == true)
            {
                cboCustomTrueSource.Visibility = System.Windows.Visibility.Visible;
                txtCustomTrueValue.Visibility = System.Windows.Visibility.Collapsed;

                this.ViewModel.Property.CustomConditionalValue.IsTrueSourceProperty = true;
                this.ViewModel.Property.CustomConditionalValue.TrueValue = string.Empty;
            }
            else
            {
                cboCustomTrueSource.Visibility = System.Windows.Visibility.Collapsed;
                txtCustomTrueValue.Visibility = System.Windows.Visibility.Visible;

                this.ViewModel.Property.CustomConditionalValue.IsTrueSourceProperty = false;
                this.ViewModel.Property.CustomConditionalValue.TrueSourceProperty = null;                
            }
            this.ViewModel.RaisePropertyChanged("CutomConditionalValue");
        }

        private void chkFalseSourceProperty_Click(object sender, RoutedEventArgs e)
        {
            if (chkFalseSourceProperty.IsChecked == true)
            {
                cboCustomFalseSource.Visibility = System.Windows.Visibility.Visible;
                txtCustomFalseValue.Visibility = System.Windows.Visibility.Collapsed;

                this.ViewModel.Property.CustomConditionalValue.IsFalseSourceProperty = true;
                this.ViewModel.Property.CustomConditionalValue.FalseValue = string.Empty;
            }
            else
            {
                cboCustomFalseSource.Visibility = System.Windows.Visibility.Collapsed;
                txtCustomFalseValue.Visibility = System.Windows.Visibility.Visible;

                this.ViewModel.Property.CustomConditionalValue.IsFalseSourceProperty = false;
                this.ViewModel.Property.CustomConditionalValue.FalseSourceProperty = null;
            }
            this.ViewModel.RaisePropertyChanged("CutomConditionalValue");

        }
        
        private void cboOperatorValue_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cboOperatorValue.SelectedValue != null)
            {
                if (cboOperatorValue.SelectedValue.Equals("IsNullOrEmpty"))
                {
                    txtConditionValue.IsEnabled = false;
                }
                else
                {
                    txtConditionValue.IsEnabled = true;
                }
            }
        }

        private void chkAdvancedExpression_Click(object sender, RoutedEventArgs e)
        {
            if(chkAdvancedExpression.IsChecked == true)
            {
                gbAdvancedExpression.Visibility = System.Windows.Visibility.Visible;
                gbCustomExpression.Visibility = System.Windows.Visibility.Collapsed;
            }
            else
            {
                gbAdvancedExpression.Visibility = System.Windows.Visibility.Collapsed;
                gbCustomExpression.Visibility = System.Windows.Visibility.Visible;
            }
        }

        private void cboSrc_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedSource = cboSrc.SelectedValue as Type;
            ObLstSourceProperties.Clear();
            foreach(var p in selectedSource.GetProperties().OrderBy(p => p.Name))
            {
                ObLstSourceProperties.Add(p);
            }
            lstSrcProperties.ItemsSource = ObLstSourceProperties;
        }

        private void cboDest_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedDest = cboDest.SelectedValue as Type;
            ObLstDestinationProperties.Clear();
            foreach (var p in selectedDest.GetProperties().OrderBy(p => p.Name))
            {
                ObLstDestinationProperties.Add(p);
            }
            lstDestProperties.ItemsSource = ObLstDestinationProperties;
        }


        private void btnClearAdvancedExpression_Click(object sender, RoutedEventArgs e)
        {
            txtAdvancedExpression.Text = string.Empty;
        }

        private void lstSrcProperties_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedSourceProperty = lstSrcProperties.SelectedItem as PropertyInfo;
            if(selectedSourceProperty != null)
            {
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                string strSource = "src.";
                if (cboSrc.SelectedIndex != -1 && !(cboSrc.SelectedItem as Type).Name.Equals(strSourceClassName))
                {
                    strSource = (cboSrc.SelectedItem as Type).Name + ".";
                }
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, strSource + selectedSourceProperty.Name);
            }
        }

        private void lstDestProperties_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var selectedDestinationProperty = lstDestProperties.SelectedItem as PropertyInfo;
            if (selectedDestinationProperty != null)
            {
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                string strDest = "dest.";
                if (cboDest.SelectedIndex != -1 && !(cboDest.SelectedItem as Type).Name.Equals(strDestinationClassName))
                {
                    strDest = " " + (cboSrc.SelectedItem as Type).Name + ".";
                }
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, strDest + selectedDestinationProperty.Name);
            }
        }

        private void lstComparisionOperators_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstComparisionOperators.SelectedValue != null)
            {
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, lstComparisionOperators.SelectedValue.ToString());
            }
        }

        private void lstLogicalOperators_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstLogicalOperators.SelectedValue != null)
            {
                string strOperator = "";

                if(lstLogicalOperators.SelectedValue.ToString() == "AND")
                {
                    strOperator = "&&";
                }
                if(lstLogicalOperators.SelectedValue.ToString() == "OR")
                {
                    strOperator = "||";
                }
                if(lstLogicalOperators.SelectedValue.ToString() == "NOT")
                {
                    strOperator = "!";
                }
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, strOperator);
            }
        }

        private void lstFunctions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstFunctions.SelectedValue != null)
            {
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, "." + lstFunctions.SelectedValue);
            }
        }

        private void lstConstantValues_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstConstantValues.SelectedValue != null)
            {
                int sIndex = txtAdvancedExpression.SelectionStart;
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex," " + lstConstantValues.SelectedValue);
            }
        }

        private void lstConditionalStructures_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (lstConditionalStructures.SelectedValue != null)
            {
                if (lstConditionalStructures.SelectedValue.Equals("IF"))
                {
                    int sIndex = txtAdvancedExpression.SelectionStart;
                    txtAdvancedExpression.Text = txtAdvancedExpression.Text.Remove(sIndex, txtAdvancedExpression.SelectionLength);
                    txtAdvancedExpression.Text = txtAdvancedExpression.Text.Insert(sIndex, "Condition ? TrueValue : FalseValue");
                }
            }

        }

        private void CommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
        {
            this.Close();
        }

    }
}