using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace waitter
{
    /// <summary>
    /// sms 的摘要说明
    /// </summary>
    public class sms : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            Stream stream = request.InputStream;
            if (stream.Length != 0)
            {
                JObject json = new JObject();
                Random rad = new Random();//实例化随机数产生器rad；
                int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
                string code = value.ToString(); //转化为字符串；
                var url = "http://gw.api.taobao.com/router/rest";
                var appkey = "24469623";
                var secret = "8d47edc41e600a1bf0eb80ba0c3e43cb";
                var act = request["action"];
                string tpl1, tpl2;
                switch (act)
                {
                    case "login":
                        tpl1 = "SMS_71470009";
                        tpl2 = "TTS_71470017";
                        break;
                    case "regist":
                        tpl1 = "SMS_71470007";
                        tpl2 = "TTS_71470015";
                        break;
                    case "mobile":
                        tpl1 = "SMS_71470004";
                        tpl2 = "TTS_71470012";
                        break;
                    default:
                        tpl1 = "SMS_71470005";
                        tpl2 = "TTS_71470013";
                        break;
                }
                ITopClient client = new DefaultTopClient(url, appkey, secret);
                if (request["type"] == "1")
                {
                    AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                    req.SmsType = "normal";
                    req.SmsFreeSignName = "微特众包";
                    req.SmsParam = "{\"code\":\"" + code + "\",\"product\":\"微特众包\"}";
                    req.RecNum = request["mobile"];
                    req.SmsTemplateCode = tpl1;
                    AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                    if (rsp.Result != null)
                    {
                        json["code"] = code;
                        if (rsp.Result.Success)
                        {
                            json["status"] = true;
                            json["msg"] = rsp.Result.Msg;
                            context.Response.Write(json);
                        }
                        else
                        {
                            json["status"] = false;
                            json["msg"] = rsp.Result.Msg;
                            context.Response.Write(json);
                        }
                    }
                    else
                    {
                        json["code"] = rsp.ErrCode;
                        json["status"] = false;
                        json["msg"] = rsp.SubErrMsg;
                        context.Response.Write(json);
                    }
                }
                else
                {
                    AlibabaAliqinFcTtsNumSinglecallRequest req = new AlibabaAliqinFcTtsNumSinglecallRequest();
                    //req.Extend = "12345";
                    req.TtsParam = "{\"code\":\"" + code + "\",\"product\":\"微特众包\"}";
                    req.CalledNum = request["mobile"];
                    req.CalledShowNum = "02131314598";
                    req.TtsCode = tpl2;
                    AlibabaAliqinFcTtsNumSinglecallResponse rsp = client.Execute(req);
                    if (rsp.Result != null)
                    {
                        json["code"] = code;
                        if (rsp.Result.Success)
                        {
                            json["status"] = true;
                            json["msg"] = rsp.Result.Msg;
                            context.Response.Write(json);
                        }
                        else
                        {
                            json["status"] = false;
                            json["msg"] = rsp.Result.Msg;
                            context.Response.Write(json);
                        }
                    }
                    else
                    {
                        json["code"] = rsp.ErrCode;
                        json["status"] = false;
                        json["msg"] = rsp.SubErrMsg;
                        context.Response.Write(json);
                    }
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