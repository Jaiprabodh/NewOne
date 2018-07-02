using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace StreamingEasy
{
    public partial class MainWindow : Window
    {
        public static MainWindow main;        
        public MainWindow()
        {
            InitializeComponent();
            main = this;            
        }        
        private void IntroPage(object sender, RoutedEventArgs e)
        {
            Controls_Back.IsEnabled = false;
            Controls_Next.IsEnabled = true;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Next.Click += DownloadPage;
            frame.Content = new IntroPage();
        }     

        private void DownloadPage(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = false;
            Controls_Back.IsEnabled = false;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += IntroPage;
            Controls_Next.Click += StreamingServer;
            frame.Content = new DownloadPage();
        }
        private void StreamingServer(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = false;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += DownloadPage;
            Controls_Next.Click += LatencyBandwidth;
            frame.Content = new StreamingServer();
        }
        private void LatencyBandwidth(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = false;
            Controls_Back.IsEnabled = false;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += StreamingServer;
            Controls_Next.Click += ContentConfig;
            frame.Content = new LatencyBandwidth();
        }
        private void ContentConfig(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = true;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += LatencyBandwidth;
            Controls_Next.Click += CameraSelect;
            frame.Content = new ContentConfig();
        }
        private void CameraSelect(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = true;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += ContentConfig;
            Controls_Next.Click += SceneSelect;
            frame.Content = new CameraSelect();
        }
        private void SceneSelect(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = true;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += CameraSelect;
            Controls_Next.Click += OutputResults;
            frame.Content = new SceneSelect();
        }
        private void OutputResults(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = true;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += SceneSelect;
            Controls_Next.Click += GenerateConfig;
            frame.Content = new OutputResults();
        }
        private void GenerateConfig(object sender, RoutedEventArgs e)
        {
            Controls_Next.IsEnabled = false;
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Back, Button.ClickEvent);
            HelperFunctions.RemoveRoutedEventHandlers(Controls_Next, Button.ClickEvent);
            Controls_Back.Click += OutputResults;
            Controls_Next.Click += CloseLaunchOBS;
            frame.Content = new GenerateConfig();
        }
        public void CloseLaunchOBS(object sender, RoutedEventArgs e)
        {
            Process obs = new Process();

            obs.StartInfo.UseShellExecute = true;
            obs.StartInfo.WorkingDirectory = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "obs-studio\\bin\\64bit\\");
            obs.StartInfo.FileName = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "obs-studio\\bin\\64bit\\obs64.exe");
            obs.Start();

            Application.Current.Shutdown();
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }
    }
}
