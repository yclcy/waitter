using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waitter.admin
{
    public partial class articleedit : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JObject obj = JObject.Parse(getData());
                title.Text = obj["title"].ToString();
                content.Text = obj["content"].ToString();
            }
        }
        public string getData()
        {
            return api.get(Request.QueryString["id"].ToString(), "article");
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            var body = new
            {
                title = title.Text,
                content = content.Text
            };
            api.edit(Request.QueryString["id"].ToString(), body, "article");
            System.Web.UI.ScriptManager.RegisterStartupScript(this.up1, this.GetType(), "null", "success();", true);
        }
    }
}