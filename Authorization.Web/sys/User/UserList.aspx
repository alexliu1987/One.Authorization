<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserList.aspx.cs" Inherits="Authorization.Web.sys.User.UserList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>用户管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" EnableCollapse="true" Title="组织列表" ShowBorder="false" Split="true"
                    Width="260px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Tree runat="server" ShowBorder="false" ShowHeader="false" OnNodeCommand="trOrgnization_NodeCommand"
                            ID="trOrgnization" AutoScroll="true" EnableArrows="true">
                        </f:Tree>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Position="Center" Title="用户列表" ShowBorder="false"
                    Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Panel ID="Panel8" ShowBorder="false" ShowHeader="false" BoxFlex="1" Layout="Fit"
                            runat="server">
                            <Toolbars>
                                <f:Toolbar ID="Toolbar1" runat="server">
                                    <Items>
                                    </Items>
                                </f:Toolbar>
                            </Toolbars>
                            <Items>
                                <f:Grid ID="Grid1" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" ShowHeader="false" IsDatabasePaging="true" AllowPaging="true" AllowSorting="true" OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort"
                                    SortField="id" SortDirection="DESC" DataKeyNames="id">
                                    <Columns>
                                        <f:RowNumberField />
                                        <f:BoundField DataField="UserName" HeaderText="用户名" SortField="UserName"
                                            Width="100px" />
                                        <f:BoundField DataField="RealName" HeaderText="姓名" SortField="RealName"
                                            Width="100px" />
                                        <f:CheckBoxField Width="80px" TextAlign="Center" RenderAsStaticField="true" DataField="IsUsing" HeaderText="是否可用" />
                                        <f:BoundField DataField="Email" HeaderText="邮箱" SortField="Email" ExpandUnusedSpace="true" />
                                        <f:BoundField DataField="sys_Orgnization.OrgnizationName" HeaderText="所属部门" SortField="sys_Orgnization.OrgnizationName"
                                            Width="150px" />
                                        <f:BoundField DataField="sys_MemberExtend.LastLoginTime" HeaderText="最后登录时间" Width="150" SortField="sys_MemberExtend.LastLoginTime" />
                                        <f:BoundField DataField="sys_MemberExtend.LastLoginIP" HeaderText="最后登录IP" Width="150" SortField="sys_MemberExtend.LastLoginIP" />
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
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Title="" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window1_Close"
            IsModal="true" Width="650px" EnableConfirmOnClose="true" Height="500px">
        </f:Window>
    </form>
</body>
</html>
