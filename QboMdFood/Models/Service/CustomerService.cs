using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using Intuit.Ipp.Data;
using Intuit.Ipp.DataService;
using QboMdFood.Models.API;
using QboMdFood.Models.DTO;
using QboMdFood.Models.Repository;
using QboMdFood.Models.Utility;

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

        public Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>> MatchCustomerWithApi()
        {
            SalesRequest sales = new SalesRequest();

            var salesItemFromApi = sales.GetCall()?.Sales?.Record;
            if (salesItemFromApi == null) return new Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>>(null, null);
            IEnumerable<Record> newSalesItem;
            IEnumerable<Record> SalesItem = new List<Record>();
            var qboCustomer = from c in GetCustomer()
                              select new { id = c.Id, name = c.FullyQualifiedName, CodeId = c.AlternatePhone?.FreeFormNumber ?? "N/A" };

            foreach (var customerItem in qboCustomer.Where(a => a.CodeId != "N/A"))
            {
                if (customerItem.CodeId == "N/A") continue;
                //Create list with QBO CustomerID and change CheckUserExsits to true
                newSalesItem = new List<Record>(from a in salesItemFromApi
                                                where a.Customer_id == customerItem.CodeId
                                                select new Record()
                                                {
                                                    Customer_id = customerItem.id,
                                                    CheckUserExsits = true,
                                                    Invoice_date = a.Invoice_date,
                                                    Gst_amount = a.Gst_amount,
                                                    Amount = a.Amount,
                                                    Invoice_number = a.Invoice_number,
                                                    Customer_name = customerItem.name
                                                });

                SalesItem = SalesItem.Concat(newSalesItem);
            }
            //remove all item that match Invoice Number in SalesItem
            var exceptNewSalesItem = salesItemFromApi.Except(SalesItem, new InvoiceNumberCompare()).ToList();

            //Create Customer list with unregistered users free duplication
            var userNotExsit = exceptNewSalesItem
                .GroupBy(a => new { a.Customer_id, a.Customer_name }).Select(q => q.First()).Select(a => new CustomerTo()
                {
                    Customer_Id = a.Customer_id,
                    Customer_name = a.Customer_name
                });
            ;


            //add all the item from SalesItem to exceptNewSalesItem
            var finalListResult = SalesItem.Concat(exceptNewSalesItem).ToList();

            return new Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>>(userNotExsit, finalListResult);
        }
    }
}