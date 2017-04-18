using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using QboMdFood.Models.DTO;
using QboMdFood.Models.Repository;

namespace QboMdFood.Models.Service
{
    public class CustomerService
    {
        DataserviceFactory dataserviceFactory = null;
        DataService dataService = null;
        CustomerRepository CustomerRepositoryobj = null;
        public CustomerService(OAuthorizationdto oAuthorization)
        {
            dataserviceFactory = new DataserviceFactory(oAuthorization);
            dataService = dataserviceFactory.getDataService();
            CustomerRepositoryobj = new CustomerRepository();
        }
        public List<Customer> GetCustomer()
        {
            return CustomerRepositoryobj.GeALLCustomerCodes(dataService);
        }

        public Dictionary<Int16, List<Customer>> MatchCustomerWithApi()
        {
            SalesRequest sales = new SalesRequest();

            var salesItem = sales.GetCall();

            List<Customer> CustomerItem = GetCustomer();

            var search = from c in CustomerItem
                         select new { id = c.Id, name = c.FullyQualifiedName, CodeId = c.AlternatePhone?.FreeFormNumber ?? "N/A" };

            search = search.Where(a => a.CodeId.Contains("AF"));


            return new Dictionary<short, List<Customer>>();
        }
    }
}