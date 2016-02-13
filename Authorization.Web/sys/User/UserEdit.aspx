<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="UserEdit.aspx.cs" Inherits="Authorization.Web.sys.User.UserEdit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1"   />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="false" runat="server"  BodyPadding="20px">
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
                        <f:SimpleForm ID="SimpleForm1" ShowBorder="false" ShowHeader="false" runat="server" BodyPadding="10px"
                            Title="SimpleForm">
                            <Items>
                                <f:TextBox ID="tbxUserName" runat="server" Label="用户名" Required="true" ShowRedStar="true" TabIndex="1">
                                </f:TextBox>
                                <f:TextBox ID="tbxRealName" runat="server" Label="姓名" Required="true" ShowRedStar="true" TabIndex="2">
                                </f:TextBox>
                                <f:RadioButtonList ID="rblSex" runat="server" Label="性别" Required="true" Width="200" ShowRedStar="true" TabIndex="3">
                                    <f:RadioItem Value="True" Text="男" Selected="true" />
                                    <f:RadioItem Value="False" Text="女" />
                                </f:RadioButtonList>
                                <f:TriggerBox ID="tbOrgnization" EnableEdit="false" Text="" Required="true" ShowRedStar="true" TabIndex="4" EmptyText="请选择所属部门" TriggerIcon="Search" Label="所属部门"
                                    runat="server">
                                </f:TriggerBox>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:GroupPanel>
                <f:GroupPanel runat="server" Title="扩展信息" ID="GroupPanel2" EnableCollapse="True">
                    <Items>
                        <f:SimpleForm ID="SimpleForm2" ShowBorder="false" ShowHeader="false" runat="server"
                            BodyPadding="10px" Title="SimpleForm">
                            <Items>
                                <f:TextBox ID="tbxEmail" runat="server" Label="邮箱" TabIndex="5" RegexPattern="EMAIL" RegexMessage="请输入正确格式的邮箱">
                                </f:TextBox>
                                <f:TextBox ID="tbxTel" runat="server" Label="电话" TabIndex="6"  Regex="^((\(\d{3}\))|(\d{3}\-))?13[0-9]\d{8}|15[89]\d{8}|18\d{9}" RegexMessage="请输入正确格式的邮箱">
                                </f:TextBox>
                                <f:TextBox ID="tbxQQ" runat="server" Label="QQ" TabIndex="7"  Regex="^\d{5,10}$" RegexMessage="请输入正确定格式的QQ号码">
                                </f:TextBox>
                                <f:TextBox ID="tbxIdCard" runat="server" Label="身份证号" TabIndex="8" Regex="^(\d{15}$|^\d{18}$|^\d{17}(\d|X|x))$" RegexMessage="请输入正确格式的身份证">
                                </f:TextBox>
                                <f:DatePicker ID="dpBirthday" TabIndex="8" EnableEdit="false" Label="生日" Width="300" runat="server"></f:DatePicker>
                            </Items>
                        </f:SimpleForm>
                    </Items>
                </f:GroupPanel>
            </Items>
        </f:Panel>
        <f:HiddenField ID="hf_OrgnizaionId" runat="server">
        </f:HiddenField>
        <f:Window ID="Window1" Title="选择所属部门" Hidden="true" EnableIFrame="true" Icon="ApplicationForm"
            EnableMaximize="true" Target="Top" EnableResize="true" runat="server"
            IsModal="true" Width="400px" EnableConfirmOnClose="true" Height="400px">
        </f:Window>
    </form>
</body>
</html>
