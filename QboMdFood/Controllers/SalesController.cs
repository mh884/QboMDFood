using Intuit.Ipp.Data;
using QboMdFood.Models.Service;
using System.Collections.Generic;
using System.Web.Mvc;
using Intuit.Ipp.Core;
using QboMdFood.Models;
using QboMdFood.Models.API;
using QboMdFood.Models.DTO;
using TaxService = QboMdFood.Models.Service.TaxService;

namespace QboMdFood.Controllers
{
    public class SalesController : Controller
    {
        Multiplemodels multiplemodels = null;

        private TaxService taxObj = null;
        // GET: Sales
        public ActionResult SalesRecord()
        {
            SalesRequest sales = new SalesRequest();


            multiplemodels = new Multiplemodels();
            multiplemodels.taxtoobj = new List<TaxCode>();
            multiplemodels.CustomersObj = new List<Customer>();
            multiplemodels.OAuthorizationModel = new OAuthorizationdto();
            multiplemodels.SalesItems = new Response();
            var oAuthModel = new OAuthService(multiplemodels.OAuthorizationModel).IsTokenAvailable(this);

            if (oAuthModel.IsConnected)
            {

                multiplemodels.OAuthorizationModel = oAuthModel;

                multiplemodels.IsConnected = oAuthModel.IsConnected;
                var taxObj = new TaxService(oAuthModel);
                var customerobj = new CustomerService(oAuthModel);
                multiplemodels.taxtoobj = taxObj.GeTaxCodes();
                multiplemodels.CustomersObj = customerobj.GetCustomer();
                var d = customerobj.MatchCustomerWithApi();
                multiplemodels.SalesItems = sales.GetCall();

                return View(multiplemodels);
            }
            else
            {
                return View(multiplemodels);
            }


        }
    }
}