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

namespace Authorization.Web.sys.Orgnization
{
    public partial class OrgnizationTree : System.Web.UI.Page
    {
        #region 定义
        /// <summary>
        /// 部门数据服务
        /// </summary>
        private OrgnizationService _OrgnizationService;
        /// <summary>
        /// 获取设置 部门数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _OrgnizationService ?? (_OrgnizationService = new OrgnizationService()); }
            set { _OrgnizationService = value; }
        }
        /// <summary>
        /// 部门列表
        /// </summary>
        private List<sys_Orgnization> _orgnizationList;
        protected List<sys_Orgnization> OrgnizationList
        {
            get { return _orgnizationList ?? (_orgnizationList = OrgnizationService.Where(p => p.IsDelete == false).ToList()); }
            set { _orgnizationList = value; }
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
            string selectedId = trOrgnizationTree.SelectedNodeID;
            if (String.IsNullOrEmpty(selectedId))
            {
                Alert.ShowInTop("请选择上级部门！");
                return;
            }
            PageContext.RegisterStartupScript(ActiveWindow.GetWriteBackValueReference(trOrgnizationTree.SelectedNode.Text, trOrgnizationTree.SelectedNodeID) + ActiveWindow.GetHideReference());
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            trOrgnizationTree.Items.Clear();

            var rootNode = new TreeNode
            {
                Text = ConfigHelper.GetAppSettingString("CorpName"),
                NodeID = "0",
                Expanded = true
            };

            trOrgnizationTree.Nodes.Add(rootNode);

            LoadChildNodes(rootNode);

        }
        private void LoadChildNodes(TreeNode node)
        {
            foreach (sys_Orgnization o in OrgnizationList.Where(p => p.PId == Convert.ToInt32(node.NodeID)))
            {
                var cNode = new TreeNode
                {
                    Text = o.OrgnizationName,
                    NodeID = o.Id.ToString(),
                    Expanded = true
                };
                //加载子部门信息
                node.Nodes.Add(cNode);
                LoadChildNodes(cNode);
            }
        }
        #endregion
    }
}