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
    public partial class LoginLog : PageBase
    {
        #region 定义
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private LoginLogService _LoginLogService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected LoginLogService LoginLogService
        {
            get { return _LoginLogService ?? (_LoginLogService = new LoginLogService()); }
            set { _LoginLogService = value; }
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
                case AuthorizationButtonType.FORBIDIP:
                    ForbidIpClick();
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
            Expression<Func<sys_LoginLog, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_LoginLog> list = LoginLogService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_LoginLog, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            DynamicLambda<sys_LoginLog> bulider = new DynamicLambda<sys_LoginLog>();
            Expression<Func<sys_LoginLog, Boolean>> expr = null;

            Expression<Func<sys_LoginLog, Boolean>> tmp = t => BaseOperResUidList.Contains(t.Uid);
            expr = bulider.BuildQueryAnd(expr, tmp);

            if (BASEttb != null && !string.IsNullOrWhiteSpace(BASEttb.Text))
            {
                Expression<Func<sys_LoginLog, Boolean>> tmp1 = t => t.RealName.Contains(BASEttb.Text);
                expr = bulider.BuildQueryAnd(expr, tmp1);
            }

            ParameterExpression parameter = Expression.Parameter(typeof(sys_LoginLog));
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
                LoginLogService.Delete(p => selectIds.Contains(p.Id));

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
            Expression<Func<sys_LoginLog, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_LoginLog> list = LoginLogService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new LoginLogExcel
                            {
                                登录时间 = q.LoginTime,
                                用户编号 = q.Uid,
                                姓名 = q.RealName,
                                IP地址 = q.Ip
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "登录日志", "登录日志" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        private void ForbidIpClick()
        {
            if (Grid1.SelectedRowIndexArray.Length == 0)
            {
                Alert.Show("请至少选择一项！", MessageBoxIcon.Information);
                return;
            }
            else if (Grid1.SelectedRowIndexArray.Length > 1)
            {
                Alert.Show("只能选择一项！", MessageBoxIcon.Information);
                return;
            }

            try
            {
                int id = Convert.ToInt32(Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][0]);
                var log = LoginLogService.FirstOrDefault(p => p.Id == id);
                var server = new ForbidIpService();
                if (server.FirstOrDefault(p => p.IP == log.Ip&&p.IsDelete==false) != null)
                {
                    Alert.Show("此IP已在黑名单中，请勿重复添加！", MessageBoxIcon.Information);
                    return;
                }
                var ip = new sys_ForbidIp
                {
                    IP = log.Ip,
                    IsDelete = false,
                    IsUsing = true,
                    CreateDate = DateTime.Now,
                    CreateUserName = BaseUserName,
                    CreateRealName = BaseRealName,
                    Describe = ""
                };
                server.Add(ip);
                Log(LogType.新增, "添加IP到黑名单：" + ip.IP, "IP黑名单");
                Alert.Show("添加成功！");
            }
            catch (Exception)
            {
                Alert.Show("删除失败！", MessageBoxIcon.Warning);
            }
        }
        #endregion
    }
}