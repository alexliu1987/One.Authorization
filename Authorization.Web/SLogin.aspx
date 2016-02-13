<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SLogin.aspx.cs" Inherits="Authorization.Web.SLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录-统一授权站点</title>
    <link href="App_Themes/css/main.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(function() {
            $("#ifEngineering").hide();
            $("#ifMaintenance").hide();
        });
    </script>
</head>
<body style="background-color: #0683b1;">
    <form id="form1" runat="server">
    <div class="logo">
    </div>
    <div class="login">
        <div class="login_info"></div>
        <div class="user">
            <asp:TextBox ID="tbxUserName" CssClass="LoginInputText" runat="server"></asp:TextBox>
        </div>
        <div class="pwd">
            <asp:TextBox ID="tbxPassWord" CssClass="LoginInputText" runat="server"></asp:TextBox>
        </div>
        <div class="btn_login_div">
            <asp:Button runat="server" ID="btnLogin" CssClass="btnLogin" OnClick="btnLogin_Click" />
        </div>
    </div>
    </form>
    <iframe runat="server" ID="ifEngineering" ClientIDMode="Static" src=""></iframe>
    <iframe runat="server" ID="ifMaintenance" ClientIDMode="Static" src=""></iframe>
</body>
</html>
