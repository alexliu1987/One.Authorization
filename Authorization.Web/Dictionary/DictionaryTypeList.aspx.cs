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

namespace Authorization.Web.Dictionary
{
    public partial class DictionaryTyleList : PageBase
    {
        #region 定义
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private DictionaryTypeService _DictionaryTypeService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected DictionaryTypeService DictionaryTypeService
        {
            get { return _DictionaryTypeService ?? (_DictionaryTypeService = new DictionaryTypeService()); }
            set { _DictionaryTypeService = value; }
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
                    AddClick(Window1, "DictionaryTypeEdit", "新增字典类型");
                    break;
                case AuthorizationButtonType.EDIT:
                    EditClick(Grid1, Window1, "DictionaryTypeEdit", "编辑字典类型");
                    break;
                case AuthorizationButtonType.DELETE:
                    DeleteClick();
                    break;
                case AuthorizationButtonType.EXPORT:
                    ExportClick();
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
            Expression<Func<sys_DictionaryType, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_DictionaryType> list = DictionaryTypeService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_DictionaryType, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(sys_DictionaryType));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //取未删除的数据
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "isDelete"), Expression.Constant(false, typeof(bool?))));

            //按名称查询
            if (ttb != null && !string.IsNullOrWhiteSpace(ttb.Text))
            {
                expr = Expression.And(expr,
                   Expression.Call(Expression.Property(parameter, "TypeName"), methodInfo, Expression.Constant(ttb.Text.Trim())));
            }

            Expression<Func<sys_DictionaryType, bool>> predicate = Expression.Lambda<Func<sys_DictionaryType, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return predicate;
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
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                IQueryable<sys_DictionaryType> list = DictionaryTypeService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_DictionaryType s in list)
                {
                    s.IsDelete = true;

                    s.ModifyDate = DateTime.Now;
                    s.ModifyUserName = BaseUserName;
                    s.ModifyRealName = BaseRealName;
                    deletelist += s.TypeName + ",";
                }
                DictionaryTypeService.SaveChanges();
                Log(LogType.删除, string.Format("删除字典内容：{0}", deletelist.Trim(',')), "字典类型管理");
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
            Expression<Func<sys_DictionaryType, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_DictionaryType> list = DictionaryTypeService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new DictionaryTypeExcel
                            {
                                字典类型名称 = q.TypeName,
                                排序 = q.SortIndex,
                                是否可用 = q.IsUsing,
                                描述 = q.Describe
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "字典类型", "字典类型" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        #endregion

    }
}