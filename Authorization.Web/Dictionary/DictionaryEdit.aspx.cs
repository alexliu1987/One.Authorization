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

namespace Authorization.Web.Dictionary
{
    public partial class DictionaryEdit : PageBase
    {
        #region 定义
        protected int ActionId
        {
            get { return GetQueryIntValue("id"); }
        }
        /// <summary>
        /// 字典信息
        /// </summary>
        private sys_Dictionary _Dictionary;
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
        /// 获取设置 字典信息
        /// </summary>
        protected sys_Dictionary Dictionary
        {
            get { return _Dictionary ?? (_Dictionary = DictionaryService.FirstOrDefault(p => p.Id == ActionId)); }
            set { _Dictionary = value; }
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
        #endregion

        #region 方法
        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();
            Bind_ddlTypeName();
            switch ((ActionType)GetQueryIntValue("action"))
            {
                case ActionType.ADD:
                    nbSortIndex.Text = Convert.ToString(DictionaryService.Count(p => p.Id != -1) + 1);
                    break;
                case ActionType.EDIT:
                    tbxValue.Text = Dictionary.Value;
                    nbSortIndex.Text = Dictionary.SortIndex.ToString();
                    cbxIsUsing.Checked = Dictionary.IsUsing;
                    taDescribe.Text = Dictionary.Describe;
                    ddlTypeName.SelectedValue = Dictionary.sys_DictionaryType_Id.ToString();
                    break;
            }

        }
        private void Bind_ddlTypeName()
        {
            ddlTypeName.DataSource = DictionaryTypeService.Where(p => p.IsDelete == false && p.IsUsing == true);
            ddlTypeName.DataTextField = "TypeName";
            ddlTypeName.DataValueField = "Id";
            ddlTypeName.DataBind();
            try
            {
                ddlTypeName.SelectedValue = GetQueryValue("typeid");
            }
            catch { }
        }
        private bool Add()
        {
            var dictionaryType = new sys_Dictionary
            {
                Value = tbxValue.Text.Trim(),
                SortIndex = Convert.ToInt32(nbSortIndex.Text),
                Describe = taDescribe.Text,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName,
                sys_DictionaryType_Id = Convert.ToInt32(ddlTypeName.SelectedValue)
            };
            Log(LogType.新增, string.Format("添加字典：{0}", dictionaryType.Value), "字典管理");
            return DictionaryService.Add(dictionaryType);
        }
        private bool Edit()
        {
            if (Dictionary != null)
            {
                Dictionary.Value = tbxValue.Text.Trim();
                Dictionary.SortIndex = Convert.ToInt32(nbSortIndex.Text);
                Dictionary.Describe = taDescribe.Text;
                Dictionary.IsUsing = Convert.ToBoolean(cbxIsUsing.Checked);

                Dictionary.ModifyDate = DateTime.Now;
                Dictionary.ModifyRealName = BaseRealName;
                Dictionary.ModifyUserName = BaseUserName;

                Dictionary.sys_DictionaryType_Id = Convert.ToInt32(ddlTypeName.SelectedValue);

                Log(LogType.修改, string.Format("修改字典：{0}", Dictionary.Value), "字典管理");
                return DictionaryService.SaveChanges() > 0;
            }
            return false;
        }
        #endregion
    }
}