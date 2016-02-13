using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;

namespace Authorization.Web.sys.Menu
{
    public partial class MenuTree : System.Web.UI.Page
    {
        #region 定义
        protected int AppId
        {
            get
            {
                int queryIntValue = -1;
                try
                {
                    queryIntValue = Convert.ToInt32(Request.QueryString["aid"]);
                }
                catch (Exception)
                {
                    // TODO
                }
                return queryIntValue;
            }
        }
        /// <summary>
        /// 菜单数据服务
        /// </summary>
        private MenuService _MenuService;
        /// <summary>
        /// 获取设置 菜单数据服务
        /// </summary>
        protected MenuService MenuService
        {
            get { return _MenuService ?? (_MenuService = new MenuService()); }
            set { _MenuService = value; }
        }
        /// <summary>
        /// 菜单列表
        /// </summary>
        private List<sys_Menu> _menuList;
        protected List<sys_Menu> MenuList
        {
            get
            {
                return _menuList ??
                       (_menuList =
                           MenuService
                               .Where(p => p.IsDelete == false
                                           && ((AppId > 0 && p.AppId == AppId)
                                               || AppId <= 0))
                               .ToList());
            }
            set { _menuList = value; }
        }
        #endregion

        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            string selectedId = trMenuTree.SelectedNodeID;
            if (String.IsNullOrEmpty(selectedId))
            {
                Alert.ShowInTop("请选择菜单！");
                return;
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(trMenuTree.SelectedNode.Text, trMenuTree.SelectedNodeID) + ActiveWindow.GetHideReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            trMenuTree.Items.Clear();

            var rootNode = new TreeNode
            {
                Text = "系统菜单",
                NodeID = "0",
                Expanded = true,
                EnablePostBack = true
            };

            trMenuTree.Nodes.Add(rootNode);

            LoadChildNodes(rootNode);

        }
        private void LoadChildNodes(TreeNode node)
        {
            foreach (sys_Menu menu in MenuList.Where(p => p.PId == node.NodeID).OrderBy(p => p.SortIndex))
            {
                var cNode = new TreeNode
                {
                    Text = menu.MenuName,
                    NodeID = menu.Id,
                    IconUrl = menu.Ico,
                    EnablePostBack = true
                };
                //加载子部门信息
                node.Nodes.Add(cNode);
                LoadChildNodes(cNode);
            }
        }
        #endregion

    }
}