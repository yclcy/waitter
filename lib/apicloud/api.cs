using APICloud.Rest;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;


public class api
{
    public static string create(object obj, string factory)
    {
        var client = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var Model = client.Factory(factory);
        return Model.Create(obj);
    }

    public static string query(string filter, string factory)
    {
        var client = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var Model = client.Factory(factory);
        return Model.Query(filter);
    }

    public static string delete(string id, string factory)
    {
        var client = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var Model = client.Factory(factory);
        return Model.Delete(id);
    }

    public static string get(string id,string factory)
    {
        var resource = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var model = resource.Factory(factory);
        return model.Get(id);
    }

    public static string edit(string id, object body, string factory)
    {
        var client = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var Model = client.Factory(factory);
        return Model.Edit(id, body);
    }

    public static string upload(string path)
    {
        var client = new Resource(sysconf.apicloud_appid, sysconf.apicloud_appkey);
        var file = client.Factory("file");
        using (var fs = new FileStream(path, FileMode.Open))
        {
            var ret = file.Upload(fs);
            return ret;
        }
    }
}
