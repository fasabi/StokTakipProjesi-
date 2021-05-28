using StokTakipProjesi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StokTakipProjesi.Filters
{
    public class AdminAuth : FilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (CurrentSession.User != null)
            {
                if (CurrentSession.User.IsAdmin == false)
                {
                    filterContext.Result = new RedirectResult("/Home/Index");
                }
            }
            else
            {
                filterContext.Result = new RedirectResult("/User/Login");
            }
        }
    }
}