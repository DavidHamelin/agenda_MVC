using Agenda.Models;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;
using System.Data.Entity;

namespace Agenda.Controllers
{
    public class AppointementsController : Controller
    {
        private agendaEntities db = new agendaEntities();
        string regexName = @"^[A-Za-zéèàêâôûùïüç\-]+$";
        string regexDateTime = @"[0-9]{1,2}+/[0-9]{1,2}+/[0-9]{4}";
        string regexPhone = @"^[0][0-9]{9}";
        // GET: Appointements
        public ActionResult AddAppointement()
        {
            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname");
            ViewBag.idCustomer = new SelectList(db.customers, "idCustomer", "lastname");
            return View("AddAppointement");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointement([Bind(Include = "idAppointement,dateHour,idBroker,idCustomer")] appointements rdvToAdd)
        {
            if (ModelState.IsValid)
            {
                db.appointements.Add(rdvToAdd); // insertion dans la bdd avec .Add
                db.SaveChanges(); // enregistrer les changements
                return RedirectToAction("SuccessAddAppointement"); // rediriger vers une page de succès
            }
            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname", rdvToAdd.idBroker);
            ViewBag.idCustomers = new SelectList(db.customers, "idCustomer", "lastname", rdvToAdd.idCustomer);
            return View("AddAppointement");
        }
        // Page de succès
        public ActionResult SuccessAddAppointement()
        {
            return View("SuccessAddAppointement");
        }
        // Liste des RDV
        public ActionResult ListAppointements()
        {
            var appointements = db.appointements.Include(a => a.brokers).Include(a => a.customers);
            if (appointements == null)
            {
                return View("Error");
            }
            else
            {
                return View(appointements.ToList());
            }
        }
        //public ActionResult ListAppointements()
        //{
        //    return View(db.appointements.ToList());
        //}

        //public ActionResult DeleteRDVConfirm(int id)
        //{
        //    appointements rdv = db.appointements.Find(id);
        //    db.appointements.Remove(rdv);
        //    db.SaveChanges();
        //    return View("SuccessAddAppointement");
        //}
        //public ActionResult SuccessDeleteRDV()
        //{
        //    return View("SuccessAddAppointement");
        //}
    }
}