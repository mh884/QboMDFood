using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QboMdFood.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        public virtual ActionResult Time()
        {
            return PartialView("_Layout");
        }
    }
}