<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="DictionaryList.aspx.cs" Inherits="Authorization.Web.Dictionary.DictionaryList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>字典管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" EnableCollapse="true" Title="字典类型" ShowBorder="false" Split="true"
                    Width="260px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Grid ID="Grid2" runat="server" ShowBorder="false" PageSize="1000" ShowHeader="false" AllowSorting="true" OnSort="Grid2_Sort" OnRowClick="Grid2_RowClick" EnableRowClickEvent="true"
                            SortField="SortIndex" SortDirection="DESC" DataKeyNames="Id">
                            <Columns>
                                <f:RowNumberField />
                                <f:BoundField DataField="TypeName" HeaderText="名称" SortField="TypeName"
                                    Width="200px" />
                            </Columns>
                        </f:Grid>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Position="Center" Title="字典值" ShowBorder="false"
                    Layout="Fit"
                    runat="server">
                    <Items>
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
                                    SortField="ID" SortDirection="DESC" DataKeyNames="Id,sys_DictionaryType_Id">
                                    <Columns>
                                        <f:RowNumberField />
                                        <f:BoundField DataField="Value" HeaderText="值" SortField="Value"
                                            Width="200px" />
                                        <f:CheckBoxField Width="80px" TextAlign="Center" RenderAsStaticField="true" DataField="IsUsing" HeaderText="是否可用" SortField="ISUSING" />
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
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Title="" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window1_Close"
            IsModal="true" Width="450px" EnableConfirmOnClose="true" Height="300px">
        </f:Window>
    </form>
</body>
</html>
