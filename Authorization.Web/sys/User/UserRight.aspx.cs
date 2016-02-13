using System;
using System.Configuration;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
using Authorization.Framework.Enum;
using Authorization.Framework.DataBase;
using Authorization.Framework.EntityRepository;

namespace Authorization.Web.sys.User
{
    public partial class UserRight : PageBase
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
        private string _RoleOperResStr;
        private string RoleOperResStr
        {
            get { return _RoleOperResStr ?? (_RoleOperResStr = CreateRoleOperResStr()); }
        }
        protected int ActionId
        {
            get { return GetQueryIntValue("id"); }
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        private sys_Member _Member;
        protected sys_Member Member
        {
            get { return _Member ?? (_Member = MemberService.FirstOrDefault(p => p.id == ActionId)); }
        }
        private string _roleRightStr;
        private string RoleRightStr
        {
            get { return _roleRightStr ?? (_roleRightStr = CreateRoleRightStr()); }
        }
        #endregion

        #region 事件
        protected override void Init_Page()
        {
            BasePageName = "UserList";//继续UserList.aspx页面权限
            BasePage = this.Page;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        protected void Grid2_RowDataBound(object sender, FineUI.GridRowEventArgs e)
        {
            string roleRightStr = "";
            string userRightStr = "";
            if (Member != null)
            {
                roleRightStr = RoleRightStr;
                userRightStr = Member.RoleRightStr ?? "";
            }

            System.Web.UI.WebControls.CheckBoxList cblpower = (System.Web.UI.WebControls.CheckBoxList)Grid2.Rows[e.RowIndex].FindControl("cblPowers");
            string mid = Grid2.DataKeys[e.RowIndex][0].ToString();
            sys_Menu m = mList.Where(p => p.Id == mid).FirstOrDefault();

            //添加浏览
            System.Web.UI.WebControls.ListItem item0 = new System.Web.UI.WebControls.ListItem();
            item0.Text = "浏览";
            item0.Value = mid + "-0";
            item0.Attributes["data-qtip"] = mid;

            //初始化浏览权限-角色
            if (roleRightStr.Contains(item0.Value))
            {
                item0.Selected = true;
                item0.Enabled = false;
            }
            else
                item0.Selected = false;
            //用户浏览权限
            if (userRightStr.Contains(item0.Value))
                item0.Selected = true;

            cblpower.Items.Add(item0);
            foreach (sys_Action a in m.sys_Action)
            {
                System.Web.UI.WebControls.ListItem item = new System.Web.UI.WebControls.ListItem();
                item.Text = a.ActionName;
                item.Value = string.Format("{0}-{1}", mid, a.Id);
                item.Attributes["data-qtip"] = mid;
                //角色权限
                if (roleRightStr.Contains(item.Value))
                {
                    item.Selected = true;
                    item.Enabled = false;
                }
                else
                {
                    item.Selected = false;
                    item.Enabled = true;
                }
                //用户权限
                if (userRightStr.Contains(item.Value))
                    item.Selected = true;
                cblpower.Items.Add(item);
            }
        }

        protected void Grid2_RowClick(object sender, FineUI.GridRowClickEventArgs e)
        {
            if (!Convert.ToBoolean(Grid2.DataKeys[Grid2.SelectedRowIndex][1]))
                trUser.Hidden = true;
            else
                trUser.Hidden = false;
        }

        protected void lbtnOperRes_Click(object sender, EventArgs e)
        {
            InitTree();
        }

        protected void trUser_NodeCheck(object sender, FineUI.TreeCheckEventArgs e)
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
        protected void btnRight_Click(object sender, EventArgs e)
        {
            string UserRightStr = "";
            for (int i = 0; i < Grid2.Rows.Count; i++)
            {
                System.Web.UI.WebControls.CheckBoxList cblPowers = (System.Web.UI.WebControls.CheckBoxList)Grid2.Rows[i].FindControl("cblPowers");
                foreach (System.Web.UI.WebControls.ListItem item in cblPowers.Items)
                {
                    if (item.Enabled && item.Selected)
                    {
                        UserRightStr += item.Value + ",";
                    }
                }
            }
            string UserOperResStr = "";
            foreach (KeyValuePair<string, string> kvp in MenuOperRes)
            {
                UserOperResStr += string.Format("{0}-{1}", kvp.Key, kvp.Value.Replace(",", string.Format(",{0}-", kvp.Key))) + ",";
            }
            Member.RoleRightStr = UserRightStr.Trim(',');//此处为用户权限 建表时复制的 忘记修改 实为RightStr
            Member.RoleOperResStr = RemoveRoleOperResForUser(UserOperResStr.Trim(','));//此处为用户数据权限  实为OperRes
            if (MemberService.SaveChanges() >= 0)
            {
                Log(LogType.授权, string.Format("用户授权：{0}", Member.RealName), "用户管理");

                Alert.Show("用户权限保存成功！");
            }
            else
                Alert.Show("用户权限保存失败！", MessageBoxIcon.Error);
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            if (!CheckQueryKey("id"))
            {
                Response.Write("参数传递失败！");
                Response.End();
                return;
            }
            BindMenuGrid();
            InitMenuOperRes();

        }
        private void BindMenuGrid()
        {
            List<MenuInfo> miList = new List<MenuInfo>();
            foreach (sys_Menu m in mList.Where(p => p.PId == "0").OrderBy(p => p.SortIndex))
            {
                MenuInfo mi = new MenuInfo();
                mi.Mid = m.Id;
                mi.AppId = m.AppId;
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
            if (Member != null)
            {
                //初始化用户数据权限
                if (Member.RoleOperResStr != null)
                {
                    string UserOperResStr = Member.RoleOperResStr;
                    foreach (string s in UserOperResStr.Split(','))
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
                //初始化角色数据权限
                if (Member.sys_Role != null)
                {
                    foreach (sys_Role r in Member.sys_Role)
                    {
                        if (r.RoleOperResStr != null)
                        {
                            foreach (string s in r.RoleOperResStr.Split(','))
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
                    }

                }
            }
            MenuOperRes = dic;
        }
        private string CreateRoleRightStr()
        {
            string role_right_str = "";
            if (Member != null)
            {
                foreach (sys_Role r in Member.sys_Role)
                {
                    if (r.RoleRightStr != null)
                    {
                        role_right_str += r.RoleRightStr + ",";
                    }
                }
            }
            return role_right_str.Trim(',');
        }
        private string CreateRoleOperResStr()
        {
            string role_operres_str = "";
            if (Member != null)
            {
                foreach (sys_Role r in Member.sys_Role)
                {
                    if (r.RoleOperResStr != null)
                    {
                        role_operres_str += r.RoleOperResStr + ",";
                    }
                }
            }
            return role_operres_str.Trim(',');
        }
        /// <summary>
        /// 去除用户数据权限中的角色数据权限
        /// </summary>
        /// <param name="user_oper_res">用户数据权限</param>
        /// <returns></returns>
        private string RemoveRoleOperResForUser(string user_oper_res)
        {
            string re = "";
            foreach (string s in user_oper_res.Split(','))
            {
                if (!RoleOperResStr.Contains(s))
                    re += s + ",";
            }
            return re.Trim(',');
        }
        #endregion


    }
}