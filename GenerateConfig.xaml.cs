using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Generates the config files for OBS.
    /// </summary>
    public partial class GenerateConfig : Page
    {
        public GenerateConfig()
        {
            InitializeComponent();
            MainWindow.main.Dispatcher.BeginInvoke(new Action(delegate
            {
                MainWindow.main.PageTitle.Text = "Configuration Complete";
                MainWindow.main.Description.Text = "Click next to coninue and launch Open Source Broadcaster.";
            }));

            //General Config
            GenerateBasic();                //Generate Basic.ini based on OBS_Temp.BasicIni Template
            GenerateEncoder();              //Generate StreamEncoder.json based on OBS_Temp.StreamEncoder Template
            GenerateService();              //Generate service.json based on OBS_Temp.Service Template

            //Scene Config
            GenerateSceneConfig();          //Generate the scene config file based on OBS_Temp.SceneTemplateTop, the selected scenes and scene items within, then source descriptions.

            //Set Global Config
            GenerateGlobalIni();            //Generates Global.ini, if Global.ini already exists it changed only the selected scenes/config.

            MainWindow.main.Controls_Next.IsEnabled = true;
            MainWindow.main.Controls_Back.IsEnabled = true;

        }
        private void GenerateBasic()
        {
            string SourceString = String.Format(OBS_Temp.BasicIni, Config.Name, Config.OutputCX, Config.OutputCY, Config.FPSCommon, Config.Encoder);

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio\\basic\\profiles\\" + Config.Name + "\\basic.ini");

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            System.IO.File.WriteAllText(file.FullName, SourceString);
        }
        private void GenerateEncoder()
        {
            string SourceString = String.Format(OBS_Temp.StreamEncoder, Config.bitrate, Config.preset, Config.profile, Config.rate_control);

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio\\basic\\profiles\\" + Config.Name + "\\streamEncoder.json");

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            System.IO.File.WriteAllText(file.FullName, SourceString);
        }
        private void GenerateService()
        {
            string SourceString = String.Format(OBS_Temp.Service, Config.Streaming_Key, Config.Streaming_Service);

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio\\basic\\profiles\\" + Config.Name + "\\service.json");

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            System.IO.File.WriteAllText(file.FullName, SourceString);
        }

        private void GenerateSceneConfig()
        {
            int i = 0;
            string SourceString = String.Format(OBS_Temp.SceneTemplateTop, Config.Name);

            foreach (var scene in Config.scenes)
            {
                if (scene.Enabled)
                {
                    if (i != 0) SourceString += ",";
                    SourceString += "\n\t\t{";
                    SourceString += "\n\t\t\t\"name\": \"" + scene.Name + "\"";
                    SourceString += "\n\t\t}";
                    i++;
                }
            }
            SourceString += "\n\t],";
            SourceString += "\n\t\"sources\": [";

            //This generates each of the scenes and then generates the items in each scene. Everything links back to the Config.scenes object.
            foreach (var scene in Config.scenes)
            {
                if (scene.Enabled)
                {
                    i = 0;
                    SourceString += String.Format(OBS_Temp.SceneDescription, scene.Name, scene.Items.Length);
                    foreach (var item in scene.Items)
                    {
                        if (i != 0) SourceString += ",";
                        SourceString += String.Format(OBS_Temp.ItemDescription, item.Xbound, item.Ybound, item.BoundType, i, item.Name, item.Xpos, item.Ypos, item.Xscale, item.Yscale);
                        i++;
                    }
                    SourceString += "\n\t\t\t\t]";
                    SourceString += "\n\t\t\t}";
                    SourceString += "\n\t\t},";
                }
            }

            //This generates each of the source descriptions. It will create the source descriptions depending on which scenes are enabled.
            string ImagePath;
            i = 0;
            List<string> completeSources = new List<string>(); //Keep track of scene items that have been already generated.
            foreach (var scene in Config.scenes)
            {
                if (scene.Enabled)
                {
                    foreach (var item in scene.Items)
                    {
                        if (completeSources.Contains(item.Name)) continue;
                        if (item.GetType() == typeof(ImageItem))
                        {
                            if (i != 0) SourceString += ",";
                            var imageitem = (ImageItem)item;
                            ImagePath = Config.usr_filepath.Replace("\\", "\\\\") + imageitem.ImageName;
                            SourceString += String.Format(OBS_Temp.SourceDescription, "image_source", imageitem.Name, "\"file\": \"" + ImagePath + "\"");
                            i++;
                        }
                        if (item.GetType() == typeof(TextItem))
                        {
                            if (i != 0) SourceString += ",";
                            var textitem = (TextItem)item;
                            SourceString += String.Format(OBS_Temp.TextSourceDescription, "text_gdiplus", textitem.Name, textitem.SceneText);
                            i++;
                        }
                        if (item.GetType() == typeof(CameraItem))
                        {
                            if (i != 0) SourceString += ",";
                            var cameraitem = (CameraItem)item;
                            SourceString += String.Format(OBS_Temp.SourceDescription, "dshow_input", cameraitem.Name, "\"last_video_device_id\": \"" + Config.Camera_DevicePath + "\",\n\t\t\t\t\"video_device_id\": \"" + Config.Camera_DevicePath + "\"");
                            i++;
                        }
                        if (item.GetType() == typeof(MonitorCaptureItem))
                        {
                            if (i != 0) SourceString += ",";
                            SourceString += String.Format(OBS_Temp.SourceDescription, "monitor_capture", "DesktopCapture", "");
                            i++;
                        }
                        completeSources.Add(item.Name);
                    }
                }
            }
            SourceString += OBS_Temp.SceneTemplateBottom;

            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio\\basic\\scenes\\" + Config.Name + ".json");

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create(); // If the directory already exists, this method does nothing.
            System.IO.File.WriteAllText(file.FullName, SourceString);
        }
        private void GenerateGlobalIni()
        {
            var filePath = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "obs-studio\\global.ini");
            string source;
            try
            {
                source = File.ReadAllText(filePath);
                if (source.Contains("SceneCollection") && source.Contains("Profile"))
                {
                    source = Regex.Replace(source, @"(?<=SceneCollection=)(\w+)", Config.Name);
                    source = Regex.Replace(source, @"(?<=SceneCollectionFile=)(\w+)", Config.Name);
                    source = Regex.Replace(source, @"(?<=Profile=)(\w+)", Config.Name);
                    source = Regex.Replace(source, @"(?<=ProfileDir=)(\w+)", Config.Name);
                }
                else
                {
                    source = String.Format(OBS_Temp.GlobalIni, Config.Name);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Failed to load global.ini, generating from scratch." + "\n" + e.Message);
                source = String.Format(OBS_Temp.GlobalIni, Config.Name);
            }

            System.IO.FileInfo file = new System.IO.FileInfo(filePath);
            file.Directory.Create();
            System.IO.File.WriteAllText(file.FullName, source);
        }
    }
}