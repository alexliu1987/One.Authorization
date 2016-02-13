<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorPage.aspx.cs" Inherits="Authorization.Web.ErrorPage" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <style>
        body {
            font-family: Microsoft Yahei,verdana;
            font-size: 14px;
            color: red;
        }

        .error1 {
            width: 260px;
            background: url(App_Themes/img/error1.gif) no-repeat right center;
            height: 78px;
        }

        .error {
            background: url(App_Themes/img/error.png) no-repeat left center;
            height: 78px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">

        <table style="width: 100%;">
            <tr>
                <td class="error1"></td>
                <td class="error"></td>
            </tr>
        </table>
        <table style="width: 100%;">
            <tr>
                <td style="width: 200px;"></td>
                <td>
                    <asp:Literal ID="lMsg" runat="server"></asp:Literal></td>
            </tr>
        </table>

    </form>
</body>
</html>
