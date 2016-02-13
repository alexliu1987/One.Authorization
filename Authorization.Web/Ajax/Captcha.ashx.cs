using System;
using System.Drawing.Imaging;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using Authorization.Model;

namespace Authorization.Web.HttpHandler
{
    /// <summary>
    /// 生成验证码图片
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    public class Captcha : IHttpHandler, IReadOnlySessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            int width = 200;
            int height = 30;

            try
            {
                width = Convert.ToInt32(context.Request.QueryString["w"]);
                height = Convert.ToInt32(context.Request.QueryString["h"]);
            }
            catch (Exception)
            {
                // Nothing
            }

            if (context.Session["CaptchaImageText"] != null)
            {
                // 从 Session 中读取验证码，并创建图片
                CaptchaImage ci = new CaptchaImage(context.Session["CaptchaImageText"].ToString(), width, height, "Consolas");

                // 输出图片
                context.Response.Clear();
                context.Response.ContentType = "image/jpeg";

                ci.Image.Save(context.Response.OutputStream, ImageFormat.Jpeg);

                ci.Dispose();
            }
            else
            {
                context.Response.Redirect("../Login.aspx");
            }
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
