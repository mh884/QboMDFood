using System.Collections.Generic;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;

namespace QboMdFood.Models.Repository
{
    internal class CustomerRepository
    {

        public List<Customer> GeALLCustomerCodes(DataService dataService)
        {
            return Helper.Helper.FindAll<Customer>(dataService, new Customer());
        }
    }
}