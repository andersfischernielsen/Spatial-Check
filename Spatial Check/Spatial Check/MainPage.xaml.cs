using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Media.Audio;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Spatial_Check
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private bool spatialIsEnabled = true;
        public string SpatialIsEnabledText { get => spatialIsEnabled ? "Enabled" : "Disabled"; }

        public MainPage()
        {
            this.InitializeComponent();
            this.DataContext = this;
            GetSpatialIsEnabled();
        }

        private async void GetSpatialIsEnabled()
        {
            var collection = await DeviceInformation.FindAllAsync(DeviceClass.AudioRender);
            var filtered = collection.AsParallel().Where(d => SpatialAudioDeviceConfiguration.GetForDeviceId(d.Id).ActiveSpatialAudioFormat != "{00000000-0000-0000-0000-000000000000}");
            this.spatialIsEnabled = filtered.Any();
        }

        private void Check(object sender, RoutedEventArgs e)
        {
            GetSpatialIsEnabled();
        }
    }
}

