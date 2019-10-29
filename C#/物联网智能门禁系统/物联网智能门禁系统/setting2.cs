using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace 物联网智能门禁系统
{
    public partial class setting : Window
    {
        void selectAdmin()
        {
           string pad= DbTools.sqlSelectPad(text_ad_adname.Text);
            if (pad == "用户不存在")
            {
                but_ad_insert.IsEnabled = true;
                text_ad_x_pad.IsEnabled = true;
                label_ad_adzt.Content = "状态：账号不存在，可新增！";
            }
            else
            {
                text_ad_x_pad.IsEnabled = true;
                but_ad_update.IsEnabled = true;
                but_ad_delete.IsEnabled = true;
                label_ad_adzt.Content = "状态：账号存在，请进行下一步操作！";
                text_ad_y_pad.Text = pad;
            }
        }
        void init()
        {
            text_name.Text = Users.UserName;
            if (Users.UserLevel == "超级管理员")
            {
                text_ad_adname.IsEnabled = true;
                but_ad_select.IsEnabled = true;
            }
        }
        void updatePad()
        {
            if (DbTools.sqlSelectAdminPad(text_name.Text, text_y_pad.Text))
            {
                DbTools.sqlExecuteNonQuery("update dbo.Administrators set AdPassword='" + text_x_pad.Text + "' where AdName='" + text_name.Text + "'");
                MessageBox.Show("修改成功！");
            }
            else
            {
                MessageBox.Show("密码错误！");
            }
        }
        void readCard(TextBox textbox)
        {
            try
            {
                textbox.Text = CardTools.scanCard().ToString().Trim();
                CardTools.readCardMessage();
            }
            catch
            {
                MessageBox.Show("读卡失败！");
            }

        }

        void insertUser()
        {
            if (text_CardID.Text.Trim() != "" && text_user.Text.Trim() != "")
            {
                try
                {
                    DbTools.sqlExecuteNonQuery("insert into dbo.Users(UsID,UsName,UsCreateTime) values('" + text_CardID.Text + "','" + text_user.Text + "','" + DateTime.Now.ToString() + "')");
                    MessageBox.Show("信息录入成功！");
                }
                catch
                {
                    MessageBox.Show("信息录入失败！");
                }
            }
            else
            {
                MessageBox.Show("请完整填写信息！");
            }
        }
    }
}
