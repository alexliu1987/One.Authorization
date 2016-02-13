using System.Linq;
using Authorization.Model;
using Authorization.Services;
using Nancy;
using Nancy.ModelBinding;

namespace Authorization.Api.REST.Modules
{
    public class UserModule : BaseModule
    {
        public UserModule()
            : base("/User")
        {
            Post["/Login"] = _ =>
            {
                var postData = this.Bind<UserPostData>();
                var user = new MemberService().FirstOrDefault(u => u.UserName == postData.UserName);
                return Response.AsJson(user != null && user.Password == postData.Password);
            };
            Get["/Auth/{UNAME}"] = _ => 
            {
                string uname = _.UNAME;
                var result = new ViewMemberRoleService().Where(v => v.UserName == uname).ToArray();
                return Response.AsJson(result);
            };
        }
    }

    public class UserPostData
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
