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
using System.Runtime.InteropServices;
using System.Windows.Interop;
using System.Windows.Media.Animation;
using project.ViewModels;

namespace project.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {
        public MainView()
        {
            InitializeComponent();
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
            this.StateChanged += Window_StateChanged;
            //WD_State();
            //this.WindowState = WindowState.Maximized;
            this.Left = SystemParameters.WorkArea.Left;
            this.Top = SystemParameters.WorkArea.Top;
            this.Width = SystemParameters.WorkArea.Width;
            this.Height = SystemParameters.WorkArea.Height;
            //this.DataContextChanged += ViewAutoView_DataContextChanged;
        }
        [DllImport("user32.dll")]
        public static extern IntPtr SendMessage(IntPtr hWnd, int wMsg, int wParam, int lParam);
        private void pnControlBar_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowInteropHelper helper = new WindowInteropHelper(this);
            SendMessage(helper.Handle, 161, 2, 0);
        }

        private void pnControlBar_MouseEnter(object sender, MouseEventArgs e)
        {
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
        }
        public void CloseWithFade()
        {
            Storyboard fadeOut = (Storyboard)this.Resources["FadeOutStoryboard"];
            fadeOut.Completed += (s, e) =>
            {
                this.Close();
            };
            fadeOut.Begin(this);
        }
        public void ShutdownWithFade()
        {
            Storyboard fadeOut = (Storyboard)this.Resources["FadeOutStoryboard"];
            fadeOut.Completed += (s, e) =>
            {
                Application.Current.Shutdown();
            };
            fadeOut.Begin(this);
        }
        public void MinimizeWithFade()
        {
            Storyboard fadeOut = (Storyboard)this.Resources["FadeOutStoryboard"];
            fadeOut.Completed += (s, e) =>
            {
                this.WindowState = WindowState.Minimized;
                this.Opacity = 1;
            };
            fadeOut.Begin(this);
        }
        private void bnMinimize_Click(object sender, RoutedEventArgs e)
        {
            MinimizeWithFade();
        }
        //private void bnMaximine_Click(object sender, RoutedEventArgs e)
        //{
        //    WD_State();
        //}
        public void WD_State()
        {
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }
        private void Window_StateChanged(object sender, EventArgs e)
        {
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
        }
    }
}
