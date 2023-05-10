using System.Configuration;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace AddingAzureAppConfigAndManagedIdentityToBlobStorage
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            //IConfigurationBuilder configBuilder = new ConfigurationBuilder();
            //string connectionString = ConfigurationManager.AppSettings["AzureAppConfigurationConnectionString"];
            //configBuilder.AddAzureAppConfiguration(connectionString);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
