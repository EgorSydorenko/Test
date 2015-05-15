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
using System.Windows.Shapes;

namespace BoardEditor
{
    /// <summary>
    /// Interaction logic for About.xaml
    /// </summary>
    public partial class MyAbout : Window
    {
        public MyAbout()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            DoubleAnimation anim = new DoubleAnimation(0.0, 1.0, new Duration(TimeSpan.FromSeconds(3)));
            this.tbName.BeginAnimation(OpacityProperty, anim);
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void btnClose_MouseEnter(object sender, MouseEventArgs e)
        {
            btnClose.Background = Brushes.DeepPink;
        }

        private void btnClose_MouseLeave(object sender, MouseEventArgs e)
        {
            btnClose.Background = Brushes.Transparent;
        }
    }
}
