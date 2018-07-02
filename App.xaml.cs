using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Reflection;
using System.Management;
using System.Diagnostics;
using Microsoft.Win32;

namespace StreamingEasy
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static bool IsWindows10orAbove;
        public static string Processorname;
        public const string OSErrorMessage = "Pre-requisites not met...!Kindly install Windows 10 as this application only supports windows 10";
        public const string ProcessorErrorMessage = "Pre-requisites not met...!The processor won't support the installation";

        private void ApplicationStartup(object sender, StartupEventArgs e)
        {
            var cpu = new ManagementObjectSearcher("select * from Win32_Processor").Get().Cast<ManagementObject>().First();
            if ((string)cpu["Manufacturer"] != "GenuineIntel")
            {
                Application.Current.Shutdown();
            }
            Debug.WriteLine((string)cpu["Name"]); //Implement CPU model checking later.



            IsWindows10orAbove = HelperFunctions.IsWindows10();

            if (!IsWindows10orAbove)
            {
                //MessageBoxResult result = MessageBox.Show("The operating system is not windows 10 or above. So we cant install the software");
                //if (result == MessageBoxResult.OK)
                //{
                //Application.Current.Shutdown();
                //}

                NotSupportedPage NSP = new NotSupportedPage(OSErrorMessage);
                NSP.Show();
            }
            else
            {
                Processorname = HelperFunctions.SendBackProcessorName();
                string[] ProcessorNameSpits = Processorname.Split(' ');
                Processorname = ProcessorNameSpits[2];
                if (HelperFunctions.ProcessorNameExist(Processorname))
                {
                    Config.scenes = Scenes.GetScenes(); //Initialize Scene and IDN objects
                    MainWindow wnd = new MainWindow();
                    wnd.Show();
                }
               else
                {
                    NotSupportedPage NSP = new NotSupportedPage(ProcessorErrorMessage);
                    NSP.Show();
                }
            }

            
        } 


    }
}
