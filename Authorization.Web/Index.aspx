<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="Authorization.Web.Index" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>统一授权站点</title>
    <link href="App_Themes/css/default.css" rel="stylesheet" />
    <script type="text/JavaScript">
        function timer() {
            $.ajax({
                type: "get",
                dataType: "json",
                data: "clienttime=" + Math.random(),
                url: "Ajax/common.ashx?oper=ajaxGetServerTime",
                success: function (d) {
                    switch (d.result) {
                        case '0':
                            break;
                        case '1':
                            document.getElementById("currentTime").innerHTML = d.returnval + "&nbsp;&nbsp;&nbsp;&nbsp;";
                            break;
                    }
                }
            });
        };

        setInterval("timer();", 1000);
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <f:PageManager ID="PageManager1" AutoSizePanelID="RegionPanel1" runat="server"></f:PageManager>
        <f:RegionPanel ID="RegionPanel1" ShowBorder="false" runat="server">
            <Regions>
                <f:Region ID="Region1" Margins="0 0 0 0" ShowBorder="false" Height="50px" ShowHeader="false"
                    Position="Top" Layout="Fit" runat="server">
                    <Items>
                        <f:ContentPanel ShowBorder="false" ShowHeader="false" ID="ContentPanel1" CssClass="header" runat="server">
                            <table style="width: 100%; border: 0; margin: 0; border-collapse: collapse; border-spacing: 0;">
                                <tr>
                                    <td class="logo"></td>
                                    <td class="title">统一授权站点</td>
                                    <td></td>
                                    <td class="loginout">
                                        <asp:LinkButton ID="lbtnLoginOut" runat="server" Text="退出" OnClick="lbtnLoginOut_Click">退出</asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>
                <f:Region ID="Region2" Split="true" Width="200px" ShowHeader="true" Title="系统菜单"
                    EnableCollapse="true" Layout="Fit" Position="Left" runat="server">
                </f:Region>
                <f:Region ID="mainRegion" ShowHeader="false" Layout="Fit" Margins="0 0 0 0" Position="Center"
                    runat="server">
                    <Items>
                        <f:TabStrip ID="mainTabStrip" EnableTabCloseMenu="true" EnableFrame="false" ShowBorder="false" runat="server">
                            
                        </f:TabStrip>
                    </Items>
                </f:Region>
                <f:Region ID="Region3" Margins="0 0 0 0" ShowBorder="false" Height="30px" ShowHeader="false"
                    Position="Bottom" Layout="Fit" runat="server">
                    <Items>
                        <f:ContentPanel ShowBorder="false" ShowHeader="false" ID="ContentPanel2" CssClass="footer" runat="server">
                            <table style="width: 100%;">
                                <tr>
                                    <td style="width: 350px; padding-left: 10px;"><span>您好：<%=BaseRealName %>[<%=BaseUserName %>] 您的IP：<%=BaseIp %></span></td>
                                    <td>&nbsp</td>
                                    <td style="width: 280px;"><span id="currentTime">当前时间：时间载入中……</span></td>
                                    <td style="width: 100px; text-align: right; padding-right: 10px;">在线人数[<span style="color: red; font-weight: bold;"><%= Convert.ToInt32(Application["AuthorizationCOUNT"]) %></span>]</td>
                                </tr>
                            </table>
                        </f:ContentPanel>
                    </Items>
                </f:Region>

            </Regions>
        </f:RegionPanel>
    </form>
    <script src="js/default.js" type="text/javascript"></script>
    <script src="js/jquery-1.10.2.min.js" type="text/javascript"></script>
</body>
</html>
