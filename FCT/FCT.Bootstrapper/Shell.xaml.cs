using System;
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
        public Shell()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            // var newResourceDictionary = LoadThemeDictionary("Royale", "NormalColor");
            // //var newResourceDictionary = LoadThemeDictionary("Aero", "NormalColor");

            // //this.Resources.MergedDictionaries.Add(newResourceDictionary);
            // Application.Current.Resources.MergedDictionaries.Clear();
            //var dictionariesWidnow = this.Resources.MergedDictionaries;
            //var dictionaries = Application.Current.Resources.MergedDictionaries;
        }

        public static ResourceDictionary LoadThemeDictionary(string themeName, string colorScheme)
        {
            Assembly controlAssembly = typeof(Button).Assembly;
            AssemblyName themeAssemblyName = controlAssembly.GetName();

            object[] attrs = controlAssembly.GetCustomAttributes(typeof(ThemeInfoAttribute), false);
            if (attrs.Length > 0)
            {
                ThemeInfoAttribute ti = (ThemeInfoAttribute)attrs[0];

                if (ti.ThemeDictionaryLocation ==
                                     ResourceDictionaryLocation.None)
                {
                    // There are no theme-specific resources.
                    return null;
                }

                if (ti.ThemeDictionaryLocation ==
                        ResourceDictionaryLocation.ExternalAssembly)
                {
                    themeAssemblyName.Name += "." + themeName;
                }
            }

            string relativePackUriForResources = "/" +
                themeAssemblyName.FullName +
                ";component/themes/" +
                themeName + "." +
                colorScheme + ".xaml";

            Uri resourceLocater = new System.Uri(
                relativePackUriForResources, System.UriKind.Relative);
            return Application.LoadComponent(resourceLocater)
                       as ResourceDictionary;
        }
    }
}
