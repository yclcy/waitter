<%@ Page Title="文章" Language="C#" MasterPageFile="~/admin/admin.master" AutoEventWireup="true" CodeBehind="article.aspx.cs" Inherits="waitter.admin.article" %>

<asp:Content ID="Content1" ContentPlaceHolderID="admin1" runat="server">
    <div class="container aui-padded-15">
        <div class="btn-group" role="group" aria-label="...">
            <a type="button" href="articlecreate.aspx" class="btn btn-default">新建文章</a>
        </div>

        <asp:DataGrid AutoGenerateColumns="false" CssClass="table table-bordered table-striped aui-margin-t-15" runat="server" ID="dv1">
            <Columns>
                <asp:TemplateColumn HeaderText="状态">
                    <ItemTemplate>
                        <div class="btn-group btn-group-xs">
                            <asp:LinkButton OnCommand="Unnamed_Command" CommandArgument='<%# Eval("id") %>' runat="server" CommandName='<%# Convert.ToBoolean(Eval("show"))==true?"false":"true" %>' data-toggle="tooltip" data-placement="right" title='<%# Convert.ToBoolean(Eval("show"))==true?"取消发布":"发布" %>' CssClass='<%# Convert.ToBoolean(Eval("show"))==true?"btn btn-default glyphicon glyphicon-ok aui-text-success":"btn btn-default glyphicon glyphicon-remove aui-text-danger" %>'></asp:LinkButton>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:TemplateColumn HeaderText="文章标题">
                    <ItemTemplate>
                        <div class="btn-group btn-group-xs">
                            <a href='articleedit.aspx?id=<%# Eval("id") %>'><%# Eval("title") %></a>
                        </div>
                    </ItemTemplate>
                </asp:TemplateColumn>
                <asp:BoundColumn DataField="category" HeaderText="分类"></asp:BoundColumn>
            </Columns>
        </asp:DataGrid>
        <asp:Label runat="server" ID="rs"></asp:Label>
    </div>
</asp:Content>
