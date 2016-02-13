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

namespace Authorization.Web.sys.ForbidIp
{
    public partial class ForbidIpEdit : PageBase
    {
        #region 定义

        /// <summary>
        /// IP黑名单数据服务
        /// </summary>
        private ForbidIpService _ForbidIpService;
        /// <summary>
        /// 获取 IP黑名单数据服务
        /// </summary>
        protected ForbidIpService ForbidIpService
        {
            get { return _ForbidIpService ?? (_ForbidIpService = new ForbidIpService()); }
        }
        #endregion

        #region 事件
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            bool isSucceed = false;

            try
            {
                isSucceed = Add();
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

        private bool Add()
        {
            var forbidip = new sys_ForbidIp
            {
                IP=tbxIP.Text.Trim(),                
                Describe = taDescribe.Text,
                IsUsing = Convert.ToBoolean(cbxIsUsing.Checked),
                IsDelete = false,
                CreateDate = DateTime.Now,
                CreateUserName = BaseUserName,
                CreateRealName = BaseRealName
            };
            Log(LogType.新增, string.Format("添加IP黑名单：{0}", forbidip.IP), "IP黑名单");
            return ForbidIpService.Add(forbidip);
        }
         
        #endregion
    }
}