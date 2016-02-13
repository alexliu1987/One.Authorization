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
    public partial class UserSelectForRole : PageBase
    {
        #region 定义
        protected int ActionId
        {
            get { return GetQueryIntValue("id"); }
        }
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
        ///获取  角色数据服务        
        /// </summary>
        protected RoleService RoleService
        {
            get { return _RoleService ?? (_RoleService = new RoleService(DbContext)); }
            set { _RoleService = value; }
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
        private MemberService _MemberService;
        protected MemberService MemberService
        {
            get { return _MemberService ?? (_MemberService = new MemberService(DbContext)); }
        }

        protected List<sys_Member> MemberList { get; set; }
        private int SelectOid
        {
            get
            {
                string SelectedNodeID = trOrgnization.SelectedNodeID;
                if (!string.IsNullOrEmpty(SelectedNodeID))
                    return Convert.ToInt32(trOrgnization.SelectedNodeID);
                return 0;
            }
        }
        private sys_Role _role;
        protected sys_Role Role
        {
            get { return _role ?? (_role = RoleService.FirstOrDefault(r => r.Id == ActionId)); }
        }
        #endregion

        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
        }

        protected void Grid1_PageIndexChange(object sender, FineUI.GridPageEventArgs e)
        {
            Grid1.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void Grid1_Sort(object sender, FineUI.GridSortEventArgs e)
        {
            Grid1.SortField = e.SortField;
            Grid1.SortDirection = e.SortDirection;
            BindGrid();
        }

        protected void trOrgnization_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            BindGrid();
        }

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (Grid1.SelectedRowIndexArray.Length <= 0)
            {
                Alert.Show("请选择用户！", MessageBoxIcon.Warning);
                return;
            }
            if (Role == null)
            {
                Alert.Show("参数初始化失败，请重试！", MessageBoxIcon.Error);
                return;
            }
            List<int> uidList = new List<int>();
            Grid1.SelectedRowIndexArray.ToList().ForEach(i => uidList.Add(Convert.ToInt32(Grid1.DataKeys[i][0])));
            List<sys_Member> memberlist = MemberService.Where(m => uidList.Contains(m.id)).ToList();
            List<sys_Role> roleList = new List<sys_Role>();
            string realnamestr = "";
            foreach (sys_Member m in memberlist)
            {
                realnamestr += m.RealName + ",";
                m.sys_Role.Add(Role);
            }
            if (MemberService.SaveChanges() > 0)
            {
                Log(LogType.新增, string.Format("添加用户到{0}角色中：{1}", Role.RoleName, realnamestr.Trim(',')), "角色管理");
                PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
            }
            else
                Alert.Show("添加用户到角色失败！", MessageBoxIcon.Error);
        }
        protected void ttbSearch_Trigger1Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = false;
            ttbSearch.Text = "";
            BindGrid();
        }

        protected void ttbSearch_Trigger2Click(object sender, EventArgs e)
        {
            ttbSearch.ShowTrigger1 = true;
            BindGrid();
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            if (ActionId <= 0)
            {
                Response.Write("参数传递失败，请重试！");
                Response.End();
                return;
            }
            Init_Tree();
            BindGrid();
        }
        private void Init_Tree()
        {
            trOrgnization.Nodes.Clear();

            var rootNode = new TreeNode
            {
                Text = ConfigHelper.GetAppSettingString("CorpName"),
                NodeID = "0",
                Expanded = true,
                EnablePostBack = true
            };

            trOrgnization.Nodes.Add(rootNode);

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
        private void BindGrid()
        {
            //数据总记录
            int RecordsCount;

            dynamic orderingSelector;
            Expression<Func<sys_Member, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_Member> list = MemberService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection), out RecordsCount);

            Grid1.RecordCount = RecordsCount;

            //绑定数据源
            Grid1.DataSource = list;
            Grid1.DataBind();

            ddlGridPageSize.SelectedValue = Grid1.PageSize.ToString(CultureInfo.InvariantCulture);
        }
        /// <summary>
        /// 创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<sys_Member, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            DynamicLambda<sys_Member> bulider = new DynamicLambda<sys_Member>();
            Expression<Func<sys_Member, Boolean>> expr = null;

            if (Role != null)
            {
                List<sys_Member> userlist = Role.sys_Member.ToList();
                List<int> uid = new List<int>();
                userlist.ForEach(i => uid.Add(i.id));
                Expression<Func<sys_Member, Boolean>> tmp = t => !uid.Contains(t.id);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }
            //未删除用户
            Expression<Func<sys_Member, Boolean>> tmp5 = t => t.IsDelete == false;
            expr = bulider.BuildQueryAnd(expr, tmp5);
            //未禁用用户
            Expression<Func<sys_Member, Boolean>> tmp6 = t => t.IsUsing == true;
            expr = bulider.BuildQueryAnd(expr, tmp6);

            if (!string.IsNullOrWhiteSpace(ttbSearch.Text))
            {
                Expression<Func<sys_Member, Boolean>> tmp2 = t => t.UserName.Contains(ttbSearch.Text.Trim());
                expr = bulider.BuildQueryAnd(expr, tmp2);

                Expression<Func<sys_Member, Boolean>> tmp3 = t => t.RealName.Contains(ttbSearch.Text.Trim());
                expr = bulider.BuildQueryOr(expr, tmp3);

                Expression<Func<sys_Member, Boolean>> tmp4 = t => t.Email.Contains(ttbSearch.Text.Trim());
                expr = bulider.BuildQueryOr(expr, tmp4);
            }

            if (SelectOid != 0)
            {
                List<int> oidList = GetAllChildOrgnizationId(SelectOid);
                Expression<Func<sys_Member, Boolean>> tmp = t => oidList.Contains(t.sys_Orgnization_Id);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }



            ParameterExpression parameter = Expression.Parameter(typeof(sys_Member));

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return expr;
        }
        private List<int> GetAllChildOrgnizationId(int poid)
        {
            List<int> oid = new List<int>();
            oid.Add(poid);
            foreach (int i in OrgnizationService.Where(p => p.PId == poid && p.IsDelete == false).Select(p => p.Id).ToList())
            {
                oid.Add(i);
                GetAllChildOrgnizationId(i);
            }
            return oid;
        }
        #endregion



    }
}