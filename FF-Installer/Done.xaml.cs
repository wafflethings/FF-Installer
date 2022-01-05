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

namespace FF_Installer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class Done : Window
    {
        public Done()
        {
            InitializeComponent();
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

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Download.CreateShortcut("Fall Factory", Environment.GetFolderPath(Environment.SpecialFolder.Desktop), Prefs.path + "\\depots\\1097151\\7843631\\FallGuys_client_game.exe");
        }
    }
}
