using Aop.Api;
using Aop.Api.Util;
using APICloud.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Web;
using WxPayAPI;

namespace waitter
{
    /// <summary>
    /// alipaynotify 的摘要说明
    /// </summary>
    public class alipaynotify : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest req = context.Request;
            Stream stream = req.InputStream;
            if (stream.Length != 0)
            {
                string ALIPAY_PUBLIC_KEY = AlipayConstants.alipay_public_key;
                string CHARSET = "utf-8";
                bool flag = AlipaySignature.RSACheckV1(GetRequestPost(context), ALIPAY_PUBLIC_KEY, CHARSET, "RSA", false);
                if (flag == true) {
                    if (context.Request.Form["app_id"] == AlipayConstants.app_id)
                    {
                        string out_trade_no = context.Request.Form["out_trade_no"];
                        var client = new Resource("A6945116931698", "5ADED860-7ADA-B69D-0C13-3B1828DF7412");
                        var Model = client.Factory("record");
                        var result = Model.Get(out_trade_no);
                        Log.Info(this.GetType().ToString(), result);
                        var ret = JObject.Parse(result);
                        if (Convert.ToInt32(ret["status"]) == 0 && Convert.ToDouble(ret["money"]) == Convert.ToDouble(context.Request.Form["total_amount"])) //判断订单状态为未处理
                        {
                            if(context.Request.Form["trade_status"] == "TRADE_SUCCESS"|| context.Request.Form["trade_status"] == "TRADE_FINISHED")
                            {
                                //修改订单状态
                                var json = Model.Edit(out_trade_no, new { status = 1, from = context.Request.Form["buyer_logon_id"] });
                                JObject r = JObject.Parse(json);
                                if (r["id"] != null)
                                {  //修改订单状态成功
                                    var userid = r["user"].ToString();
                                    Model = client.Factory("user");
                                    var user = Model.Get(userid);
                                    Log.Info(this.GetType().ToString(), user);
                                    var user_json = JObject.Parse(user);
                                    if (user_json["id"] != null)
                                    {
                                        //Decimal.Round(money / 100, 2);
                                        var b = Math.Round(Convert.ToDouble(user_json["balance"].ToString()) + Convert.ToDouble(context.Request.Form["total_amount"]), 2);
                                        Model = client.Factory("user");
                                        var userresult = Model.Edit(userid, new { balance = b }); //增加余额
                                        var result_json = JObject.Parse(userresult);
                                        Log.Info(this.GetType().ToString(), userresult);
                                        if (result_json["id"] != null) //增加余额成功
                                        {
                                            Log.Info(this.GetType().ToString(), "支付宝-商家充值/订单状态处理成功");
                                            context.Response.Write("success");
                                        }
                                        else
                                        {   //订单处理失败，状态返回为未处理
                                            Model = client.Factory("record");
                                            Model.Edit(out_trade_no, new { status = 0});
                                            Log.Error(this.GetType().ToString(), "支付宝-商家充值/订单处理失败");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            Log.Info("支付宝回调", "订单状态改变"+ ret["status"]+",支付宝返回订单金额为" + Convert.ToDouble(context.Request.Form["total_amount"])+"，订单金额为"+ Convert.ToDouble(ret["money"])+"，订单状态为"+ Convert.ToInt32(ret["status"]));
                        }
                    }
                }
                else
                {
                    Log.Info("支付宝回调", "验证不通过");
                }
            }
        }

        public Dictionary<string, string> GetRequestPost(HttpContext context)
        {
            int i = 0;
            Dictionary<string, string> sArray = new Dictionary<string, string>();
            NameValueCollection coll;
            //Load Form variables into NameValueCollection variable.
            coll = context.Request.Form;

            // Get names of all forms into a string array.
            String[] requestItem = coll.AllKeys;

            for (i = 0; i < requestItem.Length; i++)
            {
                sArray.Add(requestItem[i], context.Request.Form[requestItem[i]]);
            }
            return sArray;
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