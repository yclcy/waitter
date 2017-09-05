using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Xml;
using System.Xml.Serialization;
using APICloud.Rest;
using APICloud.Push;
using APICloud.Analytics;
using Newtonsoft.Json.Linq;
using WxPayAPI;

namespace waitter
{
    /// <summary>
    /// notify 的摘要说明
    /// </summary>
    public class notify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            System.IO.Stream s = context.Request.InputStream;
            int count = 0;
            byte[] buffer = new byte[1024];
            StringBuilder builder = new StringBuilder();
            while ((count = s.Read(buffer, 0, 1024)) > 0)
            {
                builder.Append(Encoding.UTF8.GetString(buffer, 0, count));
            }
            s.Flush();
            s.Close();
            s.Dispose();
            Log.Info(this.GetType().ToString(), "从微信服务器收到数据 : " + builder.ToString());
            WxPayData data = new WxPayData();
            try
            {
                data.FromXml(builder.ToString());
            }
            catch (WxPayException ex)
            {
                //若签名错误，则立即返回结果给微信支付后台
                WxPayData wxres = new WxPayData();
                wxres.SetValue("return_code", "FAIL");
                wxres.SetValue("return_msg", ex.Message);
                Log.Error(this.GetType().ToString(), "Sign check error : " + wxres.ToXml());
                context.Response.Write(wxres.ToXml());
                context.Response.End();
            }
            
            Log.Info(this.GetType().ToString(), "Check sign success");
            var money =  Convert.ToDouble(data.GetValue("total_fee").ToString());
            var model = "record";
            var id = data.GetValue("out_trade_no").ToString();
            var result = JObject.Parse(sysconf.toget(model, id));
            //判断订单状态为未处理，以及微信服务器返回的APPID是否正确
            if (Convert.ToInt32(result["status"]) == 0 && data.GetValue("appid").ToString() == WxPayAPI.WxPayConfig.APPID)
            {
                var list = new List<Object>();
                // 修改订单状态
                list.Add(new
                {
                    method = "PUT",
                    path = "/mcm/api/record/" + result["id"],
                    body = new
                    {
                        status = 1, //0-处理中, 1-已处理
                        from = data.GetValue("openid").ToString()
                    }
                });
                // 增加商户账户余额
                string inc = "{\"$inc\":{\"balance\":" + money / 100.0 + "}}";
                list.Add(new
                {
                    method = "PUT",
                    path = "/mcm/api/user/" + result["user"],
                    body = JObject.Parse(inc)
                });
                var res = JArray.Parse(sysconf.tobatch(list));
                //打印结果
                //Log.Info(this.GetType().ToString(), res.ToString());
                var err = 0;
                for (var i = 0; i < res.Count; i++)
                {
                    if (res[i]["id"] == null)
                    {
                        err += 1;
                    }
                }

                if (err > 0)
                {
                    Log.Error(this.GetType().ToString(), "微信服务器端处理成功，本地服务器端处理失败");
                    // 写入系统日志
                    var log = new List<Object>();
                    log.Add(new
                    {
                        method = "POST",
                        path = "/mcm/api/log",
                        body = new
                        {
                            type = 1, //0-成功，1-失败
                            title = "微信充值处理失败",
                            money = money / 100.0,
                            record = id
                        }
                    });
                    sysconf.tobatch(log);
                }
                else
                {
                    Log.Info(this.GetType().ToString(), "微信充值处理成功");
                    // 写入系统日志
                    var log = new List<Object>();
                    log.Add(new
                    {
                        method = "POST",
                        path = "/mcm/api/log",
                        body = new
                        {
                            type = 0, //0-成功，1-失败
                            title = "微信充值处理成功",
                            money = money / 100.0,
                            record = id
                        }
                    });
                    sysconf.tobatch(log);
                    WxPayData wxres1 = new WxPayData();
                    wxres1.SetValue("return_code", "SUCCESS");
                    wxres1.SetValue("return_msg", "");
                    context.Response.Write(wxres1.ToXml());
                    context.Response.End();
                }
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