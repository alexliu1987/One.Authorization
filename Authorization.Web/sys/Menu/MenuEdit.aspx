<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MenuEdit.aspx.cs" Inherits="Authorization.Web.sys.Menu.MenuEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server" BodyPadding="20px">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                    <Items>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>

                        <f:Button ID="btnSave" ValidateForms="SimpleForm1" Icon="Disk" OnClick="btnSave_Click"
                            runat="server" Text="保存">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnClose" Icon="Cancel"
                            runat="server" Text="关闭">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>

                <f:GroupPanel runat="server" Title="基础信息" ID="GroupPanel1" EnableCollapse="True">
                    <Items>
                        <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                            BodyPadding="10px" Title="SimpleForm">
                            <Items>
                                <f:TextBox ID="tbxMenuName" runat="server" Label="菜单名称" Required="true" ShowRedStar="true" TabIndex="1">
                                </f:TextBox>
                                <f:TextBox ID="tbxMenuCode" runat="server" Label="菜单标识" Required="true" ShowRedStar="true" TabIndex="1">
                                </f:TextBox>
                                <f:TriggerBox ID="tbApp" EnableEdit="false" Text="" Required="true" ShowRedStar="true" TabIndex="4" EmptyText="请选择所属模块" TriggerIcon="Search" Label="所属模块"
                                    runat="server">
                                </f:TriggerBox>
                                <f:TriggerBox ID="tbPMName" EnableEdit="false" Text="" EnablePostBack="true"
                                    Required="true" ShowRedStar="true" TabIndex="2" EmptyText="请选择上级菜单" TriggerIcon="Search" Label="上级菜单"
                                    runat="server">
                                </f:TriggerBox>
                                <f:NumberBox ID="nbSortIndex" Label="排序" Required="true" Text="1" ShowRedStar="true" runat="server" TabIndex="3">
                                </f:NumberBox>
                                <f:TextBox ID="tbxUrl" runat="server" Label="菜单链接">
                                </f:TextBox>

                                <f:Panel ID="Panel2" ShowHeader="false" CssClass="x-form-item" ShowBorder="false"
                                    Layout="Column" runat="server">
                                    <Items>

                                        <f:Image ID="imgMenuIcon" runat="server" ImageUrl="~/icon/page.png" Label="菜单图标">
                                        </f:Image>
                                        <f:Button ID="btnSelectIco" Text="选取图标" runat="server" Icon="ImageMagnify" CssStyle="margin-left:20px;">
                                        </f:Button>

                                    </Items>
                                </f:Panel>
                                <f:CheckBox ID="cbxIsOperRes" runat="server" Label="有无操作范围"></f:CheckBox>
                                <f:CheckBox ID="cbxIsUsing" Checked="true" runat="server" Label="是否可用"></f:CheckBox>
                                <f:TextArea ID="taDescribe" runat="server" Label="描述">
                                </f:TextArea>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel runat="server" Title="功能操作" ID="GroupPanel2" EnableCollapse="True">
                    <Items>
                        <f:CheckBoxList ID="cblAction" runat="server" ColumnNumber="5"></f:CheckBoxList>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:HiddenField ID="hf_PMid" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hf_Appid" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hf_MenuIcon" runat="server" Text="~/icon/page.png">
        </f:HiddenField>
        <f:Window ID="Window3" Title="选择所属模块" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
        <f:Window ID="Window1" Title="选择上级菜单" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
        <f:Window ID="Window2" Title="选择图标" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window2_Close"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
    </form>
</body>
</html>
