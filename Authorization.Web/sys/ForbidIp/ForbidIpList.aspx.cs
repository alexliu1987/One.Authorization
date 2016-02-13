using System;
using System.Data;
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

namespace Authorization.Web.sys.ForbidIp
{
    public partial class ForbidIpList : PageBase
    {
        #region 定义
        /// <summary>
        /// 数据服务
        /// </summary>
        private ForbidIpService _ForbidIpService;
        /// <summary>
        /// 获取设置 数据服务
        /// </summary>
        protected ForbidIpService ForbidIpService
        {
            get { return _ForbidIpService ?? (_ForbidIpService = new ForbidIpService()); }
            set { _ForbidIpService = value; }
        }
        private TwinTriggerBox ttb { get; set; }
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
                    AddClick(Window1, "ForbidIpEdit", "新增IP黑名单");
                    break;
                case AuthorizationButtonType.DELETE:
                    DeleteClick();
                    break;
                case AuthorizationButtonType.EXPORT:
                    ExportClick();                   
                    break;
                case AuthorizationButtonType.FORBID:                  
                    StopOrStartClick();
                    break;
            }

        }



        protected override void TwinTriggerBoxClick1(object sender, System.EventArgs e)
        {
            ttb = (TwinTriggerBox)sender;
            if (ttb != null)
            {
                ttb.Text = "";
                ttb.ShowTrigger1 = false;
                BindGrid();
            }
        }
        protected override void TwinTriggerBoxClick2(object sender, System.EventArgs e)
        {
            ttb = (TwinTriggerBox)sender;
            if (ttb != null)
            {
                ttb.ShowTrigger1 = true;
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
            Expression<Func<sys_ForbidIp, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_ForbidIp> list = ForbidIpService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_ForbidIp, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            DynamicLambda<sys_ForbidIp> bulider = new DynamicLambda<sys_ForbidIp>();
            Expression<Func<sys_ForbidIp, Boolean>> expr = null;

            Expression<Func<sys_ForbidIp, Boolean>> tmp1 = t => t.IsDelete == false;
            expr = bulider.BuildQueryAnd(expr, tmp1);

            if (ttb != null && !string.IsNullOrWhiteSpace(ttb.Text))
            {
                Expression<Func<sys_ForbidIp, Boolean>> tmp2 = t => t.IP.Contains(ttb.Text.Trim());
                expr = bulider.BuildQueryAnd(expr, tmp2);                 
            }


            // 排序表达式
            ParameterExpression parameter = Expression.Parameter(typeof(sys_ForbidIp));
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
                IQueryable<sys_ForbidIp> list = ForbidIpService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_ForbidIp s in list)
                {
                    s.IsDelete = true;
                    deletelist += s.IP + ",";
                }
                ForbidIpService.SaveChanges();
                Log(LogType.删除, string.Format("删除IP黑名单：{0}", deletelist.Trim(',')), "IP黑名单");
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
            Expression<Func<sys_ForbidIp, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_ForbidIp> list = ForbidIpService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new ForbidIpExcel
                            {
                                IP = q.IP,
                                是否可用 = q.IsUsing,
                                描述 = q.Describe,
                                操作时间 = (DateTime)q.CreateDate,
                                操作人 = q.CreateRealName
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "IP黑名单", "IP黑名单" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        private void StopOrStartClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                IQueryable<sys_ForbidIp> list = ForbidIpService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_ForbidIp s in list)
                {
                    if (s.IsUsing)
                        s.IsUsing = false;
                    else
                        s.IsUsing = true;
                    deletelist += s.IP + ",";
                }
                ForbidIpService.SaveChanges();
                Log(LogType.修改, string.Format("禁用或启用IP黑名单：{0}", deletelist.Trim(',')), "IP黑名单");
                Alert.Show("禁用或启用成功！", MessageBoxIcon.Information);
                BindGrid();
            }
            catch (Exception)
            {
                Alert.Show("禁用或启用失败！", MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}