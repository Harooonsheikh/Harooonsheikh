#pragma checksum "..\..\..\UserControlls\CommerceLinkFiles.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "FC76F768C2007511E547B21EC89A3469C279259EC11FB7BEB5A7BF0B893864C0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace VSI.EDGEAXConnector.UI.UserControlls {
    
    
    /// <summary>
    /// CommerceLinkFiles
    /// </summary>
    public partial class CommerceLinkFiles : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector {
        
        
        #line 20 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ComboBox cmbEntity;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgFailed;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMoveSelected;
        
        #line default
        #line hidden
        
        
        #line 74 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnMoveAll;
        
        #line default
        #line hidden
        
        
        #line 75 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btnRefresh;
        
        #line default
        #line hidden
        
        
        #line 79 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgProcessed;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/VSI.EDGEAXConnector.UI;component/usercontrolls/commercelinkfiles.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.cmbEntity = ((System.Windows.Controls.ComboBox)(target));
            
            #line 20 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
            this.cmbEntity.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(this.cmbEntity_SelectionChanged);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgFailed = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.btnMoveSelected = ((System.Windows.Controls.Button)(target));
            
            #line 73 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
            this.btnMoveSelected.Click += new System.Windows.RoutedEventHandler(this.btnMoveSelected_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.btnMoveAll = ((System.Windows.Controls.Button)(target));
            
            #line 74 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
            this.btnMoveAll.Click += new System.Windows.RoutedEventHandler(this.btnMoveAll_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.btnRefresh = ((System.Windows.Controls.Button)(target));
            
            #line 75 "..\..\..\UserControlls\CommerceLinkFiles.xaml"
            this.btnRefresh.Click += new System.Windows.RoutedEventHandler(this.btnRefresh_Click);
            
            #line default
            #line hidden
            return;
            case 6:
            this.dgProcessed = ((System.Windows.Controls.DataGrid)(target));
            return;
            }
            this._contentLoaded = true;
        }
    }
}

