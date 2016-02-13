<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="Authorization.Web.Login" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>登录-统一授权站点</title>
    <link href="style/login.css" rel="stylesheet" />
    <script src="js/jquery-1.10.2.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $("#ifEngineering").hide();
            $("#ifMaintenance").hide();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="header">
                <div class="logo"></div>
            </div>
            <div class="body">
                <div class="main">
                    <div class="login_title"></div>
                    <div class="login_form">
                        <div class="form_title">
                            <img src="images/login/tit_login.png" title="" />
                        </div>
                        <div class="form_item">
                            <input type="text" class="input_user" maxlength="16" placeholder="用户名" runat="server" ID="txtUserName" />
                            <br />
                            <input type="password" class="input_pwd" maxlength="16" placeholder="密码" runat="server" ID="txtPassword" />
                            <br />
                            <input type="text" class="input_verify" maxlength="6" placeholder="验证码" runat="server" ID="txtVerify" />
                        </div>
                        <div class="form_verify">
                            <img id="imgCaptcha" runat="server" width="150" height="30" />
                            <a href="javascript:void(0);" ID="btnRefresh" runat="server" OnServerClick="btnRefresh_Click">看不清换一张</a>
                        </div>
                        <div class="form_button">
                            <input type="button" class="btnLogin" runat="server" OnServerClick="btnLogin_Click" />
                        </div>
                        <div class="form_getpwd">
                            <a href="javascript:void(0);">找回密码</a>
                        </div>
                        <hr class="form_split" />
                        <div class="form_item">
                            <span style="margin: 10px 0;">没有统一认证账户？</span>
                            <br style="margin-bottom: 12px;" />
                            <a href="javascript:void(0);" class="reg">现在申请！</a><input type="button" class="btnReg" />
                            <br style="margin-bottom: 32px;" />
                            <span style="float:left;width: 80px;text-align: center;">成员服务</span><span style="float:left;width: 181px;text-align: center;">隐私安全</span><span style="float:left;width: 80px;text-align: center;">关于我们</span>
                            <div style="clear: both;"></div>
                        </div>
                        <hr class="form_split" />
                    </div>
                </div>
            </div>
            <div class="footer"></div>
        </div>
    </form>
    <iframe runat="server" ID="ifEngineering" ClientIDMode="Static" src=""></iframe>
    <iframe runat="server" ID="ifMaintenance" ClientIDMode="Static" src=""></iframe>
</body>
</html>
