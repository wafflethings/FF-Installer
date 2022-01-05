using IWshRuntimeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
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

namespace FF_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Download : Window
    {
        public Download()
        {
            InitializeComponent();

            Thread thread = new Thread(WhatInZamnation);
            thread.Start();
        }

        private void WhatInZamnation()
        {
            string appPath = AppDomain.CurrentDomain.BaseDirectory;
            string ddlPath = appPath + "\\Libraries\\depotdl";
            string[] files = Directory.GetFiles(ddlPath);

            string source = "Fall Factory - SHOW THIS TO WAFFLE";
            string log = "Application";
            if (!EventLog.SourceExists(source))
            {
                EventLog.CreateEventSource(source, log);
            }


            foreach (string f in files)
            {
                string fName = f.Substring(ddlPath.Length + 1);
                System.IO.File.Copy(System.IO.Path.Combine(ddlPath, fName), System.IO.Path.Combine(Prefs.path, fName), true);
            }

            Process pogu = new Process();

            try
            {
                var commands = "/C ";
                commands += "dotnet DepotDownloader.dll -app 1097150 -depot 1097151 -manifest 7823190448145834983 -username " + Prefs.username + " -password " + Prefs.password;
                Console.WriteLine(commands);
                pogu.StartInfo.FileName = "cmd.exe";
                pogu.StartInfo.Arguments = commands;
                pogu.StartInfo.WorkingDirectory = Prefs.path;
                //pogu.StartInfo.RedirectStandardOutput = true;
                pogu.Start();
            } catch(Exception ex)
            {
                //EventLog.WriteEntry(source, ex.ToString(), EventLogEntryType.Error, 100);
            }

            CreateShortcut("Fall Factory", Prefs.path, Prefs.path + "\\depots\\1097151\\7843631\\FallGuys_client_game.exe");
            string commonStartMenuPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu);
            string appStartMenuPath = System.IO.Path.Combine(commonStartMenuPath, "Programs", "Fall Factory");
            System.IO.Directory.CreateDirectory(appStartMenuPath);
            CreateShortcut("Fall Factory", appStartMenuPath, Prefs.path + "\\depots\\1097151\\7843631\\FallGuys_client_game.exe");

            pogu.WaitForExit();
            //EventLog.WriteEntry(source, pogu.StandardOutput.ReadToEnd(), EventLogEntryType.Information, 100);
            //System.IO.File.Copy(System.IO.Path.Combine(appPath, "ffico.ico"), Prefs.path + "\\depots\\1097151\\7843631\\ico.ico", true);
            DeleteDirectory(Prefs.path + "\\depots\\1097151\\7843631\\EasyAntiCheat");
            System.IO.File.Delete(Prefs.path + "\\depots\\1097151\\7843631\\FallGuys_client.exe");
            System.IO.File.Copy(System.IO.Path.Combine(appPath, "Libraries\\goldberg\\steam_api64.dll"), Prefs.path + "\\depots\\1097151\\7843631\\FallGuys_client_game_Data\\Plugins\\x86_64\\steam_api64.dll", true);
            DirectoryCopy(appPath + "\\Libraries\\fallfac+melon", Prefs.path + "\\depots\\1097151\\7843631\\",true);
            Thread.Sleep(10000); //this code is so bad LMAO but it should work and im too lazy

            Application.Current.Dispatcher.Invoke(() =>
            {
                this.Hide();
                Window sheesh = new Done();
                sheesh.Show();
                sheesh.Left = this.Left;
                sheesh.Top = this.Top;
            });
        }

        public static void CreateShortcut(string shortcutName, string shortcutPath, string targetFileLocation)
        {
            string shortcutLocation = System.IO.Path.Combine(shortcutPath, shortcutName + ".lnk");
            WshShell shell = new WshShell();
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutLocation);

            shortcut.Description = "Fall Factory. Pretty cool Fall Guys level editor if you ask me. (may be slightly biased)";   // The description of the shortcut
            shortcut.IconLocation = AppDomain.CurrentDomain.BaseDirectory + "\\ffico.ico";           // The icon of the shortcut
            shortcut.TargetPath = targetFileLocation;                 // The path of the file that will launch when the shortcut is run
            shortcut.Save();                                    // Save the shortcut
        }

        public static void DeleteDirectory(string target_dir)
        {
            string[] files = Directory.GetFiles(target_dir);
            string[] dirs = Directory.GetDirectories(target_dir);

            foreach (string file in files)
            {
                System.IO.File.SetAttributes(file, FileAttributes.Normal);
                System.IO.File.Delete(file);
            }

            foreach (string dir in dirs)
            {
                DeleteDirectory(dir);
            }

            Directory.Delete(target_dir, false);
        }

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Show();
            this.Hide();
            Window sheesh = new Path();
            sheesh.Show();
            sheesh.Left = this.Left;
            sheesh.Top = this.Top;
        }

        private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories();

            // If the destination directory doesn't exist, create it.       
            Directory.CreateDirectory(destDirName);

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles();
            foreach (FileInfo file in files)
            {
                string tempPath = System.IO.Path.Combine(destDirName, file.Name);
                file.CopyTo(tempPath, true);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string tempPath = System.IO.Path.Combine(destDirName, subdir.Name);
                    DirectoryCopy(subdir.FullName, tempPath, copySubDirs);
                }
            }
        }
    }
}
