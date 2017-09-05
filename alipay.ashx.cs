using Aop.Api;
using Aop.Api.Domain;
using Aop.Api.Request;
using Aop.Api.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace waitter
{
    /// <summary>
    /// alipay 的摘要说明
    /// </summary>
    public class alipay : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            Stream stream = req.InputStream;
            if (stream.Length != 0)
            {
                var OutTradeNo = req["out_trade_no"];
                var TotalAmount = req["total_fee"];
                string APPID = AlipayConstants.app_id;
                string APP_PRIVATE_KEY = AlipayConstants.private_key;
                string ALIPAY_PUBLIC_KEY = AlipayConstants.alipay_public_key;
                string CHARSET = "utf-8";
                IAopClient client = new DefaultAopClient("https://openapi.alipay.com/gateway.do", APPID, APP_PRIVATE_KEY, "json", "1.0", "RSA", ALIPAY_PUBLIC_KEY, CHARSET, false);
                AlipayTradeAppPayRequest request = new AlipayTradeAppPayRequest();
                //SDK已经封装掉了公共参数，这里只需要传入业务参数。以下方法为sdk的model入参方式(model和biz_content同时存在的情况下取biz_content)。
                AlipayTradeAppPayModel model = new AlipayTradeAppPayModel();
                model.Body = "微特众包-商家充值";
                model.Subject = "微特众包-商家充值";
                model.TotalAmount = TotalAmount;
                model.ProductCode = "1";
                model.OutTradeNo = OutTradeNo;
                model.TimeoutExpress = "30m";
                request.SetNotifyUrl(AlipayConstants.notify_url);
                request.SetBizModel(model);
                //这里和普通的接口调用不同，使用的是sdkExecute
                AlipayTradeAppPayResponse response = client.SdkExecute(request);
                //HttpUtility.HtmlEncode是为了输出到页面时防止被浏览器将关键参数html转义，实际打印到日志以及http传输不会有这个问题
                context.Response.Write(HttpUtility.HtmlEncode(response.Body));
                //context.Response.Write(response.Body);
                //页面输出的response.Body就是orderString 可以直接给客户端请求，无需再做处理。
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