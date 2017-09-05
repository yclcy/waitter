using APICloud.Rest;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waitter.admin
{
    public partial class alert : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void submit_Click(object sender, EventArgs e)
        {
            var t1 = title.Text.Trim();
            var c1 = content.Text.Trim();
            var d1 = desc.Text;
            var obj = new
            {
                content = c1,
                title = t1,
                type = type.SelectedValue,
                show = true,
                display = 0,
                isnew = isnew.Checked,
                ishot = ishot.Checked,
                recommend = recommend.Checked
            };
            var result = JObject.Parse(api.create(obj, "article"));
            if (result["id"] != null)
            {
                result_label.Text = "添加成功";
                var id = result["id"].ToString();
                topush.push("", "", t1, "", c1, "alert");
            }
            else
            {
                result_label.Text = result.ToString();
            }
        }
    }
}