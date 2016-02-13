<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppList.aspx.cs" Inherits="Authorization.Web.sys.App.AppList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>功能管理</title>
  
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel8" />
        <f:Panel ID="Panel8" ShowBorder="false" ShowHeader="false" BoxFlex="1" Layout="Fit"
            runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar2" runat="server">
                    <Items>
                        <%--<f:Button ID="btnAdd" Text="新增" runat="server" Icon="Add">
                        </f:Button>
                        <f:ToolbarSeparator runat="server" ID="ts1"></f:ToolbarSeparator>
                        <f:Button ID="btnEdit" Text="编辑" runat="server" Icon="BulletEdit" OnClick="btnEdit_Click">
                        </f:Button>
                        <f:ToolbarSeparator runat="server" ID="ToolbarSeparator1"></f:ToolbarSeparator>
                        <f:Button ID="btnDelete" Text="删除" runat="server" Icon="Delete" OnClick="btnDelete_Click">
                        </f:Button>
                        <f:ToolbarFill runat="server" ID="tf1"></f:ToolbarFill>
                        <f:TwinTriggerBox ID="ttbSearch" ShowLabel="false" OnTrigger1Click="ttbSearch_Trigger1Click" OnTrigger2Click="ttbSearch_Trigger2Click" Width="200"
                            Trigger1Icon="Clear" ShowTrigger1="False" EmptyText="请输入功能名称" Trigger2Icon="Search"
                            runat="server">
                        </f:TwinTriggerBox>--%>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:Grid ID="Grid1" runat="server" EnableCheckBoxSelect="true" ShowBorder="false" ShowHeader="false" IsDatabasePaging="true" AllowPaging="true" AllowSorting="true" OnPageIndexChange="Grid1_PageIndexChange" OnSort="Grid1_Sort"
                    SortField="SortIndex" SortDirection="ASC" DataKeyNames="Id">
                    <Columns>
                        <f:RowNumberField />
                        <f:BoundField DataField="AppName" HeaderText="模块名称" SortField="ActionName"
                            Width="150px" />
                        <f:BoundField DataField="AppCode" HeaderText="模块标识" SortField="EnName"
                            Width="120px" />
                        <f:ImageField  DataImageUrlField="Ico" HeaderText="图标" Width="50px" />
                        <f:CheckBoxField Width="80px" TextAlign="Center" RenderAsStaticField="true" DataField="IsUsing" HeaderText="是否可用" SortField="IsUsing" />
                        <f:BoundField DataField="SortIndex" HeaderText="排序" Width="50px" SortField="SortIndex" />
                        <f:BoundField DataField="Describe" HeaderText="描述" ExpandUnusedSpace="true" SortField="Describe" />
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
        <f:Window ID="Window1" Title="" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window1_Close"
            IsModal="true" Width="450px" EnableConfirmOnClose="true" Height="380px">
        </f:Window>
    </form>
</body>
</html>
