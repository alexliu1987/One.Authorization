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
namespace Authorization.Web.sys.App
{
    public partial class AppEdit : PageBase
    {
        #region 定义
        protected int AppId
        {
            get { return GetQueryIntValue("id"); }
        }
        /// <summary>
        /// 字典信息
        /// </summary>
        private sys_App _App;
        /// <summary>
        /// 字典数据服务
        /// </summary>
        private AppService _AppService;
        /// <summary>
        /// 获取设置 字典数据服务
        /// </summary>
        protected AppService AppService
        {
            get { return _AppService ?? (_AppService = new AppService()); }
            set { _AppService = value; }
        }
        /// <summary>
        /// 获取设置 字典信息
        /// </summary>
        protected sys_App sysApp
        {
            get { return _App ?? (_App = AppService.FirstOrDefault(p => p.Id == AppId)); }
            set { _App = value; }
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
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            switch ((ActionType)GetQueryIntValue("action"))
            {
                case ActionType.ADD:
                    nbSortIndex.Text = Convert.ToString(AppService.Count(p => p.Id != -1) + 1);
                    break;
                case ActionType.EDIT:
                    hf_Ico.Text = sysApp.Ico;
                    imgIcon.ImageUrl = sysApp.Ico;
                    tbxAppName.Text = sysApp.AppName;
                    tbxAppCode.Text = sysApp.AppCode;
                    nbSortIndex.Text = sysApp.SortIndex.ToString();
                    cbxIsUsing.Checked = sysApp.IsUsing;
                    taDescribe.Text = sysApp.Describe;
                    break;
            }

        }
        private bool Add()
        {
            var app = new sys_App
            {
                AppName = tbxAppName.Text.Trim(),
                AppCode = tbxAppCode.Text.Trim(),
                Ico=hf_Ico.Text,
                SortIndex = Convert.ToInt32(nbSortIndex.Text),
                Describe = taDescribe.Text,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName,
            };
            Log(LogType.新增, string.Format("新增功能：{0}", app.AppName), "功能管理");
            return AppService.Add(app);
        }
        private bool Edit()
        {
            if (sysApp != null)
            {
                sysApp.AppName = tbxAppName.Text.Trim();
                sysApp.AppCode = tbxAppCode.Text.Trim();
                sysApp.SortIndex = Convert.ToInt32(nbSortIndex.Text);
                sysApp.Describe = taDescribe.Text;
                sysApp.IsUsing = Convert.ToBoolean(cbxIsUsing.Checked);
                sysApp.Ico = hf_Ico.Text;

                sysApp.ModifyDate = DateTime.Now;
                sysApp.ModifyRealName = BaseRealName;
                sysApp.ModifyUserName = BaseUserName;
                Log(LogType.修改, string.Format("修改功能：{0}", sysApp.AppName), "功能管理");
                return AppService.SaveChanges() > 0;
            }
            return false;
        }
        #endregion


    }
}