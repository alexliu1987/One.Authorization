using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Globalization;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
using Authorization.Framework.Enum;
using Authorization.Framework.DataBase;
using Authorization.Framework.EntityRepository;

namespace Authorization.Web.sys.Role
{
    public partial class RoleList : PageBase
    {

        #region 定义
        private DbContext _dbcontext;
        protected DbContext DbContext
        {
            get { return _dbcontext ?? (_dbcontext = DbContextHelper.CreateDbContext()); }
        }
        /// <summary>
        /// 角色数据服务
        /// </summary> 
        private RoleService _RoleService;
        /// <summary>    
        /// 角色数据服务  
        /// </summary>
        protected RoleService RoleService
        {
            get { return _RoleService ?? (_RoleService = new RoleService(DbContext)); }
            set { _RoleService = value; }
        }
        private int SelectRoleId
        {
            get
            {
                if (Grid1.SelectedRowIndex >= 0)
                {
                    var SelectedID = Grid1.DataKeys[Grid1.SelectedRowIndex][0];
                    if (SelectedID != null)
                        return Convert.ToInt32(SelectedID);
                }
                return 0;
            }
        }
        private sys_Role _Role;
        protected sys_Role Role
        {
            get { return _Role ?? (_Role = RoleService.FirstOrDefault(p => p.Id == SelectRoleId)); }
            set { _Role = value; }
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
            get { return _MenuService ?? (_MenuService = new MenuService(DbContext)); }
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
        /// <summary>
        /// 菜单数据权限
        /// </summary>
        private Dictionary<string, string> MenuOperRes
        {
            get
            {
                return ViewState["MENUOPERRES"] as Dictionary<string, string> ?? (new Dictionary<string, string>());
            }
            set { ViewState["MENUOPERRES"] = value; }
        }
        /// <summary>
        /// 用户数据服务
        /// </summary>
        private MemberService _MemberService;
        protected MemberService MemberService
        {
            get { return _MemberService ?? (_MemberService = new MemberService()); }
        }
        #endregion

        #region 事件
        protected override void Init_Page()
        {
            BasePage = this.Page;
        }
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
                LoadData();
        }

        #region 角色添加、编辑、删除
        protected void btnSave_Edit_Click(object sender, EventArgs e)
        {
            if (Role != null)
            {
                bool isSuccess = false;
                try
                {
                    Role.RoleName = tbxRoleName_Edit.Text.Trim();
                    Role.RoleCode = tbxRoleCode_Edit.Text.Trim();
                    Role.Describe = taRemark_Edit.Text.Trim();

                    Role.ModifyDate = DateTime.Now;
                    Role.ModifyUserName = BaseUserName;
                    Role.ModifyRealName = "管理员";

                    isSuccess = RoleService.SaveChanges() > 0;
                }
                catch { }
                finally
                {
                    if (isSuccess)
                    {
                        Log(LogType.修改, string.Format("修改角色：{0}-{1}", Role.RoleName, Role.RoleCode), "角色管理");
                        BindGrid();
                    }
                    else
                        Alert.Show("角色修改失败，请重试！", MessageBoxIcon.Warning);
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (RoleService.FirstOrDefault(p => (p.RoleName == tbxRoleName.Text || p.RoleCode == tbxRoleCode.Text) && p.IsDelete == false) != null)
            {
                Alert.Show("角色名已存在，请勿重复添加！", MessageBoxIcon.Warning);
                return;
            }
            var r = new sys_Role
            {
                RoleName = tbxRoleName.Text.Trim(),
                RoleCode = tbxRoleCode.Text.Trim(),
                Describe = tbxRemark.Text.Trim(),

                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName
            };

            RoleService.Add(r);
            if (DbContext.SaveChanges() > 0)
            {
                Log(LogType.新增, string.Format("添加角色：{0}-{1}", r.RoleName, r.RoleCode), "角色管理");
                BindGrid();
            }
            else
                Alert.Show("角色添加失败，请重试！", MessageBoxIcon.Warning);

        }

        protected void btnDel_Click(object sender, EventArgs e)
        {
            if (Role != null)
            {
                if (Role.sys_Member.Count > 0)
                {
                    Alert.Show("所选角色中包含用户，不能删除！", MessageBoxIcon.Warning);
                    return;
                }
                bool isSuccess = false;
                try
                {

                    Role.IsDelete = true;

                    Role.ModifyDate = DateTime.Now;
                    Role.ModifyUserName = BaseUserName;
                    Role.ModifyRealName = BaseRealName;

                    Log(LogType.删除, string.Format("删除角色：{0}-{1}", Role.RoleName, Role.RoleCode), "角色管理");

                    isSuccess = RoleService.SaveChanges() > 0;
                }
                catch { }
                finally
                {
                    if (isSuccess)
                        BindGrid();
                    else
                        Alert.Show("角色删除失败，请重试！", MessageBoxIcon.Warning);
                }
            }
            else
                Alert.Show("没有要删除的角色！", MessageBoxIcon.Warning);
        }
        #endregion

        #region 角色授权
        protected void Grid1_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            switch (TabStrip1.ActiveTabIndex)
            {
                case 0:
                case 1:
                    TabStrip1.ActiveTabIndex = 1;
                    InitEdit();
                    break;
                case 2:
                    BindMenuGrid();
                    InitMenuOperRes();
                    break;
                case 3:
                    BindUserGrid();
                    break;
            }
        }
        protected void TabStrip1_TabIndexChanged(object sender, EventArgs e)
        {
            switch (TabStrip1.ActiveTabIndex)
            {
                case 0:
                case 1:
                    break;
                case 2:
                    BindMenuGrid();
                    InitMenuOperRes();
                    InitTree();
                    break;
                case 3:
                    BindUserGrid();
                    break;
            }
        }
        protected void Grid2_RowDataBound(object sender, GridRowEventArgs e)
        {
            string roleRightStr = "";
            string roleOperResStr = "";
            if (Role != null)
            {
                roleRightStr = Role.RoleRightStr ?? "";
                roleOperResStr = Role.RoleOperResStr ?? "";
            }

            var cblpower = (System.Web.UI.WebControls.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("cblPowers");
            var mid = Grid2.DataKeys[e.RowIndex][0].ToString();
            var m = mList.FirstOrDefault(p => p.Id == mid);
            Debug.Assert(m != null, "m != null");

            //添加浏览
            var item0 = new System.Web.UI.WebControls.ListItem
            {
                Text = "浏览",
                Value = m.MenuCode + "-0"
            };
            item0.Attributes["data-qtip"] = m.MenuCode;

            //初始化浏览权限
            item0.Selected = roleRightStr.Contains(m.MenuCode + "-0");

            cblpower.Items.Add(item0);
            foreach (var item in m.sys_Action.Select(a => new System.Web.UI.WebControls.ListItem
            {
                Text = a.ActionName,
                Value = string.Format("{0}-{1}", m.MenuCode, a.ActionCode)
            }))
            {
                item.Attributes["data-qtip"] = m.MenuCode;

                item.Selected = roleRightStr.Contains(item.Value);
                cblpower.Items.Add(item);
            }

        }
        protected void lbtnOperRes_Click(object sender, EventArgs e)
        {
            InitTree();
        }
        protected void trUser_NodeCheck(object sender, TreeCheckEventArgs e)
        {
            if (e.Checked)
            {

                trUser.CheckAllNodes(e.Node.Nodes);
            }
            else
            {
                trUser.UncheckAllNodes(e.Node.Nodes);
            }
            CreateMenuOperRes();
        }
        protected void Grid2_RowClick(object sender, GridRowClickEventArgs e)
        {
            if (!Convert.ToBoolean(Grid2.DataKeys[Grid2.SelectedRowIndex][1]))
                trUser.Hidden = true;
            else
                trUser.Hidden = false;
        }
        protected void btnSaveRoleRight_Click(object sender, EventArgs e)
        {
            string RoleRightStr = "";
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                var cblPowers = (System.Web.UI.WebControls.CheckBoxList)Grid2.Rows[i].FindControl("cblPowers");
                foreach (System.Web.UI.WebControls.ListItem item in cblPowers.Items)
                {
                    if (item.Selected)
                    {
                        RoleRightStr += item.Value + ",";
                    }
                }
            }
            string RoleOperResStr = "";
            foreach (KeyValuePair<string, string> kvp in MenuOperRes)
            {
                RoleOperResStr += string.Format("{0}-{1}", kvp.Key, kvp.Value.Replace(",", string.Format(",{0}-", kvp.Key))) + ",";
            }
            Role.RoleRightStr = RoleRightStr.Trim(',');
            Role.RoleOperResStr = RoleOperResStr.Trim(',');

            if (RoleService.SaveChanges() >= 0)
            {
                Log(LogType.授权, string.Format("角色授权：{0}-{1}", Role.RoleName, Role.RoleCode), "角色管理");

                Alert.Show("角色保存成功！");
            }
            else
                Alert.Show("角色保存失败！", MessageBoxIcon.Error);
        }
        #endregion

        #region 角色中用户
        protected void btnAddUser_Click(object sender, EventArgs e)
        {
            PageContext.RegisterStartupScript(
                       Window1.GetShowReference(string.Format("../User/UserSelectForRole.aspx?id={0}", Grid1.DataKeys[Grid1.SelectedRowIndex][0]), "选择角色外用户"));
        }

        protected void btnRemoveUser_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndex < 0)
            {
                Alert.Show("请选择角色！", MessageBoxIcon.Warning);
                return;
            }
            bool isSuccess = false;
            string realnamestr = "";

            try
            {
                List<int> uidList = new List<int>();
                Grid3.SelectedRowIndexArray.ToList().ForEach(i => uidList.Add(Convert.ToInt32(Grid3.DataKeys[i][0])));
                List<sys_Member> memberlist = MemberService.Where(m => uidList.Contains(m.id)).ToList();
                foreach (sys_Member m in memberlist)
                {
                    realnamestr += m.RealName + ",";
                    m.sys_Role.Remove(m.sys_Role.FirstOrDefault(r => r.Id == Role.Id));
                }
                isSuccess = MemberService.SaveChanges() > 0;
            }
            catch { }
            finally
            {
                if (isSuccess)
                {
                    Log(LogType.修改, string.Format("从{0}-{1}角色中移除用户：{2}", Role.RoleName, Role.RoleCode, realnamestr.Trim(',')), "角色管理");
                    BindUserGrid();
                }
                else
                    Alert.Show("角色中的用户删除失败，请重试！", MessageBoxIcon.Error);
            }
        }
        protected void Grid3_PageIndexChange(object sender, GridPageEventArgs e)
        {
            Grid3.PageIndex = e.NewPageIndex;
            BindUserGrid();
        }

        protected void Grid3_Sort(object sender, GridSortEventArgs e)
        {
            Grid3.SortField = e.SortField;
            Grid3.SortDirection = e.SortDirection;
            BindUserGrid();
        }
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid3.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindUserGrid();
        }
        protected void Window1_Close(object sender, WindowCloseEventArgs e)
        {
            BindUserGrid();
        }
        #endregion

        #endregion

        #region 方法
        private void LoadData()
        {
            btnSaveRoleRight.OnClientClick = Grid1.GetNoSelectionAlertReference("请选择角色！");
            btnRemoveUser.OnClientClick = Grid3.GetNoSelectionAlertReference("请选择要从角色中移除的用户！");
            btnAddUser.OnClientClick = Grid1.GetNoSelectionAlertReference("请选择角色！");
            BindGrid();
        }
        private void InitEdit()
        {
            if (Role != null)
            {
                tbxRoleName_Edit.Text = Role.RoleName;
                tbxRoleCode_Edit.Text = Role.RoleCode;
                taRemark_Edit.Text = Role.Describe;
            }
        }
        private void BindGrid()
        {
            Grid1.DataSource = RoleService.Where(p => p.IsDelete == false);
            Grid1.DataBind();
        }

        #region 角色授权
        private void BindMenuGrid()
        {
            List<MenuInfo> miList = new List<MenuInfo>();
            foreach (sys_Menu m in mList.Where(p => p.PId == "0").OrderBy(p => p.AppId).ThenBy(p => p.SortIndex))
            {
                MenuInfo mi = new MenuInfo();
                mi.Mid = m.Id;
                mi.AppId = m.AppId;
                mi.AppName = m.sys_App.AppName;
                mi.AppCode = m.sys_App.AppCode;
                mi.MenuName = m.MenuName;
                mi.PMid = m.PId;
                mi.Ico = m.Ico;
                mi.Url = m.Url;
                mi.TreeLevel = 0;
                mi.IsUsing = m.IsUsing;
                mi.IsOperRes = m.IsOperRes;
                mi.SortIndex = m.SortIndex;
                mi.Describe = m.Describe;
                //mi.ActionList = GetActionListStr(m.sys_Action.ToList());
                miList.Add(mi);
                CreateTreeNode(m.Id, miList, 0);
            }
            Grid2.DataSource = miList;
            Grid2.DataBind();

        }
        private void CreateTreeNode(string PMid, List<MenuInfo> miList, int TreeLevel)
        {
            TreeLevel++;
            foreach (sys_Menu m in mList.Where(p => p.PId == PMid).OrderBy(p => p.SortIndex))
            {
                MenuInfo mi = new MenuInfo();
                mi.Mid = m.Id;
                mi.AppId = m.AppId;
                //mi.AppName = m.sys_App.AppName;
                //mi.AppCode = m.sys_App.AppCode;
                mi.MenuName = m.MenuName;
                mi.PMid = m.PId;
                mi.Ico = m.Ico;
                mi.Url = m.Url;
                mi.TreeLevel = TreeLevel;
                mi.IsUsing = m.IsUsing;
                mi.IsOperRes = m.IsOperRes;
                mi.SortIndex = m.SortIndex;
                mi.Describe = m.Describe;

                miList.Add(mi);
                CreateTreeNode(m.Id, miList, TreeLevel);
            }

        }
        private void InitTree()
        {
            trUser.Nodes.Clear();

            var rootNode = new TreeNode
            {
                Text = ConfigHelper.GetAppSettingString("CorpName"),
                NodeID = "0",
                Expanded = true,
                EnableCheckBox = true,
                AutoPostBack = true
            };

            trUser.Nodes.Add(rootNode);

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
                    EnableCheckBox = true,
                    AutoPostBack = true
                };
                //加载子部门信息
                node.Nodes.Add(cNode);
                LoadUserNodes(cNode, o.sys_Member.ToList());
                LoadChildNodes(cNode);
            }
        }
        private void LoadUserNodes(TreeNode node, List<sys_Member> memberList)
        {
            string selectUserId = "";
            if (Grid2.SelectedRowIndex >= 0)
            {
                string mid = Grid2.DataKeys[Grid2.SelectedRowIndex][0].ToString();
                if (MenuOperRes.ContainsKey(mid))
                    selectUserId = MenuOperRes[mid] + ",";
            }

            foreach (sys_Member m in memberList)
            {
                if (m.IsDelete == false && m.IsUsing == true)
                {
                    var cNode = new TreeNode
                    {
                        Text = m.RealName,
                        NodeID = "P" + m.id.ToString(),
                        Icon = (bool)m.Sex ? Icon.User : Icon.UserFemale,
                        AutoPostBack = true,
                        EnableCheckBox = true,
                        Checked = selectUserId.Contains(m.id.ToString() + ",")
                    };
                    node.Nodes.Add(cNode);
                }
            }
        }

        private void CreateMenuOperRes()
        {

            if (Grid2.SelectedRowIndex >= 0)
            {
                string mid = Grid2.DataKeys[Grid2.SelectedRowIndex][0].ToString();
                Dictionary<string, string> _menuOperRes = MenuOperRes;
                TreeNode[] nodes = trUser.GetCheckedNodes();

                string OperResStr = "";
                foreach (TreeNode n in nodes)
                {
                    if (n.NodeID.Contains("P"))
                        OperResStr += n.NodeID.Replace("P", "") + ",";
                    if (_menuOperRes.ContainsKey(mid))
                        _menuOperRes[mid] = OperResStr.Trim(',');
                    else
                        _menuOperRes.Add(mid, OperResStr.Trim(','));
                }
                MenuOperRes = _menuOperRes;
            }
        }
        private void InitMenuOperRes()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            if (Role != null && Role.RoleOperResStr != null)
            {
                string roleOperResStr = Role.RoleOperResStr;
                foreach (string s in roleOperResStr.Split(','))
                {
                    if (!string.IsNullOrEmpty(s))
                    {
                        string[] ms = s.Split('-');
                        if (ms.Length == 2)
                        {
                            if (dic.ContainsKey(ms[0]))
                                dic[ms[0]] += "," + ms[1];
                            else
                                dic.Add(ms[0], ms[1]);
                        }
                    }
                }
            }
            MenuOperRes = dic;
        }

        #endregion

        #region 角色中用户
        private void BindUserGrid()
        {
            int RecordsCount;

            dynamic orderingSelector;
            Expression<Func<sys_Member, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_Member> list = MemberService.Where(predicate, Grid3.PageSize, Grid3.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid3.SortDirection), out RecordsCount);

            Grid3.RecordCount = RecordsCount;

            //绑定数据源
            Grid3.DataSource = list;
            Grid3.DataBind();

            ddlGridPageSize.SelectedValue = Grid3.PageSize.ToString(CultureInfo.InvariantCulture);

        }
        /// <summary>
        /// 创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<sys_Member, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(sys_Member));

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid3.SortField), parameter);

            DynamicLambda<sys_Member> bulider = new DynamicLambda<sys_Member>();
            Expression<Func<sys_Member, Boolean>> expr = null;

            Expression<Func<sys_Member, Boolean>> tmp = t => t.IsDelete == false;
            expr = bulider.BuildQueryAnd(expr, tmp);

            Expression<Func<sys_Member, Boolean>> tmp0 = t => t.IsUsing == true;
            expr = bulider.BuildQueryAnd(expr, tmp0);

            //查询角色中用户
            if (Grid1.SelectedRowIndex >= 0)
            {
                int rid = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndex][0]);
                sys_Role role = RoleService.FirstOrDefault(r => r.Id == rid);
                List<int> useridlist = new List<int>();
                foreach (sys_Member m in role.sys_Member)
                {
                    useridlist.Add(m.id);
                }
                Expression<Func<sys_Member, Boolean>> tmpSolid = t => useridlist.Contains(t.id);
                expr = bulider.BuildQueryAnd(expr, tmpSolid);
            }
            return expr;
        }
        #endregion



        #endregion






    }
}