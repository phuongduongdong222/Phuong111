using Events.web.Models;
using System.Web.Mvc;
using System.Web.Routing;

namespace Events.web.Controllers
{
    public class BaseController : Controller //Announce class 'BaseController' from 'Controller'
    {
        protected ApplicationDbContext Db = new ApplicationDbContext();
    }

    public class MyCustomAuthorize : AuthorizeAttribute
    {
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Error", action = "Index" }));
        }
    }
}