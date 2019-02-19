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
            if (appointements != null)
            {
                return View(appointements.ToList());
            }
            else
            {
                //ModelState.AddModelError("appointements", "Aucun rendez-vous n'a été ajouté");
                return View("Error");
            }
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