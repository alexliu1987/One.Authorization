using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using FineUI;

namespace Authorization.Web.sys.App
{
    public partial class SelectIcon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Init_rblImageIcon();
            }
        }
        protected void Init_rblImageIcon()
        {

            DirectoryInfo dir = new DirectoryInfo(Server.MapPath("~/icon/App/"));
            DataTable imgdt = new DataTable();
            imgdt.Columns.Add("Text");
            imgdt.Columns.Add("Value");
            foreach (FileInfo fsi in dir.GetFileSystemInfos())
            {
                DataRow dr = imgdt.NewRow();
                dr["Text"] = "<img src='../../icon/App/" + fsi.Name + "' alt='" + fsi.Name + "'/>";
                dr["Value"] = fsi.Name;
                imgdt.Rows.Add(dr);
            }
            rblMenuIcon.DataSource = imgdt;
            rblMenuIcon.DataTextField = "Text";
            rblMenuIcon.DataValueField = "Value";
            rblMenuIcon.DataBind();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference("~/icon/" + rblMenuIcon.SelectedValue) + ActiveWindow.GetHidePostBackReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHideReference());
        }
    }
}