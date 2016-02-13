using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;
using Authorization.Framework.DataBase;
using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
using Authorization.Framework.Enum;
using Authorization.Framework.EntityRepository;
using SSO.Common.SSO;

namespace Authorization.Web.sys.User
{
    public partial class UserList : PageBase
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
        private MemberService _MemberService;
        protected MemberService MemberService
        {
            get { return _MemberService ?? (_MemberService = new MemberService()); }
        }
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
        #endregion

        #region 事件
        protected override void Init_Page()
        {
            BaseToolBar = Toolbar1;
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
                LoadData();
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
                    EditClick();
                    break;
                case AuthorizationButtonType.DELETE:
                    DeleteClick();
                    break;
                case AuthorizationButtonType.EXPORT:
                    ExportClick();
                    break;
                case AuthorizationButtonType.RESETPASSWORD:
                    ReSetPasswordClick();
                    break;
                case AuthorizationButtonType.FORBID:
                    ForbidClick();
                    break;
                case AuthorizationButtonType.RIGHT:
                    RightClick();
                    break;
            }

        }
        protected void trOrgnization_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            BindGrid();
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
        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
            BindGrid();
        }
        protected void Window1_Close(object sender, FineUI.WindowCloseEventArgs e)
        {
            BindGrid();
        }
        protected override void TwinTriggerBoxClick1(object sender, System.EventArgs e)
        {
            BASEttb = (TwinTriggerBox)sender;
            if (BASEttb != null)
            {
                BASEttb.Text = "";
                BASEttb.ShowTrigger1 = false;
                BindGrid();
            }
        }
        protected override void TwinTriggerBoxClick2(object sender, System.EventArgs e)
        {
            BASEttb = (TwinTriggerBox)sender;
            if (BASEttb != null)
            {
                BASEttb.ShowTrigger1 = true;
                BindGrid();
            }
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            Init_Tree();
            BindGrid();
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
            DynamicLambda<sys_Member> bulider = new DynamicLambda<sys_Member>();
            Expression<Func<sys_Member, Boolean>> expr = null;

            Expression<Func<sys_Member, Boolean>> tmp1 = t => t.IsDelete == false;
            expr = bulider.BuildQueryAnd(expr, tmp1);

            if (BASEttb != null && !string.IsNullOrWhiteSpace(BASEttb.Text))
            {
                Expression<Func<sys_Member, Boolean>> tmp2 = t => t.UserName.Contains(BASEttb.Text.Trim());
                expr = bulider.BuildQueryAnd(expr, tmp2);

                Expression<Func<sys_Member, Boolean>> tmp3 = t => t.RealName.Contains(BASEttb.Text.Trim());
                expr = bulider.BuildQueryOr(expr, tmp3);

                Expression<Func<sys_Member, Boolean>> tmp4 = t => t.Email.Contains(BASEttb.Text.Trim());
                expr = bulider.BuildQueryOr(expr, tmp4);
            }

            if (SelectOid != 0)
            {
                List<int> oidList = GetAllChildOrgnizationId(SelectOid);
                Expression<Func<sys_Member, Boolean>> tmp = t => oidList.Contains(t.sys_Orgnization_Id);
                expr = bulider.BuildQueryAnd(expr, tmp);
            }

            // 排序表达式
            ParameterExpression parameter = Expression.Parameter(typeof(sys_Member));
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return expr;
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
        private void AddClick()
        {
            PageContext.RegisterStartupScript(
                     Window1.GetShowReference(string.Format("UserEdit.aspx?oid={0}&action=1", SelectOid), "添加用户"));

        }
        private void EditClick()
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    int id = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("UserEdit.aspx?id={0}&action=2", id), "编辑用户信息"));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败，请重试！", MessageBoxIcon.Warning);
            }
        }

        private void DeleteClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<int> selectIds = GetSelectIds();
            bool isContainAdmin = false;
            try
            {
                IQueryable<sys_Member> list = MemberService.Where(p => selectIds.Contains(p.id));
                foreach (sys_Member s in list)
                {
                    if (s.UserName != "admin")
                    {
                        s.IsDelete = true;

                        s.ModifyDate = DateTime.Now;
                        s.ModifyUserName = BaseUserName;
                        s.ModifyRealName = BaseRealName;
                    }
                    else
                        isContainAdmin = true;
                }
                MemberService.SaveChanges();
                if (!isContainAdmin)
                    Alert.Show("删除成功！", MessageBoxIcon.Information);
                else
                    Alert.Show("删除成功,其中admin[系统管理员]不能删除！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        private void ReSetPasswordClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<int> selectIds = GetSelectIds();
            try
            {
                IQueryable<sys_Member> list = MemberService.Where(p => selectIds.Contains(p.id));
                foreach (sys_Member s in list)
                {
                    s.Password = DESEncrypt.Encrypt("123", s.UserName);
                }
                MemberService.SaveChanges();

                Alert.Show("密码重置成功！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("密码重置失败！", MessageBoxIcon.Warning);
            }
        }
        private IEnumerable<int> GetSelectIds()
        {
            int[] selections = Grid1.SelectedRowIndexArray;

            var selectIds = new int[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Int32.Parse(Grid1.DataKeys[selections[i]][0].ToString());
            }
            return selectIds;
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
        private void ExportClick()
        {
            dynamic orderingSelector;
            Expression<Func<sys_Member, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_Member> list = MemberService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new MemberExcel
                            {
                                用户名 = q.UserName,
                                姓名 = q.RealName,
                                邮箱 = q.Email,
                                QQ = q.sys_MemberExtend.QQ,
                                身份证 = q.sys_MemberExtend.IdCard,
                                生日 = q.sys_MemberExtend.Birthday,
                                手机 = q.sys_MemberExtend.Tel,
                                性别 = (bool)q.Sex ? "男" : "女"
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "用户信息", "用户信息" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        private void ForbidClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<int> selectIds = GetSelectIds();
            try
            {
                IQueryable<sys_Member> list = MemberService.Where(p => selectIds.Contains(p.id));
                foreach (sys_Member s in list)
                {
                    if ((bool)s.IsUsing)
                        s.IsUsing = false;
                    else
                        s.IsUsing = true;

                    s.ModifyDate = DateTime.Now;
                    s.ModifyUserName = BaseUserName;
                    s.ModifyRealName = BaseRealName;
                }
                MemberService.SaveChanges();

                Alert.Show("用户禁用或启用成功！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("用户禁用或启用失败！", MessageBoxIcon.Warning);
            }
        }
        private void RightClick()
        {
            try
            {
                if (Grid1.SelectedRowIndexArray.Length == 0)
                {
                    Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                }
                else if (Grid1.SelectedRowIndexArray.Length > 1)
                {
                    Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                }
                else
                {
                    int id = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                    PageContext.RegisterStartupScript(
                        Window1.GetShowReference(string.Format("UserRight.aspx?id={0}", id), "用户授权") + Window1.GetMaximizeReference());
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败，请重试！", MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}