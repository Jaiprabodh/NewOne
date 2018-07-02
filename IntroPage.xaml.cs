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
using System.Windows.Shapes;

namespace StreamingEasy
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class IntroPage : Page
    {
        public IntroPage()
        {
            InitializeComponent();
            MainWindow.main.Dispatcher.BeginInvoke(new Action(delegate
            {
                MainWindow.main.PageTitle.Text = "OBS Configuration Utility";
                MainWindow.main.Description.Text = "The following pages will guide you through the installation and configuration of OBS.";
            }));
        }
    }
}
