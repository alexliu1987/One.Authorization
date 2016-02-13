<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ActionEdit.aspx.cs" Inherits="Authorization.Web.sys.Action.ActionEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        .icoFrom {
            margin-bottom: 5px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="false" runat="server">
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
                <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server"
                    BodyPadding="10px" Title="SimpleForm">
                    <Items>
                        <f:TriggerBox ID="tbApp" EnableEdit="false" Text="" Required="true" ShowRedStar="true" TabIndex="4" EmptyText="请选择所属模块" TriggerIcon="Search" Label="所属模块"
                            runat="server">
                        </f:TriggerBox>
                        <f:DropDownList ID="ddlActionType" runat="server" Label="按钮类型" Required="true" ShowRedStar="true" TabIndex="0"></f:DropDownList>
                        <f:TextBox ID="tbxAction" runat="server" Label="功能名称" Required="true" ShowRedStar="true" TabIndex="1">
                        </f:TextBox>
                        <f:TextBox ID="tbxActionCode" runat="server" Label="功能标识" Required="true" ShowRedStar="true" TabIndex="1">
                        </f:TextBox>
                        <f:Panel ID="Panel2" ShowHeader="false" CssClass="icoFrom" ShowBorder="false"
                            Layout="Column" runat="server">
                            <Items>
                                <f:Image ID="imgIcon" runat="server" ImageUrl="~/icon/page.png" Label="菜单图标">
                                </f:Image>
                                <f:Button ID="btnSelectIco" Text="选取图标" runat="server" Icon="ImageMagnify" CssStyle="margin-left:20px;">
                                </f:Button>
                            </Items>
                        </f:Panel>
                        <f:NumberBox ID="nbSortIndex" Label="排序" Required="true" Text="1" ShowRedStar="true" runat="server" TabIndex="2" Regex="^-?[1-9]\d*$" RegexMessage="只能输入整数">
                        </f:NumberBox>
                        <f:CheckBox ID="cbxIsUsing" Checked="true" runat="server" Label="是否可用" TabIndex="3"></f:CheckBox>
                        <f:TextArea ID="taDescribe" runat="server" Label="描述" TabIndex="4">
                        </f:TextArea>
                    </Items>
                </f:SimpleForm>
            </Items>
        </f:Panel>
         <f:HiddenField ID="hf_Ico" runat="server" Text="~/icon/page.png">
        </f:HiddenField>
        <f:HiddenField ID="hf_Appid" runat="server">
        </f:HiddenField>
        <f:Window ID="Window3" Title="选择所属模块" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
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
