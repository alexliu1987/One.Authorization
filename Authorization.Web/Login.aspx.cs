using System;
using System.Configuration;
using Authorization.Framework.DataBase;
using Authorization.Services;
using SSO.Common;
using SSO.Common.SSO;

namespace Authorization.Web
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
                return;
            InitCaptchaCode();

            ifEngineering.Src = ConfigHelper.GetAppSettingString("EngineeringLogoutUrl");
            ifMaintenance.Src = ConfigHelper.GetAppSettingString("MaintenanceLogoutUrl");

            SSORequest ssoRequest = new SSORequest();

            #region 验证 Post 过来的参数

            //--------------------------------
            // 请求注销
            if (!string.IsNullOrEmpty(Request["Logout"]))
            {
                Authentication.Logout();
                return;
            }
            //--------------------------------
            // 各独立站点标识
            if (string.IsNullOrEmpty(Request["AppCode"]))
            {
                return;
            }
            else
            {
                ssoRequest.AppCode = Request["AppCode"];
            }

            //--------------------------------
            // 时间戳
            if (string.IsNullOrEmpty(Request["TimeStamp"]))
            {
                return;
            }
            else
            {
                ssoRequest.TimeStamp = Request["TimeStamp"];
            }

            //--------------------------------
            // 各独立站点的访问地址
            if (string.IsNullOrEmpty(Request["AppUrl"]))
            {
                return;
            }
            else
            {
                ssoRequest.AppUrl = Request["AppUrl"];
            }

            //--------------------------------
            // 各独立站点的 Token
            if (string.IsNullOrEmpty(Request["Authenticator"]))
            {
                return;
            }
            else
            {
                ssoRequest.Authenticator = Request["Authenticator"];
            }

            ViewState["SSORequest"] = ssoRequest;

            #endregion
        }

        /// <summary>
        /// 初始化验证码
        /// </summary>
        private void InitCaptchaCode()
        {
            // 创建一个 6 位的随机数并保存在 Session 对象中
            Session["CaptchaImageText"] = GenerateRandomCode();
            imgCaptcha.Src = "Ajax/Captcha.ashx?w=150&h=30&t=" + DateTime.Now.Ticks;
        }

        /// <summary>
        /// 创建一个 6 位的随机数
        /// </summary>
        /// <returns></returns>
        private string GenerateRandomCode()
        {
            string s = String.Empty;
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                s += random.Next(10).ToString();
            }
            return s;
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            InitCaptchaCode();
        }

        //post请求
        private void Post(SSORequest ssoRequest)
        {
            PostService ps = new PostService();

            ps.Url = ConfigurationManager.AppSettings["SSOUrl"];

            ps.Add("IsLogin", "1");
            ps.Add("UserAccount", ssoRequest.UserAccount);
            ps.Add("AppCode", ssoRequest.AppCode);
            ps.Add("TimeStamp", ssoRequest.TimeStamp);
            ps.Add("AppUrl", ssoRequest.AppUrl);
            ps.Add("Authenticator", ssoRequest.Authenticator);

            ps.Post();
        }

        /// <summary>
        /// 验证登录账号和密码是否正确
        /// </summary>
        /// <param name="userName">登录账号</param>
        /// <param name="userPwd">登录密码</param>
        /// <returns></returns>
        private bool ValidateUserInfo(string userName, string userPwd)
        {
            var user = new MemberService().FirstOrDefault(u => u.UserName == userName);
            return user != null && user.Password == DESEncrypt.Encrypt(userPwd, userName);
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUserName.Value) || string.IsNullOrWhiteSpace(txtPassword.Value))
            {
                Page.RegisterClientScriptBlock("Add", "<script lanuage=\"javascript\">alert('用户名密码不能为空！');</script>");
                return;
            }
            else if (string.IsNullOrWhiteSpace(txtVerify.Value))
            {
                Page.RegisterClientScriptBlock("Add", "<script lanuage=\"javascript\">alert('验证码不能为空！');</script>");
                return;
            }
            else if (txtVerify.Value.Trim() != Session["CaptchaImageText"].ToString())
            {
                Page.RegisterClientScriptBlock("Add", "<script lanuage=\"javascript\">alert('验证码不正确！');</script>");
                return;
            }
            else if (ValidateUserInfo(txtUserName.Value.Trim(), txtPassword.Value.Trim()) == false)
            {
                Page.RegisterClientScriptBlock("Add", "<script lanuage=\"javascript\">alert('用户名密码错误！');</script>");
                return;
            }
            else
            {
                Session["CurrUserName"] = txtUserName.Value.Trim();
                Session.Timeout = 120;

                SSORequest ssoRequest = ViewState["SSORequest"] as SSORequest;
                SSORequest ssoRequestRet = new SSORequest();

                // 如果不是从各分站 Post 过来的请求，则默认登录主站
                if (ssoRequest == null)
                {
                    //主站标识ID
                    ssoRequestRet.AppCode = ConfigurationManager.AppSettings["SystemCode"];
                    ssoRequestRet.AppUrl = ConfigurationManager.AppSettings["MainUrl"];
                }
                else
                {
                    ssoRequestRet.AppCode = ssoRequest.AppCode;
                    ssoRequestRet.AppUrl = ssoRequest.AppUrl;
                }
                ssoRequestRet.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                ssoRequestRet.Authenticator = string.Empty;
                ssoRequestRet.UserAccount = txtUserName.Value.Trim();

                //创建Token
                if (Authentication.CreateEACToken(ssoRequestRet))
                {
                    Post(ssoRequestRet);
                }

            }
        }
    }
}