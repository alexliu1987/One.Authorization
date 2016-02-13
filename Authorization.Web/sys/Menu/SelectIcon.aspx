<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SelectIcon.aspx.cs" Inherits="Authorization.Web.sys.Menu.SelectIcon" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
   <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false"   AutoScroll="true" runat="server" >
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                    <Items>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>

                        <f:Button ID="btnOK" Icon="Tick" runat="server" Text="确定" OnClick="btnOK_Click">
                        </f:Button>
                        <f:ToolbarSeparator ID="ToolbarSeparator1" runat="server">
                        </f:ToolbarSeparator>
                        <f:Button ID="btnClose" Icon="ArrowUndo" OnClick="btnClose_Click"
                            runat="server" Text="返回">
                        </f:Button>
                    </Items>
                </f:Toolbar>
            </Toolbars>
            <Items>
                <f:RadioButtonList ID="rblMenuIcon" runat="server"  ColumnNumber="10" BoxConfigAlign="Center">                   
                </f:RadioButtonList>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
