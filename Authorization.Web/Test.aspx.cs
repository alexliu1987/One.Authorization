using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.IO;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.Globalization;

using FineUI;
using Authorization.Common;
using Authorization.Model;
using Authorization.Services;
using Authorization.Framework.Enum;
using Authorization.Framework.EntityRepository;
namespace Authorization.Web
{
    public partial class Test : PageBase
    {
        #region 定义
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private OperLogService _ActionService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected OperLogService ActionService
        {
            get { return _ActionService ?? (_ActionService = new OperLogService()); }
            set { _ActionService = value; }
        }
        #endregion

        #region 事件
        protected override void Init_Page()
        {
            BaseToolBar = Toolbar2;
            BaseWindow = Window2;

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
                case AuthorizationButtonType.ADD:


                    break;
                case AuthorizationButtonType.EDIT:

                case AuthorizationButtonType.DELETE:

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
        protected void Window1_Close(object sender, FineUI.WindowCloseEventArgs e)
        {
            BindGrid();
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
            IQueryable<sys_OperLog> list = ActionService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(sys_OperLog));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });



            //按名称查询


            Expression<Func<sys_OperLog, bool>> predicate = Expression.Lambda<Func<sys_OperLog, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return predicate;
        }
        private void ExportClick()
        {
            //Window2.Hidden = false;
            //dynamic orderingSelector;
            //Expression<Func<sys_OperLog, bool>> predicate = BuildPredicate(out orderingSelector);
            //IQueryable<sys_OperLog> list = ActionService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            ////var excelData = from q in list
            ////                select new DictionaryExcel
            ////                {
            ////                    字典值 = q.OperType,
            ////                    字典类型 = q.OperContent,
            ////                    排序 = q.Id,
            ////                    描述 = q.Address,
            ////                    创建时间 = q.LoginTime

            ////                };
            ////HttpContext hc = Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "系统字典定制", "字典定制" + DateTime.Now.ToString("yyyyMMddhhmmss"));

            //MemoryStream ms = Authorization.Framework.Npoi.NPOIHelper.Export(excelData.ToList(), "系统字典定制");
            //ViewState["EXCELFILE"] = ms.GetBuffer();
            ////HttpContext curContext = HttpContext.Current;

            ////// 设置编码和附件格式
            ////curContext.Response.ContentType = "application/vnd.ms-excel";
            ////curContext.Response.ContentEncoding = Encoding.UTF8;
            ////curContext.Response.Charset = "";
            ////curContext.Response.AppendHeader("Content-Disposition",
            ////    "attachment;filename=" + HttpUtility.UrlEncode("aaaa", Encoding.UTF8));
            ////Window2.Hidden = true;
            ////curContext.Response.BinaryWrite(ms.GetBuffer());
            ////curContext.Response.End();
            //lbMsg.Text = "数据准备完成，请点击下载！";
            //btnDown.Enabled = true;

        }
        #endregion

        protected void btnDown_Click(object sender, EventArgs e)
        {
            if (ViewState["EXCELFILE"] != null)
            {
                byte[] b = ViewState["EXCELFILE"] as byte[];
                HttpContext curContext = HttpContext.Current;

                // 设置编码和附件格式
                curContext.Response.ContentType = "application/vnd.ms-excel";
                curContext.Response.ContentEncoding = Encoding.UTF8;
                curContext.Response.Charset = "";
                curContext.Response.AppendHeader("Content-Disposition",
                    "attachment;filename=" + HttpUtility.UrlEncode("aaaa", Encoding.UTF8));
                Window2.Hidden = true;
                curContext.Response.BinaryWrite(b);
                curContext.Response.End();

            }
        }


    }
}