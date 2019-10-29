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

namespace 物联网智能门禁系统
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow: Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void but_load_Click(object sender, RoutedEventArgs e)
        {
            string level=DbTools.sqlUserSignIn(text_user.Text.Trim(), text_pad.Text.Trim());
            if (level == "用户不存在")
            {
                MessageBox.Show("登陆失败，用户不存在！");
            }
            else
            {
                MessageBox.Show(level+"用户："+text_user.Text+"\n欢迎进入物联网智能门禁系统！");
                Users.UserName = text_user.Text;
                Users.UserLevel = level;
                Main main = new Main();
                this.Visibility = Visibility.Collapsed;
                main.ShowDialog();
            }
        }
    }
}
