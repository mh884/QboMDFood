using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using QboMdFood.Models.Service;

namespace QboMdFood.Controllers
{
    public class SalesController : Controller
    {
        // GET: Sales
        public ActionResult SalesRecord()
        {
            SalesRequest sales = new SalesRequest();

            return View(sales.GetCall());
        }
    }
}