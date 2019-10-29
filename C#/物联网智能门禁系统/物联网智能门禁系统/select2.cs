using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace 物联网智能门禁系统
{
    public partial class select : Window
    {
        void selectUserMessage()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("姓名");
            dt.Columns.Add("卡号");
            dt.Columns.Add("打卡时间");
            data_message.ItemsSource = dt.DefaultView;
            if (text_cardid.Text.Trim() == "" && text_user.Text.Trim() == "")
            {

                try
                {
                    DbTools.sqlExecuteReader("select dbo.UserSignIn.UsID,UsName,UsSignInTime from dbo.Users,dbo.UserSignIn where UsSignInTime>'" + date_start.Text + "' and UsSignInTime<'" + date_close.Text + "' and dbo.Users.UsID=dbo.UserSignIn.UsID", dt);
                }
                catch
                {
                    MessageBox.Show("查询失败！");
                }
            }
            else if (text_cardid.Text.Trim() != "" && text_user.Text.Trim() == "")
            {
                try
                {
                    DbTools.sqlExecuteReader("select dbo.UserSignIn.UsID,UsName,UsSignInTime from dbo.Users,dbo.UserSignIn where UsSignInTime>'" + date_start.Text + "' and UsSignInTime<'" + date_close.Text + "' and dbo.Users.UsID=dbo.UserSignIn.UsID and dbo.Users.UsID='"+text_cardid.Text+"'", dt);
                }
                catch
                {
                    MessageBox.Show("查询失败！");
                }
            }
            else if (text_cardid.Text.Trim() == "" && text_user.Text.Trim() != "")
            {
                try
                {
                    DbTools.sqlExecuteReader("select dbo.UserSignIn.UsID,UsName,UsSignInTime from dbo.Users,dbo.UserSignIn where UsSignInTime>'" + date_start.Text + "' and UsSignInTime<'" + date_close.Text + "' and dbo.Users.UsID=dbo.UserSignIn.UsID and dbo.Users.UsName='" + text_user.Text + "'", dt);
                }
                catch
                {
                    MessageBox.Show("查询失败！");
                }
            }
            else if (text_cardid.Text.Trim() != "" && text_user.Text.Trim() != "")
            {
                try
                {
                    DbTools.sqlExecuteReader("select dbo.UserSignIn.UsID,UsName,UsSignInTime from dbo.Users,dbo.UserSignIn where UsSignInTime>'" + date_start.Text + "' and UsSignInTime<'" + date_close.Text + "' and dbo.Users.UsID=dbo.UserSignIn.UsID and dbo.Users.UsName='" + text_user.Text + "' and dbo.Users.UsID='" + text_cardid.Text + "'", dt);
                }
                catch
                {
                    MessageBox.Show("查询失败！");
                }
            }


        }
    }
}
