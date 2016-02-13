<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgnizationList.aspx.cs" Inherits="Authorization.Web.sys.Orgnization.OrgnizagionList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>组织管理</title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="RegionPanel1" />
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" ShowBorder="true" ShowHeader="false"
                    Width="200px" Position="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:Panel ID="Panel3" ShowBorder="false" Title="部门列表" Layout="Fit" AutoScroll="true" runat="server">
                            <Items>
                                <f:Tree runat="server" ShowBorder="false" ShowHeader="false" OnNodeCommand="trDepartmentTree_NodeCommand"
                                    ID="trDepartmentTree" AutoScroll="true" EnableArrows="true">
                                </f:Tree>
                            </Items>
                        </f:Panel>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" ShowBorder="true" ShowHeader="false" Position="Center"
                    BoxConfigAlign="Stretch" BoxConfigPosition="Left" Layout="Fit"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="TabStrip1" ShowBorder="false" TabPosition="Top" EnableFrame="false"
                            EnableTabCloseMenu="false" EnableTitleBackgroundColor="true" ActiveTabIndex="0"
                            runat="server">
                            <Tabs>
                                <f:Tab ID="Tab1" Title="添加" Layout="Fit"
                                    runat="server">
                                    <Items>
                                        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                                                    <Items>
                                                        <f:Button ID="btnSave" ValidateForms="SimpleForm1" Icon="Disk" OnClick="btnSave_Click"
                                                            runat="server" Text="保存">
                                                        </f:Button>
                                                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" Width="400" BodyPadding="20px"
                                                    Title="SimpleForm">
                                                    <Items>
                                                        <f:TextBox ID="tbxDName" runat="server" Label="部门名称" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TriggerBox ID="tbPDName" EnableEdit="false" Text="" EnablePostBack="true"
                                                            ShowRedStar="true" TabIndex="2" EmptyText="请选择上级部门" TriggerIcon="Search" Label="上级部门"
                                                            runat="server">
                                                        </f:TriggerBox>
                                                        <f:TextArea ID="tbxRemark" runat="server" Label="描述" TabIndex="3">
                                                        </f:TextArea>
                                                    </Items>
                                                </f:SimpleForm>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Tab>
                                <f:Tab ID="Tab2" Title="修改" runat="server" Layout="Fit">
                                    <Items>
                                        <f:Panel ID="Panel2" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
                                            <Toolbars>
                                                <f:Toolbar ID="Toolbar2" runat="server" Position="Footer">
                                                    <Items>
                                                        <f:Button ID="btnSave_Edit" ValidateForms="SimpleForm2" Icon="Disk" OnClick="btnSave_Edit_Click"
                                                            runat="server" Text="保存">
                                                        </f:Button>
                                                        <f:ToolbarSeparator ID="ToolbarSeparator2" runat="server">
                                                        </f:ToolbarSeparator>
                                                        <f:Button ID="btnDel" Icon="Delete" OnClick="btnDel_Click"
                                                            runat="server" Text="删除">
                                                        </f:Button>
                                                        <f:ToolbarFill ID="ToolbarFill2" runat="server"></f:ToolbarFill>
                                                    </Items>
                                                </f:Toolbar>
                                            </Toolbars>
                                            <Items>
                                                <f:SimpleForm ID="SimpleForm2" ShowBorder="false" ShowHeader="false" runat="server" Width="400"
                                                    BodyPadding="20px" Title="SimpleForm">
                                                    <Items>
                                                        <f:TextBox ID="tbxDName_Edit" runat="server" Label="部门名称" Required="true" ShowRedStar="true" TabIndex="1">
                                                        </f:TextBox>
                                                        <f:TriggerBox ID="tbPDName_Edit" EnableEdit="false" Text="" EnablePostBack="true"
                                                            Required="true" ShowRedStar="true" TabIndex="2" EmptyText="请选择上级部门" TriggerIcon="Search" Label="上级部门"
                                                            runat="server">
                                                        </f:TriggerBox>
                                                        <f:TextArea ID="taRemark_Edit" runat="server" Label="描述" TabIndex="3">
                                                        </f:TextArea>
                                                    </Items>
                                                </f:SimpleForm>
                                            </Items>
                                        </f:Panel>
                                    </Items>
                                </f:Tab>
                            </Tabs>
                        </f:TabStrip>
                    </Items>
                </f:Region>
            </Regions>
        </f:RegionPanel>
        <f:Window ID="Window1" Title="选择上级部门" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
        <f:HiddenField ID="hf_PDid" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hf_PDid_Edit" runat="server">
        </f:HiddenField>
        <f:HiddenField ID="hf_Did_Edit" runat="server">
        </f:HiddenField>
    </form>
</body>
</html>
