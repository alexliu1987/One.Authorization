﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AppEdit.aspx.cs" Inherits="Authorization.Web.sys.App.AppEdit" %>

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
                        <f:TextBox ID="tbxAppName" runat="server" Label="模块名称" Required="true" ShowRedStar="true" TabIndex="1">
                        </f:TextBox>
                        <f:TextBox ID="tbxAppCode" runat="server" Label="模块标识" Required="true" ShowRedStar="true" TabIndex="1">
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
        <f:Window ID="Window2" Title="选择图标" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server" OnClose="Window2_Close"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
    </form>
</body>
</html>
