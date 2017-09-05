<%@ Page Title="岗位培训管理" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="training.aspx.cs" Inherits="waitter.admin.training" %>
<asp:Content runat="server" ContentPlaceHolderID="admin1">
    <asp:Label runat="server">标题</asp:Label>
    <asp:TextBox runat="server" ID="title"></asp:TextBox>
    <asp:Label runat="server">内容</asp:Label>
    <asp:TextBox runat="server" ID="content" TextMode="MultiLine"></asp:TextBox>
    <asp:LinkButton runat="server" ID="submit" OnClick="submit_Click" Text="提交"></asp:LinkButton>
</asp:Content>