using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using FineUI;

using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
namespace Authorization.Web
{
    public partial class Index : PageBase
    {
        #region 定义
        /// <summary>
        /// 数据服务
        /// </summary>
        private MenuService _MenuService;
        /// <summary>
        /// 获取 数据服务
        /// </summary>
        protected MenuService MenuService
        {
            get { return _MenuService ?? (_MenuService = new MenuService()); }
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        private List<sys_Menu> _sysMenuList;
        /// <summary>
        /// 获取 菜单列表
        /// </summary>
        protected List<sys_Menu> sysMenuList
        {
            get { return _sysMenuList ?? (_sysMenuList = MenuService.Where(p => p.AppId==1 && p.IsUsing && p.IsDelete == false).OrderBy(p => p.SortIndex).ToList()); }
        }
        #endregion

        #region 事件
        protected void Page_Init(object sender, EventArgs e)
        {
            JObject ids = GetClientIDS(mainTabStrip);
            Accordion accordionMenu = InitAccordionMenu();
            ids.Add("mainMenu", accordionMenu.ClientID);
            ids.Add("menuType", "accordion");
            ids.Add("theme", PageManager.Instance.Theme.ToString());

            // 只在页面第一次加载时注册客户端用到的脚本
            if (!Page.IsPostBack)
            {
                string idsScriptStr = String.Format("window.IDS={0};", ids.ToString(Newtonsoft.Json.Formatting.None));
                PageContext.RegisterStartupScript(idsScriptStr);
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void lbtnLoginOut_Click(object sender, EventArgs e)
        {
            try
            {
                var server = new MemberService();
                var m = server.FirstOrDefault(p => p.id == BaseUid);
                if (m != null)
                    m.sys_MemberExtend.IsOnline = false;
                server.SaveChanges();
            }
            catch { }
            finally
            {
                FormsAuthentication.SignOut();
                Session.Abandon();
                Session.Clear();
                FormsAuthentication.RedirectToLoginPage();
            }
        }
        #endregion

        #region 方法
        private Accordion InitAccordionMenu()
        {
            string rightstr = BaseRightStr;

            Accordion accordionMenu = new Accordion();

            accordionMenu.ID = "accordionMenu";
            accordionMenu.EnableFill = true;
            accordionMenu.ShowBorder = false;
            accordionMenu.ShowHeader = false;
            Region2.Items.Add(accordionMenu);

            foreach (sys_Menu m in sysMenuList.Where(p => p.PId == "0").OrderBy(p => p.SortIndex).ToList())
            {
                if (!rightstr.Contains(m.MenuCode + "-0"))
                    continue;
                AccordionPane accordionPane = new AccordionPane();
                accordionPane.Title = m.MenuName;
                accordionPane.Layout = Layout.Fit;
                accordionPane.ShowBorder = false;
                accordionPane.BodyPadding = "2px 0 0 0";
                accordionPane.IconUrl = m.Ico;
                accordionMenu.Items.Add(accordionPane);

                Tree innerTree = new Tree();
                innerTree.EnableArrows = true;
                innerTree.ShowBorder = false;
                innerTree.ShowHeader = false;
                innerTree.EnableIcons = false;
                innerTree.AutoScroll = true;
                accordionPane.Items.Add(innerTree);

                CreateNode(m.Id, innerTree.Nodes, rightstr);
            }
            return accordionMenu;

        }
        protected void CreateNode(string parentID, FineUI.TreeNodeCollection pnode, string RightStr)
        {
            foreach (sys_Menu m in sysMenuList.Where(p => p.PId == parentID).OrderBy(p => p.SortIndex).ToList())
            {
                if (!RightStr.Contains(m.MenuCode + "-0"))
                    continue;
                FineUI.TreeNode node = new FineUI.TreeNode();
                pnode.Add(node);
                node.NodeID = m.MenuCode;
                node.Text = m.MenuName;
                node.NavigateUrl = ResolveUrl(m.Url);
                node.IconUrl = m.Ico;

                CreateNode(m.Id, node.Nodes, RightStr);
            }
        }
        private JObject GetClientIDS(params ControlBase[] ctrls)
        {
            JObject jo = new JObject();
            foreach (ControlBase ctrl in ctrls)
            {
                jo.Add(ctrl.ID, ctrl.ClientID);
            }

            return jo;
        }
        #endregion



    }
}