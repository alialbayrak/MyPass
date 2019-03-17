using MyPass.Web.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MyPass.Web.Filter
{
    public class AuthFilter : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionHelper.GetCurrentUser() == null)
            {
                filterContext.Result = new RedirectResult("/User/Login");
            }
        }

    }
}