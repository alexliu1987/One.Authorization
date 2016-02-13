using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

using FineUI;
using Authorization.Model;
using Authorization.Services;
using Authorization.Common;
using SSO.Common.SSO;

namespace Authorization.Web.sys.User
{
    public partial class UserEdit : PageBase
    {
        #region 定义
        protected int ActionId
        {
            get { return GetQueryIntValue("id"); }
        }
        protected int OrgnizationId
        {
            get { return GetQueryIntValue("oid"); }
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        private sys_Member _Member;
        protected sys_Member Member
        {
            get { return _Member ?? (_Member = MemberService.FirstOrDefault(p => p.id == ActionId)); }
        }
        /// <summary>
        /// 用户数据服务
        /// </summary>
        private MemberService _MemberService;
        /// <summary>
        /// 获取设置 用户数据服务
        /// </summary>
        protected MemberService MemberService
        {
            get { return _MemberService ?? (_MemberService = new MemberService()); }
            set { _MemberService = value; }
        }
        /// <summary>
        /// 部门数据服务
        /// </summary>
        private OrgnizationService _OrgnizationService;
        /// <summary>
        /// 获取 部门数据服务
        /// </summary>
        protected OrgnizationService OrgnizationService
        {
            get { return _OrgnizationService ?? (_OrgnizationService = new OrgnizationService()); }
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
            bool isUserNameExist = false;
            if (hf_OrgnizaionId.Text == "0")
            {
                Alert.Show("请选择部门，不能将用户直接添加到公司级下面！", MessageBoxIcon.Warning);
                return;
            }
            try
            {
                switch ((ActionType)GetQueryIntValue("action"))
                {
                    case ActionType.ADD:
                        if (MemberService.FirstOrDefault(p => p.UserName == tbxUserName.Text.Trim() && p.IsDelete == false) != null)
                        {
                            isUserNameExist = true;
                            return;
                        }
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
                    if (isUserNameExist)
                        Alert.Show("用户名已存在！", MessageBoxIcon.Warning);
                    else
                        Alert.Show("保存失败，请重试！", MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region 方法
        private void LoadData()
        {
            btnClose.OnClientClick = ActiveWindow.GetHideReference();

            tbOrgnization.OnClientTriggerClick = Window1.GetSaveStateReference(tbOrgnization.ClientID, hf_OrgnizaionId.ClientID)
                                          + Window1.GetShowReference("../Orgnization/OrgnizationTree.aspx");
            switch ((ActionType)GetQueryIntValue("action"))
            {
                case ActionType.ADD:
                    sys_Orgnization orgnization = OrgnizationService.FirstOrDefault(p => p.Id == OrgnizationId);
                    if (orgnization != null)
                    {
                        tbOrgnization.Text = orgnization.OrgnizationName;
                        hf_OrgnizaionId.Text = orgnization.Id.ToString();
                    }
                    break;
                case ActionType.EDIT:
                    tbxUserName.Text = Member.UserName;
                    tbxRealName.Text = Member.RealName;
                    rblSex.SelectedValue = Member.Sex.ToString();
                    tbOrgnization.Text = Member.sys_Orgnization.OrgnizationName;
                    hf_OrgnizaionId.Text = Member.sys_Orgnization.Id.ToString();
                    tbxEmail.Text = Member.Email;
                    tbxTel.Text = Member.sys_MemberExtend.Tel;
                    tbxQQ.Text = Member.sys_MemberExtend.QQ;
                    tbxIdCard.Text = Member.sys_MemberExtend.IdCard;
                    dpBirthday.Text = Member.sys_MemberExtend.Birthday.ToString();
                    break;
            }

        }

        private bool Add()
        {
            var member = new sys_Member
            {
                UserName = tbxUserName.Text.Trim(),
                RealName = tbxRealName.Text.Trim(),
                Sex = Convert.ToBoolean(rblSex.SelectedValue),
                Password = DESEncrypt.Encrypt("123", tbxUserName.Text.Trim()),
                IsDelete = false,
                Email = tbxEmail.Text.Trim(),
                sys_Orgnization_Id = Convert.ToInt32(hf_OrgnizaionId.Text),
                IsUsing = true,
                sys_MemberExtend = new sys_MemberExtend
                {
                    Birthday = string.IsNullOrWhiteSpace(dpBirthday.Text) ? DateTimeNullValue : Convert.ToDateTime(dpBirthday.Text),
                    IdCard = tbxIdCard.Text.Trim(),
                    QQ = tbxQQ.Text.Trim(),
                    Tel = tbxTel.Text.Trim()
                },
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName,

            };
            Log(LogType.新增, string.Format("添加用户：{0}", member.RealName), "用户管理");
            return MemberService.Add(member);
        }
        private bool Edit()
        {
            if (Member != null)
            {
                Member.UserName = tbxUserName.Text.Trim();
                Member.RealName = tbxRealName.Text.Trim();
                Member.Sex = Convert.ToBoolean(rblSex.SelectedValue);
                Member.Email = tbxEmail.Text.Trim();
                Member.sys_MemberExtend.Birthday = string.IsNullOrWhiteSpace(dpBirthday.Text) ? DateTimeNullValue : Convert.ToDateTime(dpBirthday.Text);
                Member.sys_MemberExtend.IdCard = tbxIdCard.Text.Trim();
                Member.sys_MemberExtend.Tel = tbxTel.Text.Trim();
                Member.sys_MemberExtend.QQ = tbxQQ.Text.Trim();

                Member.sys_Orgnization_Id = Convert.ToInt32(hf_OrgnizaionId.Text);
                Member.ModifyDate = DateTime.Now;
                Member.ModifyRealName = BaseRealName;
                Member.ModifyUserName = BaseUserName;

                Log(LogType.修改, string.Format("修改用户：{0}", Member.RealName), "用户管理");
                return MemberService.SaveChanges() > 0;
            }
            return false;
        }
        #endregion

    }
}