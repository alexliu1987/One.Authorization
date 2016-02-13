using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using Authorization.Framework.DataBase;
using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;

namespace Authorization.Web.sys.App
{
    public partial class AppTree : System.Web.UI.Page
    {
        #region 定义
        /// <summary>
        /// 部门数据服务
        /// </summary>
        private AppService _AppService;
        /// <summary>
        /// 获取设置 部门数据服务
        /// </summary>
        protected AppService AppService
        {
            get { return _AppService ?? (_AppService = new AppService()); }
            set { _AppService = value; }
        }
        /// <summary>
        /// 部门列表
        /// </summary>
        private List<sys_App> _AppList;
        protected List<sys_App> AppList
        {
            get { return _AppList ?? (_AppList = AppService.Where(p => p.IsDelete == false).ToList()); }
            set { _AppList = value; }
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
            string selectedId = trAppTree.SelectedNodeID;
            if (String.IsNullOrEmpty(selectedId))
            {
                Alert.ShowInTop("请选择所属模块！");
                return;
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(trAppTree.SelectedNode.Text, trAppTree.SelectedNodeID) + ActiveWindow.GetHideReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            trAppTree.Nodes.Clear();

            foreach (sys_App a in AppList)
            {
                var cNode = new TreeNode
                {
                    Text = a.AppName,
                    NodeID = a.Id.ToString(),
                    IconUrl = a.Ico,
                    Expanded = true,
                    EnablePostBack = true

                };
                trAppTree.Nodes.Add(cNode);
            }
        }
        #endregion
    }
}