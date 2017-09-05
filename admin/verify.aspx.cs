using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waitter.admin
{
    public partial class verify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            string filter = "{\"where\":{\"username\":\"" + query.Text + "\"},\"limit\":1}";
            var result = JArray.Parse(api.query(filter, "user"));
            if (result[0]["id"] != null)
            {
                var username = "";
                if (result[0]["papersdetail"] != null)
                {
                    if (result[0]["papersdetail"]["idcard"] != null)
                    {
                        username = result[0]["papersdetail"]["idcard"]["name"].ToString();
                    }
                    else
                    {
                        username = result[0]["username"].ToString();
                    }
                }
                else
                {
                    username = result[0]["username"].ToString();
                }
                userid.Value = result[0]["id"].ToString();
                ph1.Visible = true;
                userinfo.Text = username;
            }
        }

        protected void confirm_Click(object sender, EventArgs e)
        {
            var obj = new
            {
                verified = verified.Checked,
                study = study.Checked,
                expire = jiankang.Checked
            };
            var result = JObject.Parse(api.edit(userid.Value, obj, "user"));
            msg.Text = "保存成功";
            if (verified.Checked && result["id"] != null)
            {
                topush.push("", userid.Value, "", "", "尊敬的用户，您的实名认证已经通过审核啦！","verified");
            }
        }
    }
}