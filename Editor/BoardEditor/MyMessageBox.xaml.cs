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

namespace BoardEditor
{
    public enum MyMBType { OK, OKCANCEL };

    /// <summary>
    /// Interaction logic for MyessageBox.xaml
    /// </summary>
    public partial class MyMessageBox : Window
    {
        public MyMessageBox(String msg, MyMBType type)
        {
            InitializeComponent();
            this.lblMsg.Text = msg;
            if (type == MyMBType.OK)
                this.bCancel.Visibility = System.Windows.Visibility.Hidden;
        }
        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
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

        private String msg;

        private void OkBtn_MouseDown(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
    }
}
