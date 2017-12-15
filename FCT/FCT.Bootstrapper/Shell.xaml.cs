using System;
using System.ComponentModel;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace FCT.Bootstrapper
{
    /// <summary>
    /// Interaction logic for RootView.xaml
    /// </summary>
    public partial class Shell : System.Windows.Window
    {
        private static FieldInfo _menuDropAlignmentField; //Fix for setting the application pop-ups on the right (on windows 8 and newer)

        public Shell()
        {
            InitializeComponent();
            Init();
        }

        static void Init()
        {
            _menuDropAlignmentField = typeof(SystemParameters).GetField("_menuDropAlignment", BindingFlags.NonPublic | BindingFlags.Static);
            System.Diagnostics.Debug.Assert(_menuDropAlignmentField != null);

            EnsureStandardPopupAlignment();
            SystemParameters.StaticPropertyChanged += SystemParameters_StaticPropertyChanged;
        }

        private static void SystemParameters_StaticPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            EnsureStandardPopupAlignment();
        }

        private static void EnsureStandardPopupAlignment()
        {
            if (SystemParameters.MenuDropAlignment && _menuDropAlignmentField != null)
            {
                _menuDropAlignmentField.SetValue(null, false);
            }
        }
    }
}
