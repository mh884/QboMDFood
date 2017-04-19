using System;
using Intuit.Ipp.Data;
using QboMdFood.Models.Service;
using System.Collections.Generic;
using System.Threading;
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
            // SalesRequest sales = new SalesRequest();


            multiplemodels = new Multiplemodels
            {
                taxtoobj = new List<TaxCode>(),
                CustomersObj = new Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>>(null, null),
                OAuthorizationModel = new OAuthorizationdto()
            };

            var oAuthModel = new OAuthService(multiplemodels.OAuthorizationModel).IsTokenAvailable(this);

            var taxObj = new TaxService(oAuthModel);
            if (oAuthModel.IsConnected)
            {

                multiplemodels.OAuthorizationModel = oAuthModel;

                multiplemodels.IsConnected = oAuthModel.IsConnected;
                var customerobj = new CustomerService(oAuthModel);
                multiplemodels.taxtoobj = taxObj.GeTaxCodes();
                multiplemodels.CustomersObj = customerobj.MatchCustomerWithApi();

                //  multiplemodels.SalesItems = sales.GetCall();


                return View(multiplemodels);
            }
            else
            {
                return View(multiplemodels);
            }


        }
    }
}