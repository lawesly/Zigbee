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

namespace 物联网智能门禁系统
{
    /// <summary>
    /// select.xaml 的交互逻辑
    /// </summary>
    public partial class select : Window
    {
        public select()
        {
            InitializeComponent();
            this.Topmost = true;
        }

        private void but_select_Click(object sender, RoutedEventArgs e)
        {
            selectUserMessage();
        }

        private void but_read_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                text_cardid.Text = CardTools.scanCard().Trim();
                CardTools.readCardMessage();
            }
            catch
            {
                MessageBox.Show("读卡失败！");
            }
        }
    }
}
