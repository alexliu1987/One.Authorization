using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
namespace Authorization.Web.sys.Action
{
    public partial class ActionEdit : PageBase
    {
        #region 定义
        protected int ActionId
        {
            get { return GetQueryIntValue("id"); }
        }
        protected int AppId
        {
            get { return GetQueryIntValue("aid"); }
        }
        /// <summary>
        /// 字典信息
        /// </summary>
        private sys_Action _Action;
        /// <summary>
        /// 字典数据服务
        /// </summary>
        private ActionService _ActionService;
        /// <summary>
        /// 获取设置 字典数据服务
        /// </summary>
        protected ActionService ActionService
        {
            get { return _ActionService ?? (_ActionService = new ActionService()); }
            set { _ActionService = value; }
        }
        /// <summary>
        /// 获取设置 字典信息
        /// </summary>
        protected sys_Action sysAction
        {
            get { return _Action ?? (_Action = ActionService.FirstOrDefault(p => p.Id == ActionId)); }
            set { _Action = value; }
        }
        /// <summary>
        /// 字典类型数据服务
        /// </summary>
        private DictionaryService _DictionaryService;
        /// <summary>
        /// 获取设置 字典类型数据服务
        /// </summary>
        protected DictionaryService DictionaryService
        {
            get { return _DictionaryService ?? (_DictionaryService = new DictionaryService()); }
            set { _DictionaryService = value; }
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
            if (hf_Ico.Text != "~/icon/")
                imgIcon.ImageUrl = hf_Ico.Text;
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            btnSelectIco.OnClientClick = Window2.GetSaveStateReference(hf_Ico.ClientID)
                                       + Window2.GetShowReference("SelectIcon.aspx");

            tbApp.OnClientTriggerClick = Window3.GetSaveStateReference(tbApp.ClientID, hf_Appid.ClientID)
                                          + Window3.GetShowReference("../App/AppTree.aspx");

            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            Bind_ddlTypeName();
            switch ((ActionType)GetQueryIntValue("action"))
            {
                case ActionType.ADD:
                    sys_App app = AppService.FirstOrDefault(p => p.Id == AppId);
                    if (app != null)
                    {
                        tbApp.Text = app.AppName;
                        hf_Appid.Text = app.Id.ToString();
                    }
                    nbSortIndex.Text = Convert.ToString(ActionService.Count(p => p.AppId==AppId && p.Id != -1) + 1);
                    break;
                case ActionType.EDIT:
                    if (sysAction.sys_App != null)
                    {
                        tbApp.Text = sysAction.sys_App.AppName;
                        hf_Appid.Text = sysAction.sys_App.Id.ToString();
                    }
                    hf_Ico.Text = sysAction.ActionIco;
                    imgIcon.ImageUrl = sysAction.ActionIco;
                    tbxAction.Text = sysAction.ActionName;
                    tbxActionCode.Text = sysAction.ActionCode;
                    nbSortIndex.Text = sysAction.SortIndex.ToString();
                    cbxIsUsing.Checked = sysAction.IsUsing;
                    taDescribe.Text = sysAction.Describe;
                    ddlActionType.SelectedValue = sysAction.ActionType;
                    break;
            }

        }
        private void Bind_ddlTypeName()
        {
            ddlActionType.DataSource = DictionaryService.Where(p => p.IsDelete == false && p.IsUsing == true && p.sys_DictionaryType_Id == 8);
            ddlActionType.DataTextField = "Value";
            ddlActionType.DataValueField = "Value";
            ddlActionType.DataBind();
            try
            {
                ddlActionType.SelectedValue = GetQueryValue("typeid");
            }
            catch { }
        }
        private bool Add()
        {
            var action = new sys_Action
            {
                AppId = Convert.ToInt32(hf_Appid.Text),
                ActionType = ddlActionType.SelectedText,
                ActionName = tbxAction.Text.Trim(),
                ActionCode = tbxActionCode.Text.Trim(),
                ActionIco=hf_Ico.Text,
                SortIndex = Convert.ToInt32(nbSortIndex.Text),
                Describe = taDescribe.Text,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName,
            };
            Log(LogType.新增, string.Format("新增功能：{0}", action.ActionName), "功能管理");
            return ActionService.Add(action);
        }
        private bool Edit()
        {
            if (sysAction != null)
            {
                sysAction.AppId = Convert.ToInt32(hf_Appid.Text);
                sysAction.ActionType = ddlActionType.SelectedText;
                sysAction.ActionName = tbxAction.Text.Trim();
                sysAction.ActionCode = tbxActionCode.Text.Trim();
                sysAction.SortIndex = Convert.ToInt32(nbSortIndex.Text);
                sysAction.Describe = taDescribe.Text;
                sysAction.IsUsing = Convert.ToBoolean(cbxIsUsing.Checked);
                sysAction.ActionIco = hf_Ico.Text;

                sysAction.ModifyDate = DateTime.Now;
                sysAction.ModifyRealName = BaseRealName;
                sysAction.ModifyUserName = BaseUserName;
                Log(LogType.修改, string.Format("修改功能：{0}", sysAction.ActionName), "功能管理");
                return ActionService.SaveChanges() > 0;
            }
            return false;
        }
        #endregion


    }
}