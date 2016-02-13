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

namespace Authorization.Web.sys.Log
{
    public partial class OperLog : PageBase
    {
        #region 定义
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private OperLogService _OperLogService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected OperLogService OperLogService
        {
            get { return _OperLogService ?? (_OperLogService = new OperLogService()); }
            set { _OperLogService = value; }
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
        protected override void BtnClick(object sender, System.EventArgs e)
        {
            Button btn = (Button)sender;

            switch ((AuthorizationButtonType)System.Enum.Parse(typeof(AuthorizationButtonType), btn.ID.Replace("btn", "").ToUpper()))
            {
                case AuthorizationButtonType.DELETE:
                    DeleteClick();
                    break;
                case AuthorizationButtonType.EXPORT:
                    ExportClick();
                    break;
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
            Expression<Func<sys_OperLog, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_OperLog> list = OperLogService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_OperLog, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            DynamicLambda<sys_OperLog> bulider = new DynamicLambda<sys_OperLog>();
            Expression<Func<sys_OperLog, Boolean>> expr = null;



            Expression<Func<sys_OperLog, Boolean>> tmp = t => BaseOperResUidList.Contains(t.Uid);
            expr = bulider.BuildQueryAnd(expr, tmp);

            if (BASEttb != null && !string.IsNullOrWhiteSpace(BASEttb.Text))
            {
                Expression<Func<sys_OperLog, Boolean>> tmp1 = t => t.RealName.Contains(BASEttb.Text);
                expr = bulider.BuildQueryAnd(expr, tmp1);
                Expression<Func<sys_OperLog, Boolean>> tmp2 = t => t.OperType.Contains(BASEttb.Text);
                expr = bulider.BuildQueryOr(expr, tmp2);
                Expression<Func<sys_OperLog, Boolean>> tmp3 = t => t.OperContent.Contains(BASEttb.Text);
                expr = bulider.BuildQueryOr(expr, tmp3);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(sys_OperLog));
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);


            return expr;
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


        private void DeleteClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                OperLogService.Delete(p => selectIds.Contains(p.Id));

                Alert.Show("删除成功！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        private void ExportClick()
        {
            dynamic orderingSelector;
            Expression<Func<sys_OperLog, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_OperLog> list = OperLogService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new OperLogExcel
                            {
                                操作时间 = q.LogTime,
                                用户编号 = q.Uid,
                                姓名 = q.RealName,
                                IP地址 = q.Ip,
                                操作类型 = q.OperType,
                                操作内容 = q.OperContent

                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "操作日志", "操作日志" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        #endregion
    }
}