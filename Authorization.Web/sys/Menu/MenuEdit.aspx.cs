using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.Entity;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;

using Authorization.Framework.Enum;
using Authorization.Framework.DataBase;
using Authorization.Framework.EntityRepository;

namespace Authorization.Web.sys.Menu
{
    public partial class MenuEdit : PageBase
    {
        #region 定义
        private DbContext _dbContext;
        private DbContext DbContext
        {
            get { return _dbContext ?? (_dbContext = DbContextHelper.CreateDbContext()); }
        }
        protected string ActionId
        {
            get { return GetQueryValue("id"); }
        }
        protected int AppId
        {
            get { return GetQueryIntValue("aid"); }
        }
        /// <summary>
        /// 字典信息
        /// </summary>
        private sys_Menu _sysMenu;
        /// <summary>
        /// 字典数据服务
        /// </summary>
        private ActionService _ActionService;
        /// <summary>
        /// 获取设置 字典数据服务
        /// </summary>
        protected ActionService ActionService
        {
            get { return _ActionService ?? (_ActionService = new ActionService(DbContext)); }
            set { _ActionService = value; }
        }
        /// <summary>
        /// 菜单数据服务
        /// </summary>
        private MenuService _MenuService;
        /// <summary>
        /// 获取设置 菜单数据服务
        /// </summary>
        protected MenuService MenuService
        {
            get { return _MenuService ?? (_MenuService = new MenuService(DbContext)); }
            set { _MenuService = value; }
        }
        /// <summary>
        /// 获取设置 字典信息
        /// </summary>
        protected sys_Menu sysMenu
        {
            get { return _sysMenu ?? (_sysMenu = MenuService.FirstOrDefault(p => p.Id == ActionId)); }
            set { _sysMenu = value; }
        }
        private List<sys_Action> _ationList;
        protected List<sys_Action> ActionList
        {
            get { return _ationList ?? (_ationList = ActionService.Where(p => p.AppId==AppId && p.IsDelete == false && p.IsUsing == true).OrderBy(p => p.SortIndex).ToList()); }
            set { _ationList = value; }
        }
        /// <summary>
        /// 获取设置 模块数据服务
        /// </summary>
        private AppService _AppService;
        protected AppService AppService
        {
            get { return _AppService ?? (_AppService = new AppService()); }
            set { _AppService = value; }
        }
        #endregion

        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                switch ((ActionType)GetQueryIntValue("action"))
                {
                    case ActionType.ADD:
                        isSucceed = Add();
                        break;
                    case ActionType.EDIT:
                        isSucceed = Edit();
                        break;
                }

            }
            catch (Exception)
            {
                isSucceed = false;
            }
            finally
            {
                if (isSucceed)
                {
                    PageContext.RegisterStartupScript(ActiveWindow.GetHidePostBackReference());
                }
                else
                {
                    Alert.Show("保存失败，请重试！", MessageBoxIcon.Error);
                }
            }
        }
        protected void Window2_Close(object sender, WindowCloseEventArgs e)
        {
            if (hf_MenuIcon.Text != @"~/icon/page.png/")
                imgMenuIcon.ImageUrl = hf_MenuIcon.Text;
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            btnSelectIco.OnClientClick = Window2.GetSaveStateReference(hf_MenuIcon.ClientID)
                                       + Window2.GetShowReference("SelectIcon.aspx");

            tbPMName.OnClientTriggerClick = Window1.GetSaveStateReference(tbPMName.ClientID, hf_PMid.ClientID)
                                            + Window1.GetShowReference("MenuTree.aspx?aid=" + AppId);

            tbApp.OnClientTriggerClick = Window3.GetSaveStateReference(tbApp.ClientID, hf_Appid.ClientID)
                                          + Window3.GetShowReference("../App/AppTree.aspx");

            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            Bind_cblAction();
            switch ((ActionType)GetQueryIntValue("action"))
            {
                case ActionType.ADD:
                    sys_App app = AppService.FirstOrDefault(p => p.Id == AppId);
                    if (app != null)
                    {
                        tbApp.Text = app.AppName;
                        hf_Appid.Text = app.Id.ToString();
                    }
                    nbSortIndex.Text = Convert.ToString(MenuService.Count(p => p.AppId==AppId && p.Id != "0") + 1);
                    break;
                case ActionType.EDIT:
                    if (sysMenu.sys_App != null)
                    {
                        tbApp.Text = sysMenu.sys_App.AppName;
                        hf_Appid.Text = sysMenu.sys_App.Id.ToString();
                    }
                    tbxMenuName.Text = sysMenu.MenuName;
                    tbxMenuCode.Text = sysMenu.MenuCode;

                    //未创建关系 所以暂时直接取值  如果数据库创建了Pid与Id的0-1关系可以去掉
                    sys_Menu parentMenu = MenuService.FirstOrDefault(p => p.Id == sysMenu.PId);
                    if (parentMenu != null)
                    {
                        tbPMName.Text = parentMenu.MenuName;
                        hf_PMid.Text = parentMenu.Id;
                    }
                    else
                    {
                        tbPMName.Text = "系统菜单";
                        hf_PMid.Text = "0";
                    }
                    imgMenuIcon.ImageUrl = sysMenu.Ico;
                    hf_MenuIcon.Text = sysMenu.Ico;
                    tbxUrl.Text = sysMenu.Url;
                    cbxIsOperRes.Checked = sysMenu.IsOperRes;

                    //初始化功能操作
                    InitcblAction();

                    nbSortIndex.Text = sysMenu.SortIndex.ToString();
                    cbxIsUsing.Checked = sysMenu.IsUsing;
                    taDescribe.Text = sysMenu.Describe;

                    break;
            }

        }
        private void Bind_cblAction()
        {
            cblAction.DataSource = ActionList;
            cblAction.DataTextField = "ActionName";
            cblAction.DataValueField = "Id";
            cblAction.DataBind();
        }
        private bool Add()
        {
            //System.Data.Objects.ObjectContext oc = ObjectContextHelper.CreateDbContext();

            var LastMenu = MenuService.FirstOrDefault(p => p.Id != "0", p => p.Id, EnumHelper.ParseEnumByString<OrderingOrders>("DESC"));

            var menu = new sys_Menu
            {
                Id = SerialNumber.Get("M", LastMenu != null ? LastMenu.Id : "", 3),
                AppId = Convert.ToInt32(hf_Appid.Text),
                MenuName = tbxMenuName.Text.Trim(),
                MenuCode = tbxMenuCode.Text.Trim(),
                PId = hf_PMid.Text,
                Ico = hf_MenuIcon.Text,
                Url = tbxUrl.Text.Trim(),
                SortIndex = Convert.ToInt32(nbSortIndex.Text),
                Describe = taDescribe.Text,
                IsOperRes = cbxIsOperRes.Checked,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName,
            };


            IEnumerable<int> ActionList = ArrStrToInt(cblAction.SelectedValueArray.ToList());
            menu.sys_Action = ActionService.Where(p => ActionList.Contains(p.Id)).ToList();
            MenuService.Add(menu);
            Log(LogType.新增, string.Format("添加菜单：{0}", menu.MenuName), "菜单管理");
            return DbContext.SaveChanges() > 0;
        }
        private bool Edit()
        {
            if (sysMenu != null)
            {
                sysMenu.AppId = Convert.ToInt32(hf_Appid.Text);
                sysMenu.MenuName = tbxMenuName.Text.Trim();
                sysMenu.MenuCode = tbxMenuCode.Text.Trim();
                sysMenu.PId = hf_PMid.Text;
                sysMenu.Ico = hf_MenuIcon.Text;
                sysMenu.Url = tbxUrl.Text.Trim();
                sysMenu.IsOperRes = cbxIsOperRes.Checked;

                //修改菜单功能操作 修改之前必须先去掉原有的 不然报错
                sysMenu.sys_Action.Clear();
                IEnumerable<int> SelectActionList = ArrStrToInt(cblAction.SelectedValueArray.ToList());
                sysMenu.sys_Action = ActionList.Where(p => SelectActionList.Contains(p.Id)).ToList();

                sysMenu.SortIndex = Convert.ToInt32(nbSortIndex.Text);
                sysMenu.Describe = taDescribe.Text;
                sysMenu.IsUsing = Convert.ToBoolean(cbxIsUsing.Checked);

                sysMenu.ModifyDate = DateTime.Now;
                sysMenu.ModifyRealName = BaseRealName;
                sysMenu.ModifyUserName = BaseUserName;

                Log(LogType.修改, string.Format("修改菜单：{0}", sysMenu.MenuName), "菜单管理");
                return MenuService.SaveChanges() > 0;
            }
            return false;
        }
        private IEnumerable<int> GetSelectIds()
        {
            string[] selections = cblAction.SelectedValueArray;

            var selectIds = new int[selections.Length];

            for (int i = 0; i < selections.Length; i++)
            {
                selectIds[i] = Int32.Parse(selections[i]);
            }
            return selectIds;
        }
        private void InitcblAction()
        {
            string selectValueStr = "";
            foreach (sys_Action a in sysMenu.sys_Action)
            {
                selectValueStr += a.Id.ToString() + ",";
            }
            cblAction.SelectedValueArray = selectValueStr.Trim(',').Split(',');
        }

        #endregion
    }
}