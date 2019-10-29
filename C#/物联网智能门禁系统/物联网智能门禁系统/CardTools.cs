using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MWRDemoDll;

namespace 物联网智能门禁系统
{
    class CardTools
    {
        public static string scanCard()//读取卡号
        {
           return  MifareRFEYE.Instance.Search().Model.ToString();
        }
        public static void readCardMessage()//读取卡片信息，用不到，主要是可以让读卡器鸣响做提示
        {
            MifareRFEYE.Instance.AuthCardPwd();
            MifareRFEYE.Instance.ReadString();
        }
        public static bool connect()
        {
            ResultMessage res = MifareRFEYE.Instance.ConnDevice();
            if (res.Result != Result.Success)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
