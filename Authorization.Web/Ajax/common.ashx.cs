using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
using SSO.Common.SSO;

namespace Authorization.Web.Ajax
{
    /// <summary>
    /// common 的摘要说明
    /// </summary>
    public class common : IHttpHandler
    {
        private string _operType = string.Empty;
        private string _response = string.Empty;
        public void ProcessRequest(HttpContext context)
        {
            _operType = context.Request["oper"] ?? "";
            switch (_operType)
            {
                case "ajaxLogin":
                    var uName = context.Request["name"];
                    var uPassword = context.Request["password"];
                    AjaxLogin(uName, uPassword, context);
                    break;
                case "ajaxGetServerTime":
                    AjaxGetServerTime();
                    break;
                case "ajaxLoginLoad":
                    AjaxLoginLoad(context);
                    break;
            }
            context.Response.Write(_response);
        }
        /// <summary>
        /// 取服务器时间
        /// </summary>
        private void AjaxGetServerTime()
        {
            _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒 [dddd]") + "\"}";
        }
        private void AjaxLoginLoad(HttpContext context)
        {
            if (context.User.Identity.IsAuthenticated)
                _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + "已登录，正在自动转到主页..." + "\"}";

        }
        private void AjaxLogin(string xname, string xpassword, HttpContext context)
        {
            try
            {
                var server = new MemberService();
                string password = DESEncrypt.Encrypt(xpassword, xname);
                var member = server.FirstOrDefault(m => m.UserName == xname && m.Password == password && m.IsDelete == false);
                if (member != null)
                {
                    if (member.IsUsing == false)
                    {
                        _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "用户已锁定，请联系管理员！" + "\"}";
                        return;
                    }
                    member.sys_MemberExtend.IsOnline = true;
                    member.sys_MemberExtend.LastLoginIP = GetIP;
                    member.sys_MemberExtend.LastLoginTime = DateTime.Now;

                    var ipserver = new ForbidIpService();
                    var ipList = ipserver.Where(i => i.IsUsing && i.IsDelete == false).ToList().Select(q => q.IP);
                    if (member.UserName != "admin" && ipList.Contains(member.sys_MemberExtend.LastLoginIP))
                    {
                        _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "您的IP地址已锁定，请联系管理员！" + "\"}";
                        return;
                    }

                    CreateFormsAuthenticationTicket(context, member, false, DateTime.Now.AddMinutes(120));
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
                    _response = "{\"result\" :\"" + 1 + "\",\"returnval\" :\"" + "登录成功，正在转到主页..." + "\"}";
                }
                else
                    _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + "用户名或者密码不正确！" + "\"}";

            }
            catch (Exception ex)
            {
                _response = "{\"result\" :\"" + 0 + "\",\"returnval\" :\"" + ex.Message + "\"}";
            }

        }
        protected static string GetIP
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
        protected void CreateFormsAuthenticationTicket(HttpContext context, sys_Member model, bool isPersistent, DateTime expiration)
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

            context.Response.Cookies.Add(cookie);
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}