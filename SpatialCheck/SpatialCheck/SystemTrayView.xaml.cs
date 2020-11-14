using System.Windows;
using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Data;
using System.Xml;
using SpatialCheck.Localization;

namespace SpatialCheck
{
    /// <summary>
    ///     Interaction logic for SystemTrayView.xaml
    /// </summary>
    public partial class SystemTrayView : Window
    {
        public SystemTrayView()
        {
            InitializeComponent();
            ShowInTaskbar = false;

            var language = new CultureInfo(Properties.Settings.Default.Language);
            TranslationManager.CurrentLanguage = language;
        }

        readonly RegistryKey autoStartKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
        private readonly string appID = "SpatialCheck";

        private void StartWithWindows()
        {
            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            autoStartKey.SetValue(appID, exePath);
        }

        private void RemoveAutoStart()
        {
            autoStartKey.DeleteValue(appID, false);
        }

        private void AutoStart_Click(object sender, RoutedEventArgs e)
        {
            bool autorun_check = !Properties.Settings.Default.AutoStart;
            if (!autorun_check)
            {
                Properties.Settings.Default.AutoStart = true;
                Properties.Settings.Default.Save();
                StartWithWindows();
            }
            else
            {
                Properties.Settings.Default.AutoStart = false;
                Properties.Settings.Default.Save();
                RemoveAutoStart();
            }
        }

        private void AlwaysShowNotifications_Click(object sender, RoutedEventArgs e)
        {
            bool alwaysShow = Properties.Settings.Default.AlwaysShowNotifications;
            if (alwaysShow)
            {
                Properties.Settings.Default.AlwaysShowNotifications = false;
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.AlwaysShowNotifications = true;
                Properties.Settings.Default.Save();
            }
            Properties.Settings.Default.Upgrade();
        }

        private void LanguageItem_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedLanguage = (CultureInfo)((FrameworkElement)e.OriginalSource).DataContext;
            TranslationManager.CurrentLanguage = selectedLanguage;

            Properties.Settings.Default.Language = selectedLanguage.Name;
            Properties.Settings.Default.Save();
        }
    }

    public class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }

        public SettingBindingExtension(string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            Source = Properties.Settings.Default;
            Mode = BindingMode.TwoWay;
        }
    }
}