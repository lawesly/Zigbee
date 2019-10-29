using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.IO.Ports;

namespace 物联网智能门禁系统
{
    public partial class Main : Window
    {
        #region 使用到的变量
        DispatcherTimer selectTimer;
        DispatcherTimer mTimer;
       
        SerialPort com;
        string cardID = null;
        int i = 0;
        bool init_zt = true;
        #endregion
        #region 开启及关闭查询
        void startSelect()//开始读卡
        {
            selectTimer.Start();
            but_setting.IsEnabled = false;
            but_select.IsEnabled = false;
            but_start.Content = "关闭系统";
        }
        
        void closeSelect()//结束读卡线程
        {
            selectTimer.Stop();
            but_setting.IsEnabled = true;
            but_select.IsEnabled = true;
            but_start.Content = "开始打卡";
        }
        #endregion

        private string comnum = "COM1";

        void openZigbee()
        {
            com.Write(new byte[1] { 0xff},0,1);
        }
        void closeZigBee()
        {
            com.Write(new byte[1] { 0x00 }, 0, 1);
        }


        #region 系统硬件初始化
        void cardInit()
        {
            
            if (CardTools.connect()==false)
            {
                init_zt = false;
                label_zt.Content = "读卡器连接失败！";
            }
            
        }
        void serialInit()
        {
            com = new SerialPort(comnum, 38400);
            com.Open();
            
            if (com.IsOpen == false)
            {
                label_zt.Content = "ZigBee初始化失败！";
                init_zt = false;
            }
            
        }
        #endregion
        #region 时钟初始化
        void timerInit()//时钟初始化
        {
            selectTimer = new DispatcherTimer();
            selectTimer.Interval = TimeSpan.FromMilliseconds(100);
            selectTimer.Tick += new EventHandler((obj,eve)=> 
            {
                try
                {
                    cardID = CardTools.scanCard().Trim();//读卡操作
                }
                catch
                {
                    cardID = null;
                }
                if (cardID != null && cardID != "")
                {
                    string name = DbTools.sqlSelectUserAndInsertMessage(cardID, DateTime.Now);
                    CardTools.readCardMessage();
                    if (name != "非法卡")
                    {
                        mTimer.Start();
                        label_zt.Content = "欢迎打卡：" + name;
                        
                    }
                    else
                    {
                        label_zt.Content = "非法卡，不与进入！";
                    }
                    cardID = null;
                }

            });
            mTimer = new DispatcherTimer();
            mTimer.Interval = TimeSpan.FromMilliseconds(10);
            mTimer.Tick += new EventHandler((obj, eve) =>
            {
                //门禁动画
                if (i <=500)
                {
                    i++;
                }
                if (i == 1)
                {
                    selectTimer.Stop();
                    openZigbee();
                }
                if (i<=100)
                {
                    grid_lm.Margin = new Thickness(134 - i, 41, 0, 0);
                    grid_rm.Margin = new Thickness(263 + i, 41, 0, 0);
                }
                else
                if (i == 500)
                {
                    i = 0;
                    mTimer.Stop();
                    grid_lm.Margin = new Thickness(134, 41, 0, 0);
                    grid_rm.Margin = new Thickness(263, 41, 0, 0);
                    selectTimer.Start();
                    closeZigBee();
                }
            });
            #endregion

        }
    }
}
