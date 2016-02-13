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

namespace Authorization.Web.sys.Action
{
    public partial class ActionList : PageBase
    {
        #region 定义
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private ActionService _ActionService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected ActionService ActionService
        {
            get { return _ActionService ?? (_ActionService = new ActionService()); }
            set { _ActionService = value; }
        }
        private TwinTriggerBox ttb { get; set; }
        /// <summary>
        /// 获取设置 模块数据服务
        /// </summary>
        private AppService _AppService;
        protected AppService AppService
        {
            get { return _AppService ?? (_AppService = new AppService()); }
            set { _AppService = value; }
        }
        /// <summary>
        /// 模块列表
        /// </summary>
        private List<sys_App> _appList;
        protected List<sys_App> AppList
        {
            get { return _appList ?? (_appList = AppService.Where(a => a.IsDelete == false).ToList()); }
            set { _appList = value; }
        }
        private int SelectAppid
        {
            get
            {
                string SelectedNodeID = trApp.SelectedNodeID;
                if (!string.IsNullOrEmpty(SelectedNodeID))
                    return Convert.ToInt32(trApp.SelectedNodeID);
                return 0;
            }
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
                case AuthorizationButtonType.ADD:
                    AddClick();
                    break;
                case AuthorizationButtonType.EDIT:
                    EditClick(Grid1, Window1, "ActionEdit", "编辑功能操作");
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
        protected void trApp_NodeCommand(object sender, FineUI.TreeCommandEventArgs e)
        {
            BindGrid();
        }

        #endregion

        #region 方法
        private void LoadData()
        {
            //btnAdd.OnClientClick = Window1.GetShowReference("ActionEdit.aspx?action=1", "添加功能");
            Init_Tree();
            BindGrid();
        }
        private void Init_Tree()
        {
            trApp.Nodes.Clear();

            foreach (sys_App a in AppList)
            {
                var cNode = new TreeNode
                {
                    Text = a.AppName,
                    NodeID = a.Id.ToString(),
                    IconUrl = a.Ico,
                    Expanded = true,
                    EnablePostBack = true

                };
                trApp.Nodes.Add(cNode);
            }
        }
        private void BindGrid()
        {
            //数据总记录
            int RecordsCount;

            dynamic orderingSelector;
            Expression<Func<sys_Action, bool>> predicate = BuildPredicate(out orderingSelector);

            //取数据源
            IQueryable<sys_Action> list = ActionService.Where(predicate, Grid1.PageSize, Grid1.PageIndex + 1,
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
        private Expression<Func<sys_Action, bool>> BuildPredicate(out dynamic orderingSelector)
        {
            // 查询条件表达式
            Expression expr = Expression.Constant(true);

            ParameterExpression parameter = Expression.Parameter(typeof(sys_Action));
            MethodInfo methodInfo = typeof(string).GetMethod("Contains", new[] { typeof(string) });

            //取未删除的数据
            expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "isDelete"), Expression.Constant(false, typeof(bool?))));

            //按名称模糊查询
            if (ttb != null && !string.IsNullOrWhiteSpace(ttb.Text))
            {
                expr = Expression.And(expr, Expression.Call(Expression.Property(parameter, "ActionName"), methodInfo,
                      Expression.Constant(ttb.Text.Trim())));

            }

            if (SelectAppid != 0)
            {
                expr = Expression.And(expr,
                Expression.Equal(Expression.Property(parameter, "AppId"), Expression.Constant(SelectAppid, typeof(int))));
            }

            Expression<Func<sys_Action, bool>> predicate = Expression.Lambda<Func<sys_Action, bool>>(expr, parameter);

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
            PageContext.RegisterStartupScript(
                     Window1.GetShowReference(string.Format("ActionEdit.aspx?aid={0}&action=1", SelectAppid), "新增功能操作"));

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
                IQueryable<sys_Action> list = ActionService.Where(p => selectIds.Contains(p.Id));
                string deletelist = "";
                foreach (sys_Action s in list)
                {
                    s.IsDelete = true;

                    s.ModifyDate = DateTime.Now;
                    s.ModifyUserName = BaseUserName;
                    s.ModifyRealName = BaseRealName;

                    deletelist += s.ActionName + ",";
                }
                ActionService.SaveChanges();
                Log(LogType.删除, string.Format("删除功能：{0}", deletelist.Trim(',')), "功能管理");
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
            Expression<Func<sys_Action, bool>> predicate = BuildPredicate(out orderingSelector);
            IQueryable<sys_Action> list = ActionService.Where(predicate, orderingSelector, EnumHelper.ParseEnumByString<OrderingOrders>(Grid1.SortDirection));
            var excelData = from q in list
                            select new ActionExcel
                            {
                                功能名称 = q.ActionName,
                                功能标识 = q.ActionCode,
                                图标 = q.ActionIco,
                                按钮类型 = q.ActionType,
                                排序 = q.SortIndex,
                                是否可用 = q.IsUsing,
                                描述 = q.Describe
                            };
            Authorization.Framework.Npoi.NPOIHelper.ExportByWeb(excelData.ToList(), "功能操作", "功能操作" + DateTime.Now.ToString("yyyyMMddhhmmss"));

        }
        #endregion
    }
}