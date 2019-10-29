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
using MWRDemoDll;

namespace 物联网智能门禁系统
{
    /// <summary>
    /// setting.xaml 的交互逻辑
    /// </summary>
    public partial class setting : Window
    {
        public setting()
        {
            InitializeComponent();
            init();
            this.Topmost = true;
          
        }

        private void but_readCard_Click(object sender, RoutedEventArgs e)
        {
            readCard(text_CardID);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            insertUser();
        }

        private void but_update_Click(object sender, RoutedEventArgs e)
        {
            updatePad();
        }

        private void but_ad_read_Click(object sender, RoutedEventArgs e)
        {
            readCard(text_ad_cardid);
            but_ad_delectuser.IsEnabled = false;
        }

        private void but_selectuser_Click(object sender, RoutedEventArgs e)
        {
            string mes = DbTools.sqlSelectUserMessage(text_ad_cardid.Text);
            MessageBox.Show(mes);
            if (Users.UserLevel == "超级管理员" && mes != "查询失败")
            {
                but_ad_delectuser.IsEnabled = true;
            }
        }

        private void but_ad_delectuser_Click(object sender, RoutedEventArgs e)
        {
            DbTools.sqlExecuteNonQuery("delete from dbo.UserSignIn where UsID='"+text_ad_cardid.Text+"'");
            DbTools.sqlExecuteNonQuery("delete from dbo.Users where UsID='"+ text_ad_cardid.Text + "'");
            MessageBox.Show("删除成功！");
            but_ad_delectuser.IsEnabled = false;
        }

        private void but_ad_select_Click(object sender, RoutedEventArgs e)
        {
            text_ad_x_pad.IsEnabled = false;
            but_ad_update.IsEnabled = false;
            but_ad_delete.IsEnabled = false;
            but_ad_insert.IsEnabled = false;
            selectAdmin();
           
        }

        private void but_ad_insert_Click(object sender, RoutedEventArgs e)
        {
            if (text_ad_x_pad.Text.Trim() == "")
            {
                MessageBox.Show("请输入密码！");
            }
            else
            {
                DbTools.sqlExecuteNonQuery("insert dbo.Administrators(AdName,AdPassword,AdLevel) values('"+text_ad_adname.Text+"','"+text_ad_x_pad.Text+"','普通管理员')");
                MessageBox.Show("新增管理员成功！");
            }
        }

        private void text_ad_adname_TextChanged(object sender, TextChangedEventArgs e)
        {
            text_ad_x_pad.IsEnabled = false;
            but_ad_update.IsEnabled = false;
            but_ad_delete.IsEnabled = false;
            but_ad_insert.IsEnabled = false;
        }

        private void but_ad_update_Click(object sender, RoutedEventArgs e)
        {
            DbTools.sqlExecuteNonQuery("update dbo.Administrators set AdPassword='"+text_ad_x_pad.Text+"' where AdName='"+text_ad_adname.Text+"'");
            MessageBox.Show("密码更改成功！");
        }

        private void but_ad_delete_Click(object sender, RoutedEventArgs e)
        {
            if (text_ad_adname.Text == "admin")
            {
                MessageBox.Show("不能删除超级管理员！");
            }
            else
            {
                DbTools.sqlExecuteNonQuery("delete from dbo.Administrators where AdName='"+text_ad_adname.Text+"'");
                MessageBox.Show("删除成功！");
            }
        }
    }
}
