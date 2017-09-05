<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="verify.aspx.cs" Inherits="waitter.admin.verify" %>
<asp:Content ID="Content1" ContentPlaceHolderID="admin1" runat="server">
    <asp:TextBox runat="server" placeholder="请输入用户的手机号查询" ID="query"></asp:TextBox>
    <asp:LinkButton runat="server" ID="submit" OnClick="submit_Click"  Text="查询"></asp:LinkButton>
    <hr />
    <asp:PlaceHolder ID="ph1" runat="server" Visible="false">
        <asp:Label ID="userinfo" runat="server"></asp:Label>
        <hr />
        <asp:CheckBox runat="server" Text="身份证审核通过" ID="verified" />
        <asp:CheckBox runat="server" Text="已培训" ID="study" />
        <asp:CheckBox runat="server" Text="健康证审核通过" ID="jiankang" />
        <hr />
        <asp:LinkButton runat="server" Text="提交" ID="confirm" OnClick="confirm_Click"></asp:LinkButton>
        <hr />
        <asp:Label runat="server" ID="msg"></asp:Label>
    </asp:PlaceHolder>
    <asp:HiddenField ID="userid" runat="server" />
</asp:Content>
