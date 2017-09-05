<%@ Page Title="文章编辑" ValidateRequest="false" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="articleedit.aspx.cs" Inherits="waitter.admin.articleedit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="admin1" runat="server">
     <asp:UpdatePanel runat="server" ID="up1">
        <ContentTemplate>
            <div class="container aui-padded-15">
                标题：<br />1
                <asp:TextBox CssClass="form-control" runat="server" ID="title"></asp:TextBox>
                <br />
                正文：
        <asp:TextBox ID="content"
            name="content"
            runat="server"
            TextMode="MultiLine" Height="400px"
            ClientIDMode="Static"></asp:TextBox>
                <br />
                <asp:LinkButton OnClick="Unnamed_Click" CssClass="btn btn-success" runat="server" Text="保存"></asp:LinkButton>
            </div>
   
        </ContentTemplate>
    </asp:UpdatePanel>

    <!-- 配置文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.config.js"></script>
    <!-- 编辑器源码文件 -->
    <script type="text/javascript" src="/ueditor/ueditor.all.js"></script>
    <!-- 实例化编辑器 -->
    <script type="text/javascript">
        var dialog = new auiDialog({});
        var toast = new auiToast({});
        var tick;
        var ue = UE.getEditor('content');
        function success() {
            var ue = UE.getEditor('content');
            dialog.alert({
                title: "",
                msg: '保存成功',
                buttons: ['确定']
            }, function (ret) {
                location.href = "article.aspx";
            })
        }
    </script>
</asp:Content>
