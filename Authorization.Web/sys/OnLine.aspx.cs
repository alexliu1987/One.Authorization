using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;

using FineUI;
using Authorization.Common;
using Authorization.Model;
using Authorization.Services;
using Authorization.Framework.Enum;
using Authorization.Framework.EntityRepository;

namespace Authorization.Web.sys
{
    public partial class OnLine : PageBase
    {
        #region 定义
        /// <summary>
        /// 用户数据服务
        /// </summary>
        private MemberService _MemberService;
        /// <summary>
        /// 获取设置 用户数据服务
        /// </summary>
        protected MemberService MemberService
        {
            get { return _MemberService ?? (_MemberService = new MemberService()); }
            set { _MemberService = value; }
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

        protected void ddlGridPageSize_SelectedIndexChanged(object sender, EventArgs e)
        {
            Grid1.PageSize = Convert.ToInt32(ddlGridPageSize.SelectedValue);
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

            Expression<Func<sys_Member, Boolean>> tmp = t => t.sys_MemberExtend.IsOnline;
            expr = bulider.BuildQueryAnd(expr, tmp);

            if (BASEttb != null && !string.IsNullOrWhiteSpace(BASEttb.Text))
            {
                Expression<Func<sys_Member, Boolean>> tmp1 = t => t.RealName.Contains(BASEttb.Text);
                expr = bulider.BuildQueryAnd(expr, tmp1);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(sys_Member));
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return expr;
        }       
        #endregion
    }
}