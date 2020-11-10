using SpatialCheckTray.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SpatialCheckTray
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new SpatialCheckTrayApplicationContext());
            Application.Exit();
        }
    }


    public class SpatialCheckTrayApplicationContext : ApplicationContext
    {
        private NotifyIcon trayIcon;

        public SpatialCheckTrayApplicationContext()
        {
            // Initialize Tray Icon
            trayIcon = new NotifyIcon()
            {
                Icon = Resources.AppIcon,
                Text = "Spatial Check",
                ContextMenu = new ContextMenu(new MenuItem[] {
                new MenuItem("Exit", Exit),
                new MenuItem("Check", Check)
            }),
                Visible = true
            };

            Check("");
        }


        void Check(object sender, EventArgs e)
        {
            Check("userCheck");
        }

        void Check(string args)
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Directory.GetCurrentDirectory() + @"\ConsoleApplication.exe",
                Arguments = args,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process template = new Process
            {
                StartInfo = startInfo
            };
            try
            {
                template.Start();
                return;
            }
            catch 
            {
                throw;
            }
        }

        void Exit(object sender, EventArgs e)
        {
            trayIcon.Visible = false;
            Application.Exit();
        }
    }
}
