﻿#pragma checksum "..\..\..\UserControlls\FileJobsDetail.xaml" "{8829d00f-11b8-4213-878b-770e8597ac16}" "EA53917494307CC0F7AE180DE42E0015670A9903AED0E12E57C0753766FAE452"
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
using VSI.EDGEAXConnector.UI.Convertors;


namespace VSI.EDGEAXConnector.UI.UserControlls {
    
    
    /// <summary>
    /// FileJobsDetail
    /// </summary>
    public partial class FileJobsDetail : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 28 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGrid dgJobs;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colJobId;
        
        #line default
        #line hidden
        
        
        #line 64 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colJobName;
        
        #line default
        #line hidden
        
        
        #line 73 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colJobInterval;
        
        #line default
        #line hidden
        
        
        #line 87 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colIsRepeatable;
        
        #line default
        #line hidden
        
        
        #line 101 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colIsActive;
        
        #line default
        #line hidden
        
        
        #line 115 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colStartTime;
        
        #line default
        #line hidden
        
        
        #line 129 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colJobStatus;
        
        #line default
        #line hidden
        
        
        #line 147 "..\..\..\UserControlls\FileJobsDetail.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DataGridTemplateColumn colOptions;
        
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
            System.Uri resourceLocater = new System.Uri("/VSI.EDGEAXConnector.UI;component/usercontrolls/filejobsdetail.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\UserControlls\FileJobsDetail.xaml"
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
            
            #line 8 "..\..\..\UserControlls\FileJobsDetail.xaml"
            ((VSI.EDGEAXConnector.UI.UserControlls.FileJobsDetail)(target)).Loaded += new System.Windows.RoutedEventHandler(this.UserControl_Loaded);
            
            #line default
            #line hidden
            return;
            case 2:
            this.dgJobs = ((System.Windows.Controls.DataGrid)(target));
            return;
            case 3:
            this.colJobId = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 4:
            this.colJobName = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 5:
            this.colJobInterval = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 6:
            this.colIsRepeatable = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 7:
            this.colIsActive = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 8:
            this.colStartTime = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 9:
            this.colJobStatus = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            case 10:
            this.colOptions = ((System.Windows.Controls.DataGridTemplateColumn)(target));
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 11:
            
            #line 151 "..\..\..\UserControlls\FileJobsDetail.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.btnSave_Click);
            
            #line default
            #line hidden
            break;
            }
        }
    }
}
