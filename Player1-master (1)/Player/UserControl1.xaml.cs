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

namespace Player
{
    /// <summary>
    /// UserControl1.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        MainWindow mw1 = null;
        public UserControl1(MainWindow mw)
        {
            InitializeComponent();
            mw1 = mw;
        }
        private void Login_Click(object sender, RoutedEventArgs e)
        {
            mw1.GridPrincipal.Children.Remove(this);
        }
    }
}
