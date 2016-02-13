using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Authorization.Framework.Web;
namespace Authorization.Web
{
    public partial class ErrorPage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                var message = Cookie.GetValue("ERRORINFO");

                lMsg.Text = message;
            }
        }
    }
}