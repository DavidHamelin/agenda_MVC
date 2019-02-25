using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Agenda.Controllers
{
    public class SharedController : Controller
    {
        // GET: Error
        public ActionResult Error()
        {
            return View("Error");
        }
    }
}