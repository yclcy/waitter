<%@ Page Title="" ValidateRequest="false" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="banner.aspx.cs" Inherits="waitter.admin.banner" %>

<asp:Content ID="Content1" ContentPlaceHolderID="admin1" runat="server">
    <table>
        <tr>
            <td>类型<br />
                <asp:DropDownList runat="server" ID="type">
                    <asp:ListItem Text="页面" Value="0"></asp:ListItem>
                    <asp:ListItem Text="活动与通知" Value="1"></asp:ListItem>
                </asp:DropDownList>
                <hr />
                当前活动<br />
                <asp:DropDownList OnSelectedIndexChanged="notice_SelectedIndexChanged" AutoPostBack="true" runat="server" ID="notice">
                    <asp:ListItem Text="选择活动与通知" Value="-1"></asp:ListItem>
                </asp:DropDownList>
                <hr />
                APP页面<br />
                <asp:DropDownList ID="page" runat="server" AutoPostBack="true" OnSelectedIndexChanged="page_SelectedIndexChanged">
                    <asp:ListItem Text="选择页面" Value="-1"></asp:ListItem>
                    <asp:ListItem Text="我的钱包" Value="wallet"></asp:ListItem>
                    <asp:ListItem Text="推荐有奖" Value="invite"></asp:ListItem>
                    <asp:ListItem Text="实名认证" Value="idcard"></asp:ListItem>
                </asp:DropDownList>
                <hr />
                BANNER位置<br />
                <asp:DropDownList ID="position" runat="server">
                    <asp:ListItem Text="首页" Value="home"></asp:ListItem>
                    <asp:ListItem Text="推荐有奖" Value="invite"></asp:ListItem>
                </asp:DropDownList>
                <hr />
                目标(APP页面或者活动通知ID)<br />
                <asp:TextBox runat="server" ID="target"></asp:TextBox>类型为页面时可空
                <hr />
                Banner图片地址<br />
                <asp:TextBox runat="server" ID="imgurl"></asp:TextBox>
                <a href="javascript:;" onclick="upImage()">上传图片</a>
                <hr />
                <asp:LinkButton runat="server" ID="submit" OnClick="submit_Click" Text="提交"></asp:LinkButton>
                <asp:Label runat="server" ID="msg"></asp:Label>
            </td>
            <td>
                <asp:RadioButtonList runat="server" ID="bannerlist">

                </asp:RadioButtonList>
                <asp:LinkButton runat="server" ID="delete" OnClick="delete_Click" Text="删除"></asp:LinkButton>
            </td>
        </tr>
    </table>
    <!-- 加载编辑器的容器 -->
    <script id="container" style="height: 5px; display: none;" name="content" type="text/plain">
        这里写你的初始化内容
    </script>
    <!-- 配置文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.all.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var ue = UE.getEditor('container', {
            autoHeightEnabled: false
        });
        ue.ready(function () {
            ue.hide();//隐藏编辑器
            //监听图片上传
            ue.addListener('beforeInsertImage', function (t, arg) {
                $("#<%= imgurl.ClientID%>").val(arg[0].src);
            });

            /* 文件上传监听
             * 需要在ueditor.all.min.js文件中找到
             * d.execCommand("insertHtml",l)
             * 之后插入d.fireEvent('afterUpfile',b)
             */
            ue.addListener('afterUpfile', function (t, arg) {
                alert('这是文件地址：' + arg[0].url);
            });
        });

        //弹出图片上传的对话框
        function upImage() {
            var myImage = ue.getDialog("insertimage");
            myImage.open();
        }
    </script>
</asp:Content>
