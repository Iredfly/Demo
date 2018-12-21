using DiYi.Msg.SMS;
using DiYi.Msg.SMS.DanhanTc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiYi.Demo.Service
{
    public class SendMsgService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="content"></param>
        /// <param name="head"></param>
        /// <param name="phone"></param>
        /// <param name="businessId"></param>
        /// <returns></returns>
        public int SendMsg(string content, string head, string phone, int businessId)
        {
            string msgId = "";
            //大汉
            if (businessId == 1)
            {
                DahanSmsHelper dh = new DahanSmsHelper();
                if (dh.SendSmsData(head, content, phone, ref msgId))
                {
                    return 1;
                }
                return 0;
            }


            //助通
            if (businessId == 2)
            {
                ZhuTongSmsHelper zt = new ZhuTongSmsHelper();
                if (zt.SendSmsData(head, content, phone, ref msgId))
                {
                    return 2;
                }

                return 0;
            }

            return 0;
        }



    }
}
