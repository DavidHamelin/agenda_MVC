using Agenda.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace Agenda.Controllers
{
    public class HomeController : Controller
    {
        private agendaEntities db = new agendaEntities();
        public ActionResult Index()
        {
            var appointements = db.appointements.Include(a => a.brokers).Include(a => a.customers);
            //var appointements = db.appointements.Include("brokers").Include("customers");
            return View(appointements.ToList());
            // return View("_listAppointements");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}