<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuList.aspx.cs" Inherits="Authorization.Web.sys.Menu.MenuList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
        <title>菜单管理</title>
    </head>
    <body>
        <form id="form1" runat="server">
            <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
            <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
                <Regions>
                    <f:Region ID="Region1" EnableCollapse="true" Title="模块列表" ShowBorder="false" Split="true"
                              Width="260px" Position="Left" Layout="Fit"
                              runat="server">
                        <Items>
                            <f:Tree runat="server" ShowBorder="false" ShowHeader="false" OnNodeCommand="trApp_NodeCommand"
                                    ID="trApp" AutoScroll="true" EnableArrows="true">
                            </f:Tree>
                        </Items>
                    </f:Region>
                    <f:Region ID="Region2" Position="Center" Title="菜单列表" ShowBorder="false"
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
                                    <f:Grid ID="Grid1" runat="server" ShowBorder="false" ShowHeader="false" EnableCheckBoxSelect="true"
                                            DataKeyNames="Mid">
                                        <Columns>
                                            <f:BoundField DataField="MenuName" HeaderText="菜单标题" DataSimulateTreeLevelField="TreeLevel"
                                                          Width="150px" />
                                            <f:BoundField DataField="MenuCode" HeaderText="菜单标识" Width="150px" />
                                            <f:ImageField Width="50px" DataImageUrlField="Ico" DataImageUrlFormatString="{0}" TextAlign="Center"
                                                          HeaderText="图标"></f:ImageField>
                                            <f:BoundField DataField="Url" HeaderText="链接" Width="300px" />
                                            <f:CheckBoxField Width="80px" TextAlign="Center" RenderAsStaticField="true" DataField="IsUsing" HeaderText="是否可用" />
                                            <f:CheckBoxField Width="80px" TextAlign="Center" RenderAsStaticField="true" DataField="IsOperRes" HeaderText="有无范围" />
                                            <f:BoundField DataField="SortIndex" HeaderText="排序" Width="50px" TextAlign="Center" />
                                            <f:BoundField DataField="ActionList" HeaderText="功能列表" ExpandUnusedSpace="true" />
                                            <f:BoundField DataField="Describe" HeaderText="描述" Width="150px" />
                                        </Columns>
                                    </f:Grid>
                                </Items>
                            </f:Panel>
                        </Items>
                    </f:Region>
                </Regions>
            </f:RegionPanel>
            <f:Window ID="Window1" Title="" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
                      EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window1_Close"
                      IsModal="true" Width="650px" EnableConfirmOnClose="true" Height="600px">
            </f:Window>
        </form>
    </body>
</html>