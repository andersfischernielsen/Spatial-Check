using System.Threading;
using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;
using System;
using System.Management;
using System.Collections.ObjectModel;
using System.IO;
using System.Diagnostics;
using System.Globalization;
using System.Media;
using SpatialCheck.ShellHelpers;
using MS.WindowsAPICodePack.Internal;
using Microsoft.WindowsAPICodePack.Shell.PropertySystem;
using SpatialCheck.Localization;
using SpatialCheck.Properties;
using System.Security.Principal;
using Microsoft.Win32;

namespace SpatialCheck
{
    public class SystemTrayViewModel : Caliburn.Micro.Screen
    {
        private string _activeIcon;
        private string _tooltipText;
        private const string APP_ID = "AndersFischerNielsen.SpatialCheck";
        private bool hasShown = false;
        private bool isEnabled = false;
        private const string ThemeRegKeyPath = @"Software\Microsoft\Windows\CurrentVersion\Themes\Personalize";
        private const string ThemeRegValueName = "SystemUsesLightTheme";

        public SystemTrayViewModel()
        {
            GetAvailableLanguages();
            TranslationManager.CurrentLanguageChangedEvent += (sender, args) => GetAvailableLanguages();

            ActiveIcon = $"Resources/icon{LightTheme()}.ico";
            TryCreateShortcut();
            Thread th = new Thread(RefreshSpatialState) { IsBackground = true };
            th.Start();
        }

        public string ActiveIcon
        {
            get { return _activeIcon; }
            set { Set(ref _activeIcon, value); }
        }

        public string TooltipText
        {
            get { return _tooltipText; }
            set { Set(ref _tooltipText, value); }
        }

        public ObservableCollection<CultureInfo> AvailableLanguages { get; } = new ObservableCollection<CultureInfo>();

        public void RefreshSpatialState()
        {
            while (true) { 
                var isEnabled = SpatialAudio.SpatialAudio.GetMaxDynamicObjectCount() > 0;
                if (isEnabled)
                {
                    TooltipText = Strings.SpatialSound_Enabled;
                    ActiveIcon = $"Resources/icon{LightTheme()}.ico";
                    ToastNotificationManager.History.Remove($"Enabled", "SpatialSoundToast", APP_ID);
                }
                else
                {
                    TooltipText = Strings.SpatialSound_Disabled;
                    ActiveIcon = $"Resources/disabled{LightTheme()}.ico";
                    if (!hasShown) ShowToast(isEnabled);
                    hasShown = !Settings.Default.AlwaysShowNotifications;
                }
                this.isEnabled = isEnabled;
                Thread.Sleep(10000);
            }
        }

        private bool TryCreateShortcut()
        {
            var shortcutPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Microsoft\\Windows\\Start Menu\\Programs\\Spatial Check.lnk";
            if (!File.Exists(shortcutPath))
            {
                InstallShortcut(shortcutPath);
                return true;
            }
            return false;
        }

        private void InstallShortcut(string shortcutPath)
        {
            var exePath = Process.GetCurrentProcess().MainModule.FileName;
            IShellLinkW newShortcut = (IShellLinkW) new CShellLink();

            ErrorHelper.VerifySucceeded(newShortcut.SetPath(exePath));
            ErrorHelper.VerifySucceeded(newShortcut.SetArguments(""));

            IPropertyStore newShortcutProperties = (IPropertyStore)newShortcut;

            using (PropVariant appId = new PropVariant(APP_ID))
            {
                ErrorHelper.VerifySucceeded(newShortcutProperties.SetValue(SystemProperties.System.AppUserModel.ID, appId));
                ErrorHelper.VerifySucceeded(newShortcutProperties.Commit());
            }

            IPersistFile newShortcutSave = (IPersistFile)newShortcut;
            ErrorHelper.VerifySucceeded(newShortcutSave.Save(shortcutPath, true));
        }

        private void ShowToast(bool isEnabled)
        {
            string argsDismiss = $"dismissed";
            string toastMarkup =
                $@"<toast scenario='reminder'>
                    <visual>
                        <binding template='ToastGeneric'>
                            <text>{(isEnabled ? Strings.SpatialSound_Enabled : Strings.SpatialSound_Disabled)}</text>
                            <text>{(isEnabled ? Strings.EnabledTooltipText : Strings.DisabledTooltipText)}</text>
                        </binding>
                    </visual>
                </toast>";

            XmlDocument toastXml = new XmlDocument();
            toastXml.LoadXml(toastMarkup);
            var toast = new ToastNotification(toastXml);
            toast.Activated += ToastActivated;
            toast.Dismissed += ToastDismissed;
            toast.Tag = $"SpatialCheck";
            toast.Group = "SpatialCheck";
            ToastNotificationManager.CreateToastNotifier(APP_ID).Show(toast);
        }

        private void ToastActivated(ToastNotification sender, object e)
        {
            var toastArgs = e as ToastActivatedEventArgs;
            // TODO: Implement opening Sound Settings Control Panel. 
            if (this.isEnabled)
            {
                Console.WriteLine();
            }
        }
        private void ToastDismissed(ToastNotification sender, object e)
        {
        }

        public void ExitApplication()
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void GetAvailableLanguages()
        {
            AvailableLanguages.Clear();
            foreach (var language in TranslationManager.AvailableLanguages)
            {
                AvailableLanguages.Add(language);
            }
        }

        public void WatchTheme()
        {
            var currentUser = WindowsIdentity.GetCurrent();
            string query = string.Format(
                CultureInfo.InvariantCulture,
                @"SELECT * FROM RegistryValueChangeEvent WHERE Hive = 'HKEY_USERS' AND KeyPath = '{0}\\{1}' AND ValueName = '{2}'",
                currentUser.User.Value,
                ThemeRegKeyPath.Replace(@"\", @"\\"),
                ThemeRegValueName);

            try
            {
                var watcher = new ManagementEventWatcher(query);
                watcher.EventArrived += (sender, args) =>
                {
                    LightTheme();
                    
                };
                watcher.Start();
            }
            catch (Exception) { }

            LightTheme();
        }

        private string LightTheme()
        {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey(ThemeRegKeyPath))
            {
                object registryValueObject = key?.GetValue(ThemeRegValueName);
                if (registryValueObject == null)
                {
                    return "";
                }

                int registryValue = (int)registryValueObject;

                return registryValue > 0 ? "-black" : "";
            }
        }
    }
}