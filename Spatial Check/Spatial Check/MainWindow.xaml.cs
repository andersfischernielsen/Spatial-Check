using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Runtime.InteropServices;
using System.Windows.Shapes;
using AudioHelpers;
using NAudio.CoreAudioApi;

namespace Spatial_Check
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Test();
        }

        private void Test()
        {
            var deviceEnumerator = new MMDeviceEnumerator();
            foreach (var d in deviceEnumerator.EnumerateAudioEndPoints(DataFlow.Render, DeviceState.Active)) { 
                var spatialAudioClient = d.SpatialAudioClient;
                var result = spatialAudioClient.MaxDynamicObjectCount;
                Console.WriteLine(result);
            }
        }
    }
}
