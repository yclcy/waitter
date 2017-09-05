using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json.Linq;
using System.IO;

namespace waitter.admin
{
    public partial class banner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string filter = "{\"fields\":[\"id\",\"title\"],\"where\":{\"show\":true,\"type\":{\"inq\":[1,2]}},\"limit\":2000}";
            var result = JArray.Parse(api.query(filter, "article"));
            for(var i = 0; i < result.Count; i++)
            {
                ListItem item = new ListItem();
                item.Text = result[i]["title"].ToString();
                item.Value = result[i]["id"].ToString();
                notice.Items.Add(item);
            }
            var bannerarr = JArray.Parse(api.query("{\"limit\":2000}", "banner"));
            
            for (var i = 0; i < bannerarr.Count; i++)
            {
                ListItem item = new ListItem();
                var show = (Convert.ToBoolean(bannerarr[i]["show"]) == true) ? "可见" : "不可见";
                var position = bannerarr[i]["position"].ToString() == "home" ? "首页" : "邀请有奖";
                item.Text = "<img width='60' src='" + bannerarr[i]["url"].ToString() + "' /> / " + show + " / "+position;
                item.Value = bannerarr[i]["id"].ToString();
                bannerlist.Items.Add(item);
            }
        }

        protected void submit_Click(object sender, EventArgs e)
        {
            //msg.Text = Request.Form["uploadfile"];
            var result = JObject.Parse(api.upload(Server.MapPath("/") + imgurl.Text));
            if (result["id"] != null)
            {
                var obj = new
                {
                    type = type.SelectedIndex,
                    url = result["url"],
                    show = true,
                    position = position.SelectedValue,
                    target = target.Text
                };
                var res = JObject.Parse(api.create(obj, "banner"));
                if (res["id"] != null)
                {
                    msg.Text = "添加成功";
                }
                else
                {
                    msg.Text = "添加失败";
                }
            }
            else
            {
                msg.Text = "添加失败";
            }
        }

        protected void notice_SelectedIndexChanged(object sender, EventArgs e)
        {
            target.Text = notice.SelectedValue;
        }

        protected void page_SelectedIndexChanged(object sender, EventArgs e)
        {
            target.Text = page.SelectedValue;
        }

        protected void delete_Click(object sender, EventArgs e)
        {
            var id = bannerlist.SelectedValue;
            api.delete(id, "banner");
            bannerlist.Items.Clear();
        }
    }
}