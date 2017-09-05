using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace waitter
{
    /// <summary>
    /// getOrderId 的摘要说明
    /// </summary>
    public class getOrderId : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            Stream stream = request.InputStream;
            if (stream.Length != 0)
            {
                int total_fee = Convert.ToInt32(request["total_fee"]);
                string out_trade_no = request["out_trade_no"];
                WxPayData data = new WxPayData();
                data.SetValue("body", "微特众包-商家充值");//商品描述
                //data.SetValue("attach", "test");//附加数据
                //data.SetValue("out_trade_no", WxPayApi.GenerateOutTradeNo());//订单号
                data.SetValue("out_trade_no", out_trade_no);//订单号
                data.SetValue("total_fee", total_fee);//总金额
                data.SetValue("time_start", DateTime.Now.ToString("yyyyMMddHHmmss"));//交易起始时间
                data.SetValue("time_expire", DateTime.Now.AddMinutes(10).ToString("yyyyMMddHHmmss"));//交易结束时间
                data.SetValue("trade_type", "APP");//交易类型
                WxPayData result = WxPayApi.UnifiedOrder(data);//调用统一下单接口
                if (result.IsSet("prepay_id"))
                {
                    WxPayData order = new WxPayData();
                    order.SetValue("appid", WxPayConfig.APPID);
                    order.SetValue("partnerid", WxPayConfig.MCHID);
                    order.SetValue("noncestr", WxPayApi.GenerateNonceStr());
                    order.SetValue("prepayid", result.GetValue("prepay_id"));
                    order.SetValue("timestamp", WxPayApi.GenerateTimeStamp());
                    order.SetValue("package", "Sign=WXPay");
                    order.SetValue("sign", order.MakeSign());
                    context.Response.Write(order.ToJson());
                }
            }
            else
            {
                context.Response.Write("");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}