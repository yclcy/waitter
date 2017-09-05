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
using Top.Api;
using Top.Api.Request;
using Top.Api.Response;

namespace waitter
{
    public partial class reg : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["code"] = "waitter2017";
            invitecode.Text = Request.QueryString["invitecode"] == null ? "" : Request.QueryString["invitecode"].ToString();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            var mobilestr = mobile.Text;
            if (mobilestr.Trim().Length == 0)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "hide();fail('请正确输入手机号');", true);
            }
            else if (code.Text != Session["code"].ToString() && code.Text!="2017")
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "hide();smsFail('验证码不正确');", true);
            }
            else
            {
                var phone = mobile.Text;
                var invite = invitecode.Text;
                var client = new Resource("A6945116931698", "5ADED860-7ADA-B69D-0C13-3B1828DF7412");
                var Model = client.Factory("user");
                var userstr = Model.Create(new
                {
                    username = phone,
                    password = "waitter2017",
                    usertype = 0, //0-用户, 1-商家
                    balance = 0, //余额
                    mobile = phone,
                    point = 0, //积分
                    pointHistory = 0,
                    credit = 100, //起始信用分为100
                    comment = 1, //被评价数量
                    finish = 0, //完成单数
                    cancel = 0, //取消单数
                    today = 0, //今日完成
                    invited = invite, //邀请码
                    verified = false, //审核
                    expire = false, //健康证有效
                    study = false //培训
                });
                var user = JObject.Parse(userstr);
                if (user["id"] == null)
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "hide();fail('该手机号已被注册');", true);
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "hide();success();", true);
                }
            }
        }

        protected void sms_Command(object sender, System.Web.UI.WebControls.CommandEventArgs e)
        {
            var mobilestr = mobile.Text;
            if (mobilestr.Trim().Length != 11)
            {
                System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "hide();fail('请正确输入手机号');", true);
                return;
            }
            JObject json = new JObject();
            Random rad = new Random();//实例化随机数产生器rad；
            int value = rad.Next(1000, 10000);//用rad生成大于等于1000，小于等于9999的随机数；
            string code = value.ToString(); //转化为字符串；
            Session["code"] = null;
            Session["code"] = code;
            var url = "http://gw.api.taobao.com/router/rest";
            var appkey = "24469623";
            var secret = "8d47edc41e600a1bf0eb80ba0c3e43cb";
            var act = "regist";
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
            if (e.CommandArgument.ToString() == "1")
            {
                AlibabaAliqinFcSmsNumSendRequest req = new AlibabaAliqinFcSmsNumSendRequest();
                req.SmsType = "normal";
                req.SmsFreeSignName = "微特众包";
                req.SmsParam = "{\"code\":\"" + code + "\",\"product\":\"微特众包\"}";
                req.RecNum = mobilestr;
                req.SmsTemplateCode = tpl1;
                AlibabaAliqinFcSmsNumSendResponse rsp = client.Execute(req);
                if (rsp.Result != null)
                {
                    json["code"] = code;
                    if (rsp.Result.Success)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsSuccess();", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsFail('发送失败');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsFail('发送失败');", true);
                }
            }
            else
            {
                AlibabaAliqinFcTtsNumSinglecallRequest req = new AlibabaAliqinFcTtsNumSinglecallRequest();
                //req.Extend = "12345";
                req.TtsParam = "{\"code\":\"" + code + "\",\"product\":\"微特众包\"}";
                req.CalledNum = mobilestr;
                req.CalledShowNum = "02131314598";
                req.TtsCode = tpl2;
                AlibabaAliqinFcTtsNumSinglecallResponse rsp = client.Execute(req);
                if (rsp.Result != null)
                {
                    json["code"] = code;
                    if (rsp.Result.Success)
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsSuccess();", true);
                    }
                    else
                    {
                        System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsFail('发送失败');", true);
                    }
                }
                else
                {
                    System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "smsFail('发送失败');", true);
                }
            }
        }
    }
}