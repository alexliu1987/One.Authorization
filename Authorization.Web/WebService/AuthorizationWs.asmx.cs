using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Dynamic;
using System.Web.Services;
using Authorization.Framework.DataBase;
using Authorization.Model;
using Authorization.Services;
using MasterData.ServiceBusProvider;
using SSO.Common.Model;

namespace Authorization.Web.WebService
{
    /// <summary>
    /// AuthorizationWs 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class AuthorizationWs : System.Web.Services.WebService
    {
        [WebMethod]
        public UserInfo GetUserWithAuth(string userName)
        {
            var user = Bus.GetMasterDataSync(ConfigHelper.GetAppSettingString("UserServiceCode"), "FindByName", new { name = userName },
                ConfigHelper.GetAppSettingString("SystemCode"), t => t.ToObject<UserInfo>());
            if (user == null)
                return null;
            var roleService = new RoleService();
            var lstRoleRight = new List<string>();
            roleService.Where(r => r.sys_Member.Any(m => m.UserName == user.Name))
                .Select(r => r.RoleRightStr.Split(','))
                .ToList()
                .ForEach(lstRoleRight.AddRange);
            user.RightList = lstRoleRight.Distinct().ToList();
            return user;
        }
    }
}
