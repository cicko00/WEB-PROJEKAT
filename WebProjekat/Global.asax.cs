using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebProjekat.Models;

namespace WebProjekat
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            BazaPodataka bp = new BazaPodataka();
            HttpContext.Current.Application["Korisnici"] = bp.KORISNICI;
            HttpContext.Current.Application["FitnesCentri"] = bp.FITNESCENTRI;
            HttpContext.Current.Application["GrupniTreninzi"] = bp.GRUPNITRENINZI;
            HttpContext.Current.Application["Komentari"] = bp.KOMENTARI;
        }
    }
}
