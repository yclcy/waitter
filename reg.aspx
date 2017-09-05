<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="reg.aspx.cs" Inherits="waitter.reg" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>微特众包</title>
    <meta name="viewport" content="maximum-scale=1.0,minimum-scale=1.0,user-scalable=0,width=device-width,initial-scale=1.0" />
    <meta name="format-detection" content="telephone=no,email=no,date=no,address=no" />
    <link href="Content/aui/aui.css" rel="stylesheet" />
    <link href="Content/aui/api.css" rel="stylesheet" />
    <script src="Scripts/jquery-1.10.2.min.js"></script>
    <script src="Scripts/aui-dialog.js"></script>
    <script src="Scripts/aui-toast.js"></script>
    <style>
        .aui-btn-info{background-color:#39c8d9 !important;}
        a{color:#39c8d9 !important;}
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager runat="server" EnablePartialRendering="true"></asp:ScriptManager>
        <asp:UpdatePanel runat="server" ID="up1">
            <ContentTemplate>
                <div class="aui-content aui-margin-b-15 aui-margin-t-10">
                    <div class="aui-content-padded">
                        <p class="aui-text-center aui-font-size-20">欢迎注册</p>
                    </div>
                    <ul class="aui-list aui-form-list">
                        <li class="aui-list-item">
                            <div class="aui-list-item-inner">
                                <div class="aui-list-item-label">
                                    手机号
                                </div>
                                <div class="aui-list-item-input">
                                    <asp:TextBox runat="server" ID="mobile" pattern="[0-9]*" placeholder="请输入手机号"></asp:TextBox>
                                </div>
                                <div class="aui-list-item-right" style="width:140px;"></div>
                            </div>
                        </li>
                        <li class="aui-list-item">
                            <div class="aui-list-item-inner">
                                <div class="aui-list-item-label">
                                    验证码
                                </div>
                                <div class="aui-list-item-input">
                                    <asp:TextBox runat="server" pattern="[0-9]*" ID="code" placeholder="请输入短信验证码"></asp:TextBox>
                                </div>
                                <div class="aui-list-item-right" style="width:140px;text-align:right !important;">
                                    <asp:LinkButton OnClientClick="return send('发送验证码')" runat="server" ID="sms" CssClass="aui-font-size-14 aui-margin-r-5" OnCommand="sms_Command" CommandArgument="1" Text=""></asp:LinkButton>
                                </div>
                            </div>
                        </li>
                        <li class="aui-list-item">
                            <div class="aui-list-item-inner">
                                <div class="aui-list-item-label">
                                    邀请码
                                </div>
                                <div class="aui-list-item-input">
                                    <asp:TextBox runat="server" Text="" ID="invitecode" placeholder="邀请码"></asp:TextBox>
                                </div>
                                <div class="aui-list-item-right" style="width:140px;"></div>
                            </div>
                        </li>
                    </ul>
                    
                    <div class="aui-content-padded">
                        <p>
                            <asp:LinkButton OnClientClick="loading()" runat="server" OnClick="Unnamed_Click" CssClass="aui-btn aui-btn-info aui-btn-block" Text="注册"></asp:LinkButton>
                        </p>
                    </div>
                    <div class="aui-content-padded aui-margin-t-15 aui-margin-b-15">
                        <p class="aui-text-center">
                            <br />
                            注册即同意<a href="protocol.aspx">《微特众包用户协议》</a>
                        </p>
                    </div>
                    <div class="aui-content-padded aui-margin-t-15 aui-margin-b-15">
                        <p class="aui-text-center">
                            <br />
                            收不到短信？使用<asp:LinkButton runat="server" OnClientClick="return voice('语音验证码')" Text="" ID="voice" OnCommand="sms_Command" CommandArgument="2"></asp:LinkButton>
                            <br /><br />
                        </p>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </form>
    <script>
        var dialog = new auiDialog({});
        var toast = new auiToast({});
        var tick;

        $(document).ready(function () {
            if (localStorage.tick != undefined) {
                time(localStorage.tick)
            } else {
                $("#<%= sms.ClientID%>").html("发送验证码");
                $("#<%= voice.ClientID%>").html("语音验证码");
            }
        })

        function loading() {
            toast.loading({
                title: "努力处理中",
                duration: 2000
            }, function (ret) {
            });
        }
        function send(str) {
            var title = $("#<%= sms.ClientID%>").html();
            if (title != str) {
                return false;
            }
        }
        function voice(str) {
            var title = $("#<%= voice.ClientID%>").html();
            if (title != str) {
                return false;
            }
        }
        function time(tick) {
            tick = localStorage.tick;
            if (tick > 0) {
                tick--;
                localStorage.tick = tick;
                $("#<%= sms.ClientID%>").html(tick + "s");
                $("#<%= voice.ClientID%>").html(tick + "s");
                setTimeout(function () {
                    time(tick)
                }, 1000)
            } else {
                $("#<%= sms.ClientID%>").html("发送验证码");
                $("#<%= voice.ClientID%>").html("语音验证码");
            }
        }

        function smsSuccess() {
            localStorage.tick = 60;
            time(60);
            toast.success({
                title: "发送成功",
                duration: 2000
            });
        }

        function smsFail(msg) {
            toast.fail({
                title: msg,
                duration: 2000
            });
        }

        function hide() {
            toast.hide();
        }
        function success() {
            dialog.alert({
                title: "温馨提示",
                msg: '注册成功',
                buttons: ['确定']
            }, function (ret) {
                location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=cn.waitter.user";
            })
        }
        function fail(msg) {
            $("#<%= sms.ClientID%>").html("发送验证码");
            $("#<%= voice.ClientID%>").html("语音验证码");
            dialog.alert({
                title: "温馨提示",
                msg: msg,
                buttons: ['确定']
            }, function (ret) {
                //location.href = "http://a.app.qq.com/o/simple.jsp?pkgname=cn.waitter.user";
            })
        }
    </script>
</body>
</html>
