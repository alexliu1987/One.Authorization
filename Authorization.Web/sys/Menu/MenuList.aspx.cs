using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.UI;
using System.Data.Entity;
using Authorization.Framework.DataBase;
using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;


namespace Authorization.Web.sys.Menu
{
    public partial class MenuList : PageBase
    {
        #region 定义
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
        protected List<sys_Menu> mList
        {
            get { return _menuList ?? (_menuList = MenuService.Where(p => p.IsDelete == false).ToList()); }
            set { _menuList = value; }
        }
        /// <summary>
        /// 获取设置 模块数据服务
        /// </summary>
        private AppService _AppService;
        protected AppService AppService
        {
            get { return _AppService ?? (_AppService = new AppService()); }
            set { _AppService = value; }
        }
        /// <summary>
        /// 模块列表
        /// </summary>
        private List<sys_App> _appList;
        protected List<sys_App> AppList
        {
            get { return _appList ?? (_appList = AppService.Where(a => a.IsDelete == false).ToList()); }
            set { _appList = value; }
        }
        private int SelectAppid
        {
            get
            {
                string SelectedNodeID = trApp.SelectedNodeID;
                if (!string.IsNullOrEmpty(SelectedNodeID))
                    return Convert.ToInt32(trApp.SelectedNodeID);
                return 0;
            }
        }
        #endregion

        #region 事件
        protected override void Init_Page()
        {
            BaseToolBar = Toolbar2;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void Window1_Close(object sender, FineUI.WindowCloseEventArgs e)
        {
            BindGrid();
        }
        protected override void BtnClick(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;

            switch ((AuthorizationButtonType)System.Enum.Parse(typeof(AuthorizationButtonType), btn.ID.Replace("btn", "").ToUpper()))
            {
                case AuthorizationButtonType.ADD:
                    AddClick();
                    break;
                case AuthorizationButtonType.EDIT:
                    //EditClick(Grid1, Window1, "MenuEdit", "编辑菜单");
                    EditClick();
                    break;
                case AuthorizationButtonType.DELETE:
                    DeleteClick();
                    break;
                case AuthorizationButtonType.EXPORT:
                    ExportClick();
                    break;
            }

        }
        protected void trApp_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            BindGrid();
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            Init_Tree();
            BindGrid();
        }
        private void Init_Tree()
        {
            trApp.Nodes.Clear();

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
                trApp.Nodes.Add(cNode);
            }
        }
        private void BindGrid()
        {
            List<MenuInfo> miList = new List<MenuInfo>();
            foreach (
                sys_Menu m in
                    mList.Where(p => p.PId == "0" && ((SelectAppid != 0 && p.AppId == SelectAppid) || SelectAppid == 0))
                        .OrderBy(p => p.SortIndex))
            {
                MenuInfo mi = new MenuInfo();
                mi.Mid = m.Id;
                mi.AppId = m.AppId;
                mi.MenuName = m.MenuName;
                mi.MenuCode = m.MenuCode;
                mi.PMid = m.PId;
                mi.Ico = m.Ico;
                mi.Url = m.Url;
                mi.TreeLevel = 0;
                mi.IsUsing = m.IsUsing;
                mi.IsOperRes = m.IsOperRes;
                mi.SortIndex = m.SortIndex;
                mi.Describe = m.Describe;
                mi.ActionList = GetActionListStr(m.sys_Action.ToList());
                miList.Add(mi);
                CreateTreeNode(m.Id, miList, 0);
            }
            Grid1.DataSource = miList;
            Grid1.DataBind();

        }
        private void CreateTreeNode(string PMid, List<MenuInfo> miList, int TreeLevel)
        {
            TreeLevel++;
            foreach (sys_Menu m in mList.Where(p => p.PId == PMid).OrderBy(p => p.SortIndex))
            {
                MenuInfo mi = new MenuInfo();
                mi.Mid = m.Id;
                mi.AppId = m.AppId;
                mi.MenuName = m.MenuName;
                mi.MenuCode = m.MenuCode;
                mi.PMid = m.PId;
                mi.Ico = m.Ico;
                mi.Url = m.Url;
                mi.TreeLevel = TreeLevel;
                mi.IsUsing = m.IsUsing;
                mi.IsOperRes = m.IsOperRes;
                mi.SortIndex = m.SortIndex;
                mi.Describe = m.Describe;
                mi.ActionList = GetActionListStr(m.sys_Action.ToList());
                miList.Add(mi);
                CreateTreeNode(m.Id, miList, TreeLevel);
            }

        }
        private void AddClick()
        {
            PageContext.RegisterStartupScript(
                     Window1.GetShowReference(string.Format("MenuEdit.aspx?aid={0}&action=1", SelectAppid), "新增菜单"));

        }
        private void EditClick()
        {
            string id = Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0].ToString();
            PageContext.RegisterStartupScript(
                Window1.GetShowReference(string.Format("MenuEdit.aspx?aid={0}&id={1}&action=2", SelectAppid, id), "编辑菜单"));

        }
        private void DeleteClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<string> selectIds = GetSelectIds();

            try
            {
                IQueryable<sys_Menu> list = MenuService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_Menu s in list)
                {
                    s.IsDelete = true;

                    s.ModifyDate = DateTime.Now;
                    s.ModifyUserName = BaseUserName;
                    s.ModifyRealName = BaseRealName;
                    deletelist += s.MenuName + ",";
                }
                MenuService.SaveChanges();

                Log(LogType.删除, string.Format("删除菜单：{0}", deletelist.Trim(',')), "菜单管理");

                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        private string GetActionListStr(IEnumerable<sys_Action> actionList)
        {
            string re = "";
            int i = 1;
            foreach (sys_Action a in actionList)
            {
                re += string.Format("【{0}】{1}", i, a.ActionName);
                i++;
            }
            return re;
        }
        private IEnumerable<string> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new string[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Grid1.DataKeys[selections[i]][0].ToString();
            }
            return selectIds;
        }
        private void ExportClick()
        {

            var excelData = from q in mList
                            select new MenuExcel
                            {
                                菜单ID = q.Id,
                                菜单名称 = q.MenuName,
                                上级菜单ID = q.PId,
                                图标 = q.Ico,
                                连接 = q.Url,
                                是否可用 = q.IsUsing,
                                有无范围 = q.IsOperRes,
                                排序 = q.SortIndex,
                                描述 = q.Describe
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "菜单管理", "菜单管理" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        #endregion

    }
}