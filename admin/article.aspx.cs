using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace waitter.admin
{
    public partial class article : System.Web.UI.Page
    {
        public JArray category;
        protected void Page_Load(object sender, EventArgs e)
        {
            getData();
        }

        public void getData()
        {
            string filter, result;
            filter = "{\"fields\":[\"id\",\"title\",\"type\",\"show\"],\"where\":{\"type\":{\"ne\":null}},\"limit\":2000}";
            result = api.query(filter, "article");
            DataTable dt = updateInfo(result);
            dv1.DataSource = dt;
            dv1.DataBind();
        }

        public DataTable getDataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("id", typeof(System.String));
            dt.Columns.Add("title", typeof(System.String));
            dt.Columns.Add("category", typeof(System.String));
            dt.Columns.Add("show", typeof(System.Boolean));
            return dt;
        }
        /// <summary>  
        /// json转换为DataTable  
        /// </summary>  
        /// <param name="json">需要转化的json格式字符串</param>  
        /// <returns></returns>  
        public DataTable updateInfo(string json)
        {
            string filter = "{\"fields\":[\"type\",\"title\"],\"limit\":2000,\"order\":\"type ASC\"}";
            string result = api.query(filter, "article_category");
            //rs.Text = result;
            category = JArray.Parse(result);

            var obj = JArray.Parse(json);
            DataRow dr;
            DataTable dt = getDataTable();
            for(var i=0;i<obj.Count;i++)
            {
                dr = dt.NewRow();
                dt.Rows.Add(dr);
                dr["id"] = obj[i]["id"];
                dr["title"] = obj[i]["title"];
                dr["category"] = category[Convert.ToInt32(obj[i]["type"])]["title"];
                dr["show"] = obj[i]["show"];
            }
            return dt;
        }

        protected void Unnamed_Command(object sender, CommandEventArgs e)
        {
            var id = e.CommandArgument.ToString();
            var body = new
            {
                show = Convert.ToBoolean(e.CommandName)
            };
            api.edit(id, body, "article");
            getData();
        }
    }
}