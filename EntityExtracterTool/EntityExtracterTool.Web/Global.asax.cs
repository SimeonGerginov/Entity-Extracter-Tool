using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

using EntityExtracterTool.Web.Common;
using EntityExtracterTool.Web.Providers;
using EntityExtracterTool.Web.Services.Contracts;

namespace EntityExtracterTool.Web
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            this.FillEntityHolder();
        }

        private void FillEntityHolder()
        {
            var entityComparer = (IEntityComparer)DependencyResolver.Current.GetService(typeof(IEntityComparer));

            entityComparer.CompareEntities(EntityHolderProvider.GetEntityHolder(), Constants.PreviousVersion, Constants.CurrentVersion);
        }
    }
}
