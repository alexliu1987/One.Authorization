<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Test.aspx.cs" Inherits="Authorization.Web.Test" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel8" />
        <f:Panel ID="Panel8" ShowBorder="false" ShowHeader="false" BoxFlex="1" Layout="Fit"
            runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid ID="Grid1" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" ShowHeader="false" IsDatabasePaging="true" AllowPaging="true" AllowSorting="true" OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort"
                    SortField="Id" SortDirection="ASC" DataKeyNames="Id">
                    <Columns>
                        <f:RowNumberField />
                        <f:BoundField DataField="OperType" HeaderText="操作类型" SortField="OperType"
                            Width="150px" />
                        <f:BoundField DataField="OperContent" HeaderText="内容" SortField="OperContent"
                            Width="120px" />

                        <f:BoundField DataField="RealName" HeaderText="用户名" SortField="RealName"
                            Width="150px" />

                        <f:BoundField DataField="LoginTime" HeaderText="时间" Width="150px" SortField="LoginTime" />

                    </Columns>
                    <PageItems>
                        <f:ToolbarSeparator ID="ToolbarSeparator3" runat="server">
                        </f:ToolbarSeparator>
                        <f:ToolbarText ID="ToolbarText1" runat="server" Text="每页记录数：">
                        </f:ToolbarText>
                        <f:DropDownList ID="ddlGridPageSize" Width="80px" AutoPostBack="true" OnSelectedIndexChanged="ddlGridPageSize_SelectedIndexChanged"
                            runat="server">
                            <f:ListItem Text="10" Value="10" />
                            <f:ListItem Text="20" Value="20" />
                            <f:ListItem Text="50" Value="50" />
                            <f:ListItem Text="100" Value="100" />
                        </f:DropDownList>
                    </PageItems>
                </f:Grid>
            </Items>
        </f:Panel>
        <f:Window ID="Window2" Width="200px" Height="120px" Icon="TagBlue" Title="窗体" Hidden="true" Target="Parent"   BodyPadding="6px"
            EnableMaximize="true" EnableCollapse="true" runat="server" EnableResize="true" CloseAction="HideRefresh"
            IsModal="true" Layout="Fit">
            <Toolbars>
                <f:Toolbar runat="server" Position="Bottom">
                    <Items>
                        <f:Button ID="btnDown" Text="下载" runat="server" Icon="Add" Enabled="false" OnClick="btnDown_Click" EnableAjax="false"   DisableControlBeforePostBack="false">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Label ID="lbMsg" runat="server" Text="数据导出准备中……"></f:Label>
            </Items>
        </f:Window>
    </form>
</body>
</html>
