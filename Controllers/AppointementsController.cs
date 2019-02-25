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
        //private DateTime regexDateTime = @"^([0-9]{2})+[\/-]([0-9]{2})+[-\/]([0-9]{1,4})$";

        // GET: Appointements
        public ActionResult AddAppointement(int? id)
        {
            if (id == null)
            {
                ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname");
                ViewBag.idCustomer = new SelectList(db.customers, "idCustomer", "lastname");
                return View("AddAppointement");
            }
            else
            {
                ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname", id);
                ViewBag.idCustomer = new SelectList(db.customers, "idCustomer", "lastname");
                return View("AddAppointement");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddAppointement([Bind(Include = "idAppointement,dateHour,idBroker,idCustomer")] appointements rdvToAdd)
        {
            // concaténation date et time puis conversion en datetime
            var date = Request.Form["dateRdv"]; // récuperer date 
            var hour = Request.Form["hourRdv"]; // récupérer heure
            var dateAndHour = date + " " + hour; // Concaténation
            rdvToAdd.dateHour = Convert.ToDateTime(dateAndHour); // Conversion en Datetime
            // Vérifier si les champs date et time sont vides
            if (String.IsNullOrEmpty(date))
            {
                ModelState.AddModelError("dateHour", "/!\\ Date manquante /!\\");
            }
            if (String.IsNullOrEmpty(hour))
            {
                ModelState.AddModelError("dateHour", "/!\\ Heure manquante /!\\");
            }
            //if (String.IsNullOrEmpty(dateAndHour))
            //{
            //    ModelState.AddModelError("dateHour", "/!\\ Date et Heure manquante /!\\");
            //}
            // Faire en sorte qu'un courtier n'ai pas 2 rendez-vous en même temps (même jour et même heure)
            var brokerAlreadyUsed = db.appointements.Where(rdv => rdv.idBroker == rdvToAdd.idBroker && rdv.dateHour == rdvToAdd.dateHour).SingleOrDefault();
            if (brokerAlreadyUsed != null)
            {
                ModelState.AddModelError("dateHour", "/!\\ Un RDV existe déja avec ce Courtier à cette plage horaire /!\\");
            }
            var customerAlreadyUsed = db.appointements.Where(rdv => rdv.idCustomer == rdvToAdd.idCustomer && rdv.dateHour == rdvToAdd.dateHour).SingleOrDefault();
            if (customerAlreadyUsed != null)
            {
                ModelState.AddModelError("dateHour", "/!\\ Un RDV existe déja avec ce Client à cette plage horaire /!\\");
            }
            if (ModelState.IsValid)
            {
                db.appointements.Add(rdvToAdd);
                db.SaveChanges();
                return RedirectToAction("SuccessAddAppointement");
            }
            ViewBag.idBroker = new SelectList(db.brokers, "idBroker", "lastname", rdvToAdd.idBroker);
            ViewBag.idCustomer = new SelectList(db.customers, "idCustomer", "lastname", rdvToAdd.idCustomer);
            return View("AddAppointement");
        }
        // Page de succès
        public ActionResult SuccessAddAppointement()
        {
            return View("SuccessAddAppointement");
        }
        // VUE PARTIELLE pour la liste dans Home
        // Détails du rdv selectionné
        public ActionResult detailsAppointement(int? id)
        {
            var appointements = db.appointements.Include(a => a.brokers).Include(a => a.customers);
            appointements idAppointement = db.appointements.Find(id);
            if (idAppointement == null || id == null)
            {
                return RedirectToAction("Error", "Shared"); // renvoyer vers page d'erreur
            }
            return View(idAppointement);
        }
        public ActionResult SuccessDeleteAppointement()
        {
            return View("SuccessDeleteAppointement");
        }
        public ActionResult DeleteRdvConfirm(int id)
        {
            appointements rdv = db.appointements.Find(id);
            db.appointements.Remove(rdv);
            db.SaveChanges();
            return View("SuccessDeleteAppointement");
        }
        
    }
}