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
    public partial class DictionaryTypeEdit : PageBase
    {
        #region 定义
        protected int ActionId {
            get { return GetQueryIntValue("id"); }
        }
        /// <summary>
        /// 字典类型信息
        /// </summary>
        private sys_DictionaryType _DictionaryType;
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
        /// <summary>
        /// 获取设置 字典类型信息
        /// </summary>
        protected sys_DictionaryType DictionaryType
        {
            get { return _DictionaryType ?? (_DictionaryType = DictionaryTypeService.FirstOrDefault(p => p.Id == ActionId)); }
            set { _DictionaryType = value; }
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
            switch ((ActionType)GetQueryIntValue("action"))
            { 
                case ActionType.ADD:
                    nbSortIndex.Text =Convert.ToString( DictionaryTypeService.Count(p=>p.Id!=-1)+1);
                    break;
                case ActionType.EDIT:
                    tbxTypeName.Text = DictionaryType.TypeName;
                    nbSortIndex.Text = DictionaryType.SortIndex.ToString();
                    cbxIsUsing.Checked = DictionaryType.IsUsing;
                    taDescribe.Text = DictionaryType.Describe;
                    break;            
            }

        }
        private bool Add()
        {
            var dictionaryType = new sys_DictionaryType
            {
                TypeName = tbxTypeName.Text.Trim(),
                SortIndex = Convert.ToInt32(nbSortIndex.Text),
                Describe = taDescribe.Text,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName
            };
            Log(LogType.新增, string.Format("添加字典内容：{0}", dictionaryType.TypeName), "字典类型管理");
            return DictionaryTypeService.Add(dictionaryType);
        }
        private bool Edit()
        {
            if (DictionaryType != null)
            {
                DictionaryType.TypeName = tbxTypeName.Text.Trim();
                DictionaryType.SortIndex = Convert.ToInt32(nbSortIndex.Text);
                DictionaryType.Describe = taDescribe.Text;
                DictionaryType.IsUsing = Convert.ToBoolean(cbxIsUsing.Checked);

                DictionaryType.ModifyDate = DateTime.Now;
                DictionaryType.ModifyRealName = BaseRealName;
                DictionaryType.ModifyUserName = BaseUserName;
                Log(LogType.修改, string.Format("修改字典内容：{0}", DictionaryType.TypeName), "字典类型管理");
                return DictionaryTypeService.SaveChanges() > 0;
            }
            return false;
        }
        #endregion

    }
}