using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace SOP.Controllers
{
    public class BaseController : Controller
    {
        //
        // GET: /Base/
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {   
            base.Initialize(requestContext);

            if (requestContext.HttpContext.Session["UserId"] == null)
            {
                var url = Url.Action("Index", "Login");
                requestContext.HttpContext.Response.Redirect(url);
                return;
            }

        }
        
   
	}

}