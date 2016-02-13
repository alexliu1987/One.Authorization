using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Data.Entity;
using Authorization.Framework.DataBase;
using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
namespace Authorization.Web.sys.Orgnization
{
    public partial class OrgnizagionList : PageBase
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
        private int SelectOid
        {
            get
            {
                string SelectedNodeID = trDepartmentTree.SelectedNodeID;
                if (!string.IsNullOrEmpty(SelectedNodeID))
                    return Convert.ToInt32(trDepartmentTree.SelectedNodeID);
                return 0;
            }
        }
        private sys_Orgnization _Orgnization;
        protected sys_Orgnization Orgnization
        {
            get { return _Orgnization ?? (_Orgnization = OrgnizationService.FirstOrDefault(p => p.Id == SelectOid)); }
            set { _Orgnization = value; }
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
        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSuccess = false;
            try
            {
                var orgnization = new sys_Orgnization
                {
                    OrgnizationName = tbxDName.Text.Trim(),
                    PId = Convert.ToInt32(hf_PDid.Text),
                    Describe = tbxRemark.Text,
                    IsDelete = false,
                    CreateDate = DateTime.Now,
                    CreateUserName = BaseUserName,
                    CreateRealName = BaseRealName
                };
                isSuccess = OrgnizationService.Add(orgnization);
            }
            catch { isSuccess = false; }
            finally
            {
                if (isSuccess)
                {
                    Log(LogType.新增, string.Format("添加部门：{0}", tbxDName.Text.Trim()), "部门管理");
                    Init_Tree();
                }
                else
                    Alert.Show("部门添加失败，请重试！", MessageBoxIcon.Warning);
            }

        }
        protected void trDepartmentTree_NodeCommand(object sender, TreeCommandEventArgs e)
        {
            TabStrip1.ActiveTabIndex = 1;
            Init_EditData();
        }
        protected void btnSave_Edit_Click(object sender, EventArgs e)
        {
            if (Orgnization != null)
            {
                bool isSuccess = false;
                try
                {
                    Orgnization.OrgnizationName = tbxDName_Edit.Text.Trim();
                    Orgnization.PId = Convert.ToInt32(hf_PDid_Edit.Text);
                    Orgnization.Describe = taRemark_Edit.Text;
                    Orgnization.ModifyDate = DateTime.Now;
                    Orgnization.ModifyUserName = BaseUserName;
                    Orgnization.ModifyRealName = BaseRealName;
                    isSuccess = OrgnizationService.SaveChanges() > 0;

                }
                catch
                {
                    isSuccess = false;
                }
                finally
                {
                    if (isSuccess)
                    {
                        Log(LogType.修改, string.Format("修改部门：{0}", tbxDName.Text.Trim()), "部门管理");
                        Init_Tree();
                    }
                    else
                        Alert.Show("部门修改失败，请重试！", MessageBoxIcon.Warning);
                }

            }
            else
                Alert.Show("部门修改失败，请重试！", MessageBoxIcon.Warning);
        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (Orgnization == null)
            {
                Alert.Show("所选择部门已删除，请勿重复操作！", MessageBoxIcon.Warning);
                return;
            }
            if (Orgnization.sys_Member.Count > 0)
            {
                Alert.Show("所选择部门中包含用户，不能删除！", MessageBoxIcon.Warning);
                return;
            }
            if (OrgnizationService.FirstOrDefault(p => p.PId == SelectOid) != null)
            {
                Alert.Show("所选择部门存在子部门，不能直接删除！", MessageBoxIcon.Warning);
                return;
            }
           
            bool isSuccess = false;
            try
            {
                Orgnization.IsDelete = true;

                Orgnization.ModifyDate = DateTime.Now;
                Orgnization.ModifyUserName = BaseUserName;
                Orgnization.ModifyRealName = BaseRealName;

                isSuccess = OrgnizationService.SaveChanges() > 0;
            }
            catch
            {
                isSuccess = false;
            }
            finally
            {
                if (isSuccess)
                {
                    Log(LogType.删除, string.Format("删除部门：{0}", Orgnization.OrgnizationName), "部门管理");
                    Init_Tree();
                }
                else
                    Alert.Show("部门删除失败，请重试！", MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            tbPDName.OnClientTriggerClick = Window1.GetSaveStateReference(tbPDName.ClientID, hf_PDid.ClientID)
                                         + Window1.GetShowReference("OrgnizationTree.aspx");
            tbPDName_Edit.OnClientTriggerClick = Window1.GetSaveStateReference(tbPDName_Edit.ClientID, hf_PDid_Edit.ClientID)
                                       + Window1.GetShowReference("OrgnizationTree.aspx");
            Init_Tree();
        }
        private void Init_Tree()
        {
            trDepartmentTree.Nodes.Clear();

            var rootNode = new TreeNode
            {
                Text = ConfigHelper.GetAppSettingString("CorpName"),
                NodeID = "0",
                Expanded = true
            };

            trDepartmentTree.Nodes.Add(rootNode);

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
                    Expanded = true,
                    EnablePostBack = true

                };
                //加载子部门信息
                node.Nodes.Add(cNode);
                LoadChildNodes(cNode);
            }
        }
        private void Init_EditData()
        {
            tbxDName_Edit.Text = Orgnization.OrgnizationName;
            taRemark_Edit.Text = Orgnization.Describe;
            if (Orgnization.PId == 0)
                tbPDName_Edit.Text = ConfigHelper.GetAppSettingString("CorpName");
            else
                tbPDName_Edit.Text = OrgnizationService.FirstOrDefault(p => p.Id == Orgnization.PId).OrgnizationName;
            hf_PDid_Edit.Text = Orgnization.PId.ToString();
        }
        #endregion

    }
}