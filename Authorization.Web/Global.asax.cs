using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

using Authorization.Common;
using Authorization.Model;
using Authorization.Services;
using Authorization.Framework.Web;
namespace Authorization.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Application.Lock();
            Application["AuthorizationCOUNT"] = Convert.ToInt32(Application["AuthorizationCOUNT"]) + 1;//用于统计网站在线人数
            Application.UnLock();
        }
        protected void Session_End(object sender, EventArgs e)
        {
            // 在会话结束时运行的代码。
            // 注意: 只有在 Web.config 文件中的 sessionstate 模式设置为
            // InProc 时，才会引发 Session_End 事件。如果会话模式设置为 StateServer
            // 或 SQLServer，则不会引发该事件。
            Application.Lock();
            Application["AuthorizationCOUNT"] = Convert.ToInt32(Application["AuthorizationCOUNT"]) - 1;//用于统计网站在线人数
            Application.UnLock();
        }
        protected void Application_Error(object sender, EventArgs e)
        {
            var context = HttpContext.Current;
            if (context != null)
            {
                Exception ex = HttpContext.Current.Server.GetLastError();
                //if (ex.Message == "授权已到期，请联系管理员。")
                //{
                //    context.Response.Redirect("~/Login.html");
                //    return;
                //}

                var logservice = new ErrorLogService();
                var memberservice = new MemberService();
                int uid = Convert.ToInt32(context.User.Identity.Name);
                var member = memberservice.FirstOrDefault(m => m.id == uid);
               
                var errorlog = new sys_ErrorLog
                {
                    uid = member.id,
                    RealName = member.RealName,
                    LogContent = ex.InnerException.Message,
                    Ip = GetIP,
                    LogTime = DateTime.Now
                };
                logservice.Add(errorlog);

                var errorcontent = new StringBuilder();
                errorcontent.AppendLine(string.Format("<b>发生时间：</b>{0}<br/><br/>", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss")));
                errorcontent.AppendLine(string.Format("<b>错误描述：</b>{0}<br/><br/>", ex.Message.Replace("\r\n", "")));
                errorcontent.AppendLine(string.Format("<b>错误对象：</b>{0}<br/><br/>", ex.Source));
                errorcontent.AppendLine(string.Format("<b>错误页面：</b>{0}<br/><br/>", HttpContext.Current.Request.Url));
                errorcontent.AppendLine(string.Format("<b>浏览器IE：</b>{0}<br/><br/>", HttpContext.Current.Request.UserAgent));
                errorcontent.AppendLine(string.Format("<b>服务器IP：</b>{0}<br/><br/>", HttpContext.Current.Request.ServerVariables.Get("Local_Addr")));
                errorcontent.AppendLine(string.Format("<b>方法名称：</b>{0}<br/><br/>", ex.TargetSite));
                errorcontent.AppendLine(string.Format("<b>C#类名称：</b>{0}<br/><br/>", ex.TargetSite.DeclaringType));
                errorcontent.AppendLine(string.Format("<b>成员变量：</b>{0}<br/><br/>", ex.TargetSite.MemberType));

                Cookie.SetObj("ERRORINFO", errorcontent.ToString());
                Server.ClearError();

                //出错画面处理
                context.Response.Redirect("~/ErrorPage.aspx");
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
    }
}