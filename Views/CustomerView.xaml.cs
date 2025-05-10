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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace project.Views
{
    /// <summary>
    /// Interaction logic for CustomerView.xaml
    /// </summary>
    public partial class CustomerView : UserControl
    {
        public CustomerView()
        {
            InitializeComponent();
            Loaded += CustomerView_Loaded;
        }
        private void CustomerView_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard fadeIn = (Storyboard)this.Resources["FadeInStoryboard"];
            fadeIn.Begin(this);
        }
    }
}
