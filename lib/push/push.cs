using cn.jpush.api;
using cn.jpush.api.push.mode;
using cn.jpush.api.push.notification;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;


public class topush
{
    //tag-标签，alias-别名，title-标题，content-内容，type-类型，subtitle-副标题
    public static void push(string tag, string alias, string title, string subtitle, string content, string type)
    {
        //string[] tags = tag.Split(',');
        JPushClient client;
        if (type == "shop")
        {
            client = new JPushClient(sysconf.jpush_app_key_shop, sysconf.jpush_master_secret_shop);
        }
        else
        {
            client = new JPushClient(sysconf.jpush_app_key, sysconf.jpush_master_secret);
        }
        Audience target;
        switch (type)
        {
            case "alert":
                target = Audience.all();
                break;
            case "verified":
                target = Audience.s_alias(alias);
                break;
            case "order":
                string[] tags = tag.Split(',');
                target = Audience.s_tag_and(tags);
                break;
            case "finish":
                target = Audience.s_alias(alias);
                break;
            case "shop":
                target = Audience.s_alias(alias);
                break;
            default:
                target = Audience.all();
                break;
        }
        PushPayload payload = ios(target, title, content, type, subtitle);
        try
        {
            var result = client.SendPush(payload);
        }
        catch
        {
            //
        }
        payload = android(target, title, content, type, subtitle);
        try
        {
            var result = client.SendPush(payload);
        }
        catch
        {
            //
        }
    }

    public static PushPayload ios(Audience audience, string title, string content, string type, string subtitle)
    {
        var options = new Options();
        options.apns_production = true;
        var sound = type == "order" ? "widget/notify.m4a" : "default";
        PushPayload pushPayload = new PushPayload()
        {
            platform = Platform.ios(),
            audience = audience,
            options = options
        };

        Hashtable alert = new Hashtable
        {
            ["title"] = title,
            ["subtitle"] = subtitle,
            ["body"] = content
        };

        pushPayload.notification = new Notification()
        {
            IosNotification = new IosNotification()
                .setAlert(alert)
                .setBadge(1)
                .setSound(sound)
                .AddExtra("type", type)
        };
        //pushPayload.message = Message.content(MSG_CONTENT);
        return pushPayload;
    }

    public static PushPayload android(Audience audience, string title, string content, string type, string subtitle)
    {
        var pushPayload = new PushPayload()
        {
            platform = Platform.android(),
            audience = audience,
            message = Message.content(content).AddExtras("type", type)
        };
        pushPayload.message.title = title;
        pushPayload.options.apns_production = true;
        return pushPayload;
    }
}
