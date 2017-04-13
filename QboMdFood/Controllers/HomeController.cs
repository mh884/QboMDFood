using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QboMdFood.Models;
using QboMdFood.Models.DTO;
using QboMdFood.Models.Service;


namespace QboMdFood.Controllers
{
    public class HomeController : Controller
    {
        Multiplemodels multiplemodels = null;
        public ActionResult Index()
        {

            multiplemodels = new Multiplemodels();
            multiplemodels.OAuthorizationModel = new OAuthorizationdto();

            var oAuthModel = new OAuthService(multiplemodels.OAuthorizationModel).IsTokenAvailable(this);
            if (oAuthModel.IsConnected)
            {

                multiplemodels.OAuthorizationModel = oAuthModel;
                multiplemodels.IsConnected = oAuthModel.IsConnected;

                return View(multiplemodels);
            }
            else
            {
                return View(multiplemodels);
            }
        }


        public ActionResult Close()
        {
            multiplemodels = new Multiplemodels();
            multiplemodels.OAuthorizationModel = new OAuthorizationdto();

            return View(multiplemodels);
        }
    }
}