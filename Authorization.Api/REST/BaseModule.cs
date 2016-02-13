using System.Web;
using Nancy;

namespace Authorization.Api.REST
{
    public class BaseModule : NancyModule
    {
        public BaseModule(string detachmentunit)
            : base("/REST" + detachmentunit)
        {
        }

        public BaseModule()
            : base("/REST")
        {
        }
    }
}