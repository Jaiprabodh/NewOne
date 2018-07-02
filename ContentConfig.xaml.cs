using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Select the type of content that the user will be streaming. It adjusts FPS, Resolution, and scene configuration based on selection.
    /// </summary>
    public partial class ContentConfig : Page
    {
        public ContentConfig()
        {
            InitializeComponent();
            MainWindow.main.Dispatcher.BeginInvoke(new Action(delegate
            {
                MainWindow.main.PageTitle.Text = "Content Type";
                MainWindow.main.Description.Text = "Select the type of content that will be streamed. If you are unsure, click next.";
            }));
            switch (Config.Content_Type)
            {
                case "Personality":
                    Personality.IsChecked = true;
                    break;
                case "RTS":
                    RTS.IsChecked = true;
                    break;
                case "FPS":
                    FPS.IsChecked = true;
                    break;
                default:
                    Default.IsChecked = true;
                    break;
            }

        }
        private void ContentTypeHandler(object sender, EventArgs e)
        {
            var radiobutton = sender as RadioButton;
            if (radiobutton == null)
                return;
            Config.Content_Type = radiobutton.Name.ToString();
            Debug.WriteLine("Content Type: " + radiobutton.Name.ToString());
        }
    }
}
