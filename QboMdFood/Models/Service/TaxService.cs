using Intuit.Ipp.Data;
using QboMdFood.Models.Repository;
using System.Collections.Generic;
using Intuit.Ipp.Core;
using Intuit.Ipp.DataService;
using QboMdFood.Models.DTO;

namespace QboMdFood.Models.Service
{
    public class TaxService
    {


        DataserviceFactory dataserviceFactory = null;
        DataService dataService = null;
        TaxRepository taxRepositoryobj = null;
        public TaxService(OAuthorizationdto oAuthorization)
        {
            dataserviceFactory = new DataserviceFactory(oAuthorization);
            dataService = dataserviceFactory.getDataService();
            taxRepositoryobj = new TaxRepository();
        }
        public List<TaxCode> GeTaxCodes()
        {
            return taxRepositoryobj.GeALLTaxCodes(dataService);
        }
    }
}