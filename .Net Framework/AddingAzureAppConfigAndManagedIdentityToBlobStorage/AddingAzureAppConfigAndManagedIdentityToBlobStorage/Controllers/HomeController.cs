using Microsoft.FeatureManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace AddingAzureAppConfigAndManagedIdentityToBlobStorage.Controllers
{
    public class HomeController : Controller
    {
        //private readonly IConfiguration _configuration;
        private readonly IFeatureManager _featureManager;
        public HomeController(
            //IConfigurationBuilder configuration,
            IFeatureManager featureManager)
        {
            //_configuration = configuration;
            _featureManager = featureManager;
        }

        public HomeController() { }

        public async Task<ActionResult> Index()
        {
            if (_featureManager != null && await _featureManager.IsEnabledAsync("Test"))
            {
                //var message = _configuration["TestApp:Settings:Message"];
                //ViewBag.Message = message;
            }
            else
            {
                // Code to execute when the feature flag is disabled
            }

            

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}