<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OperLog.aspx.cs" Inherits="Authorization.Web.sys.Log.OperLog" %>

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
                    SortField="Id" SortDirection="DESC" DataKeyNames="Id">
                    <Columns>
                        <f:RowNumberField />
                        <f:BoundField DataField="LogTime" HeaderText="操作时间" SortField="LogTime"
                            Width="150px" DataFormatString="{0:yyyy-MM-dd HH:mm:ss}" />
                        <f:BoundField DataField="OperType" HeaderText="操作类型" SortField="OperType"
                            Width="80px" />
                         <f:BoundField DataField="ModuleName" HeaderText="模块名称" SortField="ModuleName"
                            Width="150px" />
                        <f:BoundField DataField="OperContent" HeaderText="操作内容" SortField="OperContent"
                            ExpandUnusedSpace="true" />
                        <f:BoundField DataField="RealName" HeaderText="操作人" SortField="RealName"
                            Width="150px" />
                        <f:BoundField DataField="Ip" HeaderText="操作IP" SortField="Ip"
                            Width="150px" />
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
    </form>
</body>
</html>
