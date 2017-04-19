using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Intuit.Ipp.Data;
using QboMdFood.Models.API;
using QboMdFood.Models.DTO;
using QboMdFood.Models.Service;

namespace QboMdFood.Models
{
    public class Multiplemodels
    {
        public OAuthorizationdto OAuthorizationModel { get; set; }
        public bool IsConnected { get; set; }
        public List<TaxCode> taxtoobj { get; set; }


        public Tuple<IEnumerable<CustomerTo>, IEnumerable<Record>> CustomersObj { get; set; }
    }
}