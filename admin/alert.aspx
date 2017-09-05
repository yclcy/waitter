<%@ Page Title="" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="alert.aspx.cs" Inherits="waitter.admin.alert" %>

<asp:Content ID="Content1" ContentPlaceHolderID="admin1" runat="server">
    类型<br />
    <asp:RadioButtonList runat="server" RepeatDirection="Horizontal" ID="type">
        <asp:ListItem Text="通知" Value="1" Selected="True"></asp:ListItem>
        <asp:ListItem Text="活动" Value="2"></asp:ListItem>
    </asp:RadioButtonList>
    <hr />
    <asp:CheckBox runat="server" ID="isnew" Text="新" />
    <asp:CheckBox runat="server" ID="ishot" Text="热" />
    <asp:CheckBox runat="server" ID="recommend" Text="推荐" />
    <hr />
    <asp:Label ID="Label1" runat="server" Text="标题"></asp:Label>
    <asp:TextBox ID="title" runat="server"></asp:TextBox>
    <hr />
    <asp:Label runat="server" Text="描述"></asp:Label>
    <asp:TextBox ID="desc" TextMode="MultiLine" Height="100" runat="server"></asp:TextBox>
    <hr />
    <asp:Label ID="Label2" runat="server" Text="内容"></asp:Label>
    <asp:TextBox ID="content" TextMode="MultiLine" Height="100" runat="server"></asp:TextBox>
    <hr />
    <asp:LinkButton ID="submit" OnClick="submit_Click" runat="server">确定</asp:LinkButton>
    <asp:Label ID="result_label" runat="server"></asp:Label>
</asp:Content>
