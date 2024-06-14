using Microsoft.Win32;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfAnimatedGif;

// If you enjoy this project, you can support it by making a donation!
// Donation link: https://buymeacoffee.com/_ronniexcoder
// You can also visit my YouTube channel for more content: https://www.youtube.com/@ronniexcoder

namespace ScreenAnimation
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<Window> dynamicWindows = new List<Window>();
        bool firstIteration = true;
        public MainWindow()
        {
            InitializeComponent();
            this.MouseLeftButtonDown += MainWindow_MouseLeftButtonDown;
            this.KeyDown += MainWindow_KeyDown;
        }

        private void MainWindow_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                ((Window)sender).Close();
            }
        }

        private void CloseMenuItem_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
        {
            foreach (var window in dynamicWindows)
            {
                window.Close();
            }

            dynamicWindows.Clear();
            this.Close();
        }

        private void AddMenuItem_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Multiselect = true;
            openFileDialog.Filter = "GIF files (*.gif)|*.gif";
            Random rand = new Random();

            bool? result = openFileDialog.ShowDialog();

            if (result == true)
            {

                string[] files = openFileDialog.FileNames;


                foreach (string file in files)
                {

                    if (firstIteration)
                    {
                        firstIteration = false;
                        Image img = new Image();
                        ImageBehavior.SetAnimatedSource(img, new BitmapImage(new Uri(file)));
                        Grid.SetColumn(img, 0);
                        Grid.SetRow(img, 0);
                        MainGrid.Children.Add(img);
                        continue;
                    }


                    Window dynamicWindow = new Window();
                    dynamicWindow.Title = "Dynamic Window";
                    dynamicWindow.Height = 450;
                    dynamicWindow.Width = 400;
                    dynamicWindow.AllowsTransparency = true;
                    dynamicWindow.Background = System.Windows.Media.Brushes.Transparent;
                    dynamicWindow.Left = rand.Next((int)(SystemParameters.PrimaryScreenWidth - dynamicWindow.Width));
                    dynamicWindow.Top = rand.Next((int)(SystemParameters.PrimaryScreenHeight - dynamicWindow.Height));
                    dynamicWindow.WindowStyle = WindowStyle.None;
                    dynamicWindow.Topmost = true;
                    dynamicWindow.ShowInTaskbar = false;
                    dynamicWindow.KeyDown += MainWindow_KeyDown;

                    Grid grid = new Grid();
                    grid.MouseLeftButtonDown += (sender, e) => dynamicWindow.DragMove();

                    ContextMenu contextMenu = new ContextMenu();
                    MenuItem closeMenuItem = new MenuItem();
                    closeMenuItem.Header = "Close";
                    closeMenuItem.Click += (sender, e) => dynamicWindow.Close();
                    contextMenu.Items.Add(closeMenuItem);
                    grid.ContextMenu = contextMenu;

                    Image image = new Image();
                    ImageBehavior.SetAnimatedSource(image,
                        new BitmapImage(new Uri(file)));

                    Grid.SetColumn(image, 0);
                    Grid.SetRow(image, 0);
                    grid.Children.Add(image);

                    dynamicWindow.Content = grid;
                    dynamicWindows.Add(dynamicWindow);
                    dynamicWindow.Show();
                }
            }
        }

    }
}
