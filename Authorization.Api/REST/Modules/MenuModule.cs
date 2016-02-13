using System.Linq;
using Authorization.Services;
using Nancy;

namespace Authorization.Api.REST.Modules
{
    public class MenuModule : BaseModule
    {
        public MenuModule()
            : base("/Menu")
        {
            Get["/AppMenu/{APPCODE}"] = _ =>
            {
                string appCode = _.APPCODE;
                var result = new ViewAppMenuService().Where(v => v.AppCode == appCode && v.IsUsing && (!v.IsDelete.HasValue || !v.IsDelete.Value)).ToArray();
                return Response.AsJson(result);
            };
        }
    }
}
