using Intuit.Ipp.Core;
using Intuit.Ipp.Data;
using QboMdFood.Models.Service;
using System.Collections.Generic;
using System.Linq;
using Intuit.Ipp.DataService;
using Intuit.Ipp.QueryFilter;
using QboMdFood.Models.DTO;

namespace QboMdFood.Models.Repository
{
    public class TaxRepository
    {

        public List<TaxCode> GeALLTaxCodes(DataService _qboContextoAuth)
        {
            return Helper.Helper.FindAll<TaxCode>(_qboContextoAuth, new TaxCode());


        }


    }
}