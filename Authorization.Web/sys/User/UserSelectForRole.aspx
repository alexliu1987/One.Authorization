<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserSelectForRole.aspx.cs" Inherits="Authorization.Web.sys.User.UserSelectForRole" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>选择用户</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                    <Items>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>
                        <f:Button ID="btnOK" Icon="Tick" OnClick="btnOK_Click" runat="server" Text="确定">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnClose" Icon="ArrowUndo"
                            runat="server" Text="返回">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Regions>
                <f:Region ID="Region1" EnableCollapse="true" Title="组织列表" ShowBorder="false" Split="true"
                    Width="200px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Tree runat="server" ShowBorder="false" ShowHeader="false" OnNodeCommand="trOrgnization_NodeCommand"
                            ID="trOrgnization" AutoScroll="true" EnableArrows="true">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Position="Center" Title="用户列表" ShowBorder="false" ShowHeader="false"
                    Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid1" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" ShowHeader="false" IsDatabasePaging="true" AllowPaging="true" AllowSorting="true" OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort"
                            SortField="id" SortDirection="DESC" DataKeyNames="id" PageSize="8">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar2" runat="server">
                                    <Items>
                                        <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                        <f:TwinTriggerBox ID="ttbSearch" runat="server" EmptyText="请输入用户名或姓名" Trigger1Icon="Clear" ShowTrigger1="false" Width="200" Trigger2Icon="Search" OnTrigger1Click="ttbSearch_Trigger1Click" OnTrigger2Click="ttbSearch_Trigger2Click"></f:TwinTriggerBox>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="UserName" HeaderText="用户名" SortField="UserName"
                                    Width="100px" />
                                <f:BoundField DataField="RealName" HeaderText="姓名" SortField="RealName"
                                    Width="100px" />
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
                </f:Region>
            </Regions>
        </f:RegionPanel>
    </form>
</body>
</html>
