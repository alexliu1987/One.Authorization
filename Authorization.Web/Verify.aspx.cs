using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Authorization.Common;
using Authorization.Model;
using Authorization.Services;
using SSO.Common;
using SSO.Common.SSO;

namespace Authorization.Web
{
    public partial class Verify : System.Web.UI.Page
    {
        protected string GetIP
        {
            get
            {
                string result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (string.IsNullOrEmpty(result))
                    result = HttpContext.Current.Request.UserHostAddress;
                if (string.IsNullOrEmpty(result) || !Utility.IsIP(result))
                    return "127.0.0.1";
                return result;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region SSO 部分代码
                SSORequest ssoRequest = new SSORequest();

                if (string.IsNullOrEmpty(Request["AppCode"]))
                {
                    ssoRequest.AppCode = ConfigurationManager.AppSettings["SystemCode"];
                    ssoRequest.TimeStamp = DateTime.Now.ToString("yyyyMMddHHmm");
                    ssoRequest.AppUrl = Request.Url.ToString();
                    Authentication.CreateAppToken(ssoRequest);

                    Post(ssoRequest);
                }
                else if (!string.IsNullOrEmpty(Request["AppCode"])
                    && !string.IsNullOrEmpty(Request["TimeStamp"])
                    && !string.IsNullOrEmpty(Request["AppUrl"])
                    && !string.IsNullOrEmpty(Request["UserAccount"])
                    && !string.IsNullOrEmpty(Request["Authenticator"]))
                {
                    ssoRequest.AppCode = Request["AppCode"];
                    ssoRequest.TimeStamp = Request["TimeStamp"];
                    ssoRequest.AppUrl = Request["AppUrl"];
                    ssoRequest.UserAccount = Request["UserAccount"];
                    ssoRequest.Authenticator = Request["Authenticator"];

                    if (Authentication.ValidateEACToken(ssoRequest))
                    {
                        var userAccount = Request["UserAccount"];
                        try
                        {
                            var server = new MemberService();
                            //string password = DESEncrypt.Encrypt(xpassword);
                            var member = server.FirstOrDefault(m => m.UserName == userAccount && m.IsDelete == false);
                            if (member != null)
                            {
                                if (member.IsUsing == false)
                                {
                                    //_response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "用户已锁定，请联系管理员！" + "\"}";
                                    return;
                                }
                                member.sys_MemberExtend.IsOnline = true;
                                member.sys_MemberExtend.LastLoginIP = GetIP;
                                member.sys_MemberExtend.LastLoginTime = DateTime.Now;

                                var ipserver = new ForbidIpService();
                                var ipList = ipserver.Where(i => i.IsUsing && i.IsDelete == false).ToList().Select(q => q.IP);
                                if (member.UserName != "admin" && ipList.Contains(member.sys_MemberExtend.LastLoginIP))
                                {
                                    //_response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "您的IP地址已锁定，请联系管理员！" + "\"}";
                                    return;
                                }

                                CreateFormsAuthenticationTicket(member, false, DateTime.Now.AddMinutes(120));
                                server.SaveChanges();
                                var loginlogservice = new LoginLogService();
                                var loginlog = new sys_LoginLog
                                {
                                    Uid = member.id,
                                    RealName = member.RealName,
                                    Ip = GetIP,
                                    Address = "",
                                    Mac = "",
                                    LoginTime = DateTime.Now
                                };
                                loginlogservice.Add(loginlog);
                                //_response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + "登录成功，正在转到主页..." + "\"}";
                            }
                            else
                                //_response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "用户名或者密码不正确！" + "\"}";
                                return;
                            Response.Redirect("Index.aspx");
                        }
                        catch (Exception ex)
                        {
                            //_response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + ex.Message + "\"}";
                        }
                    }
                }

                ViewState["SSORequest"] = ssoRequest;

                #endregion
            }
        }

        void Post(SSORequest ssoRequest)
        {
            PostService ps = new PostService();
            //认证中心(主站)地址
            string EACUrl = ConfigurationManager.AppSettings["SSOUrl"];
            ps.Url = EACUrl;
            //ps.Add("UserAccount", ssoRequest.UserAccount);
            ps.Add("AppCode", ssoRequest.AppCode);
            ps.Add("TimeStamp", ssoRequest.TimeStamp);
            ps.Add("AppUrl", ssoRequest.AppUrl);
            ps.Add("Authenticator", ssoRequest.Authenticator);

            ps.Post();
        }
        protected void CreateFormsAuthenticationTicket(sys_Member model, bool isPersistent, DateTime expiration)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1,
                                               model.id.ToString(),// 与票证关联的用户名
                                               DateTime.Now,// 票证发出时间
                                               expiration, // 票证过期时间
                                               isPersistent,// 如果票证将存储在持久性 Cookie 中（跨浏览器会话保存），则为 true；否则为 false。
                                               model.UserName                 // 存储在票证中的用户特定的数据 
                                               );
            // 对Forms身份验证票据进行加密，然后保存到客户端Cookie中
            string hashTicket = FormsAuthentication.Encrypt(ticket);
            HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, hashTicket);
            cookie.HttpOnly = true;
            if (isPersistent)
            {
                cookie.Expires = expiration;
            }
            else
            {
                cookie.Expires = DateTime.MinValue;
            }

            Response.Cookies.Add(cookie);
        }
    }
}