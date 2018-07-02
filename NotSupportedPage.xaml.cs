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
using System.Windows.Shapes;

namespace StreamingEasy
{
    /// <summary>
    /// Interaction logic for NotSupportedPage.xaml
    /// </summary>
    public partial class NotSupportedPage : Window
    {
        public NotSupportedPage()
        {
            InitializeComponent();
            //frame.Content = "Pre-requisites not met...!Kindly install Windows 10 as this application only supports windows 10";
        }
        public NotSupportedPage(string str)
        {
            InitializeComponent();
            frame.Content = str;
        }
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            //MessageBoxResult result = MessageBox.Show("Do you want to close this window?", "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            //if (result == MessageBoxResult.Yes)
            //{
                Application.Current.Shutdown();
           // }
        }
    }
}
