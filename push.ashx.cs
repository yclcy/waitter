using cn.jpush.api;
using cn.jpush.api.common;
using cn.jpush.api.common.resp;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace waitter
{
    /// <summary>
    /// push 的摘要说明
    /// </summary>
    public class push : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            HttpRequest request = context.Request;
            Stream stream = request.InputStream;
            if (stream.Length > 0)
            {
                var type = request["type"];
                var title = request["title"];
                var content = request["content"];
                var tag = type == "order" ? request["tag"] : "";
                var alias = (type == "finish" || type == "shop") ? request["tag"] : "";
                var subtitle = request["subtitle"];
                topush.push(tag, alias, title, "", content, type);
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