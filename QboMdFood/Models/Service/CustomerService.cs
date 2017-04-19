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

            var salesItem = sales.GetCall().Sales.Record;

            IEnumerable<Record> NewSalesItem = new List<Record>();
            var qboCustomer = from c in GetCustomer()
                              select new { id = c.Id, name = c.FullyQualifiedName, CodeId = c.AlternatePhone?.FreeFormNumber ?? "N/A" };

            foreach (var customerItem in qboCustomer.Where(a => a.CodeId != "N/A"))
            {
                if (customerItem.CodeId != "N/A")
                {
                    NewSalesItem = from a in salesItem
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
                                   };

                }

            }
            var exceptNewSalesItem = salesItem.Except(NewSalesItem, new InvoiceNumberComparer()).ToList();

            var userNotExsit = salesItem.Except(NewSalesItem, new CustomerComparer())
                .Select(a =>
            new CustomerTo()
            {
                Customer_Id = a.Customer_id,
                Customer_name = a.Customer_name
            });

            var finalListResult = NewSalesItem.Concat(exceptNewSalesItem).ToList();



            return new Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>>(userNotExsit, finalListResult);


        }
    }
}