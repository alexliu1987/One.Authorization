<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OrgnizationTree.aspx.cs" Inherits="Authorization.Web.sys.Orgnization.OrgnizationTree" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" runat="server" AutoSizePanelID="Panel1" />
        <f:Panel ID="Panel1" ShowBorder="false" ShowHeader="false" AutoScroll="true" runat="server">
            <Toolbars>
                <f:Toolbar ID="Toolbar1" runat="server" Position="Footer">
                    <Items>
                        <f:ToolbarFill ID="ToolbarFill1" runat="server"></f:ToolbarFill>

                        <f:Button ID="btnOK" Icon="Tick" OnClick="btnOK_Click" runat="server" Text="确定">
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
                <f:Tree runat="server" ShowBorder="false" ShowHeader="false"
                    ID="trOrgnizationTree" AutoScroll="true" EnableArrows="true">
                </f:Tree>
            </Items>
        </f:Panel>
    </form>
</body>
</html>
