﻿#pragma checksum "..\..\SettingWindow.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4F179444ACE2C0005D6AC0EACF48D93A"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.34209
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
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
using Xceed.Wpf.Toolkit;
using Xceed.Wpf.Toolkit.Chromes;
using Xceed.Wpf.Toolkit.Core.Converters;
using Xceed.Wpf.Toolkit.Core.Input;
using Xceed.Wpf.Toolkit.Core.Media;
using Xceed.Wpf.Toolkit.Core.Utilities;
using Xceed.Wpf.Toolkit.Panels;
using Xceed.Wpf.Toolkit.Primitives;
using Xceed.Wpf.Toolkit.PropertyGrid;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;
using Xceed.Wpf.Toolkit.PropertyGrid.Commands;
using Xceed.Wpf.Toolkit.PropertyGrid.Converters;
using Xceed.Wpf.Toolkit.PropertyGrid.Editors;
using Xceed.Wpf.Toolkit.Zoombox;


namespace BoardClient {
    
    
    /// <summary>
    /// SettingWindow
    /// </summary>
    public partial class SettingWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 31 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel spAdress;
        
        #line default
        #line hidden
        
        
        #line 33 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbIp;
        
        #line default
        #line hidden
        
        
        #line 37 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox tbPort;
        
        #line default
        #line hidden
        
        
        #line 43 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slOpacity;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.CheckBox cbTopmost;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Slider slUpdateRate;
        
        #line default
        #line hidden
        
        
        #line 62 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker cpText;
        
        #line default
        #line hidden
        
        
        #line 67 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal Xceed.Wpf.Toolkit.ColorPicker cpBackground;
        
        #line default
        #line hidden
        
        
        #line 71 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btCancel;
        
        #line default
        #line hidden
        
        
        #line 72 "..\..\SettingWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button btOk;
        
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
            System.Uri resourceLocater = new System.Uri("/BoardClient;component/settingwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\SettingWindow.xaml"
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
            this.spAdress = ((System.Windows.Controls.StackPanel)(target));
            return;
            case 2:
            this.tbIp = ((System.Windows.Controls.TextBox)(target));
            
            #line 35 "..\..\SettingWindow.xaml"
            this.tbIp.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbIp_TextChanged);
            
            #line default
            #line hidden
            return;
            case 3:
            this.tbPort = ((System.Windows.Controls.TextBox)(target));
            
            #line 39 "..\..\SettingWindow.xaml"
            this.tbPort.TextChanged += new System.Windows.Controls.TextChangedEventHandler(this.tbPort_TextChanged);
            
            #line default
            #line hidden
            return;
            case 4:
            this.slOpacity = ((System.Windows.Controls.Slider)(target));
            
            #line 48 "..\..\SettingWindow.xaml"
            this.slOpacity.ValueChanged += new System.Windows.RoutedPropertyChangedEventHandler<double>(this.slOpacity_ValueChanged);
            
            #line default
            #line hidden
            return;
            case 5:
            this.cbTopmost = ((System.Windows.Controls.CheckBox)(target));
            return;
            case 6:
            this.slUpdateRate = ((System.Windows.Controls.Slider)(target));
            return;
            case 7:
            this.cpText = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            return;
            case 8:
            this.cpBackground = ((Xceed.Wpf.Toolkit.ColorPicker)(target));
            return;
            case 9:
            this.btCancel = ((System.Windows.Controls.Button)(target));
            return;
            case 10:
            this.btOk = ((System.Windows.Controls.Button)(target));
            
            #line 72 "..\..\SettingWindow.xaml"
            this.btOk.Click += new System.Windows.RoutedEventHandler(this.btOk_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}

