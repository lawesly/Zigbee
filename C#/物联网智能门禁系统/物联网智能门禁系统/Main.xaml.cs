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
using System.Windows.Threading;


namespace 物联网智能门禁系统
{
    /// <summary>
    /// Main.xaml 的交互逻辑
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
            serialInit();
            cardInit();
            timerInit();
            this.WindowStyle = WindowStyle.None;
            this.Topmost = true;
        }



        private void Window_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(System.Environment.ExitCode);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            label_zt.Content = "欢迎" + Users.UserLevel + "用户：" + Users.UserName + "使用该系统！";
        }

        private void but_start_Click(object sender, RoutedEventArgs e)
        {
            if (but_start.Content.ToString() == "开始打卡" && init_zt == true)
            {
                startSelect();
            }
            else
            {
                closeSelect();
            }
        }

        private void but_setting_Click(object sender, RoutedEventArgs e)
        {
            setting set = new setting();
            set.ShowDialog();
        }

        private void but_select_Click(object sender, RoutedEventArgs e)
        {
            select sel = new select();
            sel.ShowDialog();
        }

        private void label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void label_MouseEnter(object sender, MouseEventArgs e)
        {
            lable_close.Background = new SolidColorBrush() { Color = Colors.Yellow };
        }

        private void lable_close_MouseLeave(object sender, MouseEventArgs e)
        {
            lable_close.Background = new SolidColorBrush() { Color = Colors.White };
        }

    }
}
