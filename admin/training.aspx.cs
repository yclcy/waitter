using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waitter.admin
{
    public partial class training : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            var t = title.Text;
            var c = content.Text;
            JArray arr = new JArray();
            List<object> list = new List<object>();
            JObject obj = new JObject();
            obj["method"] = "POST";
            obj["path"] = "/mcm/api/article";
            JObject body = new JObject();
            body["type"] = "6";
            body["title"] = t;
            body["content"] = c;
            body["show"] = "true";
            obj["body"] = body;
            arr.Add(obj);
            list.Add(new
            {
                method = "POST",
                path = "/mcm/api/article",
                body = new
                {
                    title = t, //标题
                    content = c, //正文
                    type = 6, //分类
                    show = true, //是否显示
                    display = 0 //阅读次数
                }
            });
            string json = "{\"$inc\":{\"fund\":1}}";
            list.Add(new
            {
                method = "PUT",
                path = "/mcm/api/sysConfig/5928e8c9d345ab9b2c83102f",
                body = JObject.Parse(json)
            });
            //Response.Write(JObject.Parse(json).ToString());
            Response.Write(sysconf.tobatch(list));
        }
    }
}