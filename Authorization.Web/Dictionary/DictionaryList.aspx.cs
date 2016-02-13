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

namespace Authorization.Web.Dictionary
{
    public partial class DictionaryList : PageBase
    {

        #region 定义
        /// <summary>
        /// 字典数据服务
        /// </summary>
        private DictionaryService _DictionaryService;
        /// <summary>
        /// 获取设置 字典数据服务
        /// </summary>
        protected DictionaryService DictionaryService
        {
            get { return _DictionaryService ?? (_DictionaryService = new DictionaryService()); }
            set { _DictionaryService = value; }
        }
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
        protected void Grid2_Sort(object sender, GridSortEventArgs e)
        {
            Grid2.SortField = e.SortField;
            Grid2.SortDirection = e.SortDirection;
            BindGrid2();
        }

        protected void Grid2_RowClick(object sender, GridRowClickEventArgs e)
        {
            BindGrid();
        }

        #endregion

        #region 方法
        private void LoadData()
        {
            BindGrid2();
            if (Grid2.Rows.Count > 0)
                Grid2.SelectedRowIndex = 0;
            BindGrid();
        }
        private void BindGrid()
        {
            //数据总记录
            int RecordsCount;

            dynamic orderingSelector;
            Expression<Func<sys_Dictionary, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_Dictionary> list = DictionaryService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_Dictionary, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(sys_Dictionary));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //取未删除的数据
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "isDelete"), Expression.Constant(false, typeof(bool?))));

            if (Grid2.SelectedRowIndex >= 0)
            {
                expr = Expression.And(expr,
                   Expression.Equal(Expression.Property(parameter, "sys_DictionaryType_Id"), Expression.Constant(Convert.ToInt32(Grid2.DataKeys[Grid2.SelectedRowIndex][0]), typeof(int))));
            }

            //按名称查询
            if (ttb != null && !string.IsNullOrWhiteSpace(ttb.Text))
            {
                expr = Expression.And(expr,
                   Expression.Call(Expression.Property(parameter, "Value"), methodInfo, Expression.Constant(ttb.Text.Trim())));
            }

            Expression<Func<sys_Dictionary, bool>> predicate = Expression.Lambda<Func<sys_Dictionary, bool>>(expr, parameter);

            // 排序表达式
            orderingSelector = Expression.Lambda(Expression.Property(parameter, Grid1.SortField), parameter);

            return predicate;
        }
        private void BindGrid2()
        {
            //数据总记录
            int RecordsCount;

            dynamic orderingSelector;
            Expression<Func<sys_DictionaryType, bool>> predicate = BuildPredicate2(out orderingSelector);

            //取数据源
            IQueryable<sys_DictionaryType> list = DictionaryTypeService.Where(predicate, Grid2.PageSize, Grid2.PageIndex + 1,
                orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid2.SortDirection), out RecordsCount);

            Grid2.RecordCount = RecordsCount;

            //绑定数据源
            Grid2.DataSource = list;
            Grid2.DataBind();

        }
        /// <summary>
        /// 创建查询条件表达式和排序表达式
        /// </summary>
        /// <param name="orderingSelector"></param>
        /// <returns></returns>
        private Expression<Func<sys_DictionaryType, bool>> BuildPredicate2(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(sys_DictionaryType));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //取未删除的数据
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "isDelete"), Expression.Constant(false, typeof(bool?))));

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
        private void AddClick()
        {
            if (Grid2.SelectedRowIndex >= 0)
            {
                PageContext.RegisterStartupScript(
                         Window1.GetShowReference(string.Format("DictionaryEdit.aspx?typeid={0}&action=1", Grid2.DataKeys[Grid2.SelectedRowIndex][0]), "添加字典"));
            }
            else
                Alert.Show("请选择字典类型", MessageBoxIcon.Warning);
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
                        Window1.GetShowReference(string.Format("DictionaryEdit.aspx?id={0}&action=2&typeid={1}", id, Grid1.DataKeys[Grid1.SelectedRowIndexArray[0]][1]), "编辑字典"));
                }
            }
            catch (Exception)
            {
                Alert.Show("编辑失败，请重试！", MessageBoxIcon.Warning);
            }
        }
        private void DeleteClick()
        {
            IEnumerable<int> selectIds = GetSelectIds();

            try
            {
                IQueryable<sys_Dictionary> list = DictionaryService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_Dictionary s in list)
                {
                    s.IsDelete = true;

                    s.ModifyDate = DateTime.Now;
                    s.ModifyUserName = BaseUserName;
                    s.ModifyRealName = BaseRealName;
                    deletelist += s.Value + ",";
                }
                DictionaryService.SaveChanges();
                Log(LogType.删除, string.Format("删除字典：{0}", deletelist.Trim(',')), "字典管理");
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
            Expression<Func<sys_Dictionary, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_Dictionary> list = DictionaryService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new DictionaryExcel
                            {
                                字典值 = q.Value,
                                字典类型 = q.sys_DictionaryType.TypeName,
                                排序 = q.SortIndex,
                                描述 = q.Describe,
                                创建时间 = q.CreateDate

                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "系统字典定制", "字典定制" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        #endregion




    }
}