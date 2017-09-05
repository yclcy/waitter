using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace waitter
{
    /// <summary>
    /// gettime 的摘要说明
    /// </summary>
    public class gettime : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(System.DateTime.Now.ToString());
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