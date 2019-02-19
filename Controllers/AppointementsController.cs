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
            rdvToAdd.dateHour = Convert.ToDateTime(dateAndHour);
            // Conversion en Datetime
            // Faire en sorte qu'un courtier n'ai pas 2 rendez-vous en même temps (même jour et même heure)
            //var isAlreadyUsed = db.appointements.Where(rdv => (rdv.dateHour == rdvToAdd.dateHour) && (rdv.idBroker == rdvToAdd.idBroker));
            // Vérifie si ce rdv est déjà utilisé
            //if (rdvToAdd == isAlreadyUsed)
            //{
            //ModelState.AddModelError("datehour", "Un client existant porte cette adresse mail");
            //return View("Error");
            //}
            //vérification que le champ datehour n'est pas null
            //if (rdvToAdd.dateHour == null)
            //{
            //    ModelState.AddModelError("dateHour", "Ecrire une date");
            //}

            ViewBag.idBroker = new SelectList(db.brokers.Select(bro => new { idBro = bro.idBroker, fullname = bro.firstname + " " + bro.lastname }), "idBro", "fullname", rdvToAdd.idBroker);
            ViewBag.idCustomer = new SelectList(db.customers.Select(cus => new { idCus = cus.idCustomer, fullname = cus.firstname + " " + cus.lastname }), "idCus", "fullname", rdvToAdd.idCustomer);

            var isAlreadyUsed = db.appointements.Where(rdv => rdv.idBroker == rdvToAdd.idBroker && rdv.dateHour == rdvToAdd.dateHour || rdv.idCustomer == rdvToAdd.idCustomer && rdv.dateHour == rdvToAdd.dateHour).SingleOrDefault();
            if (isAlreadyUsed != null)
            {
                ModelState.AddModelError("dateHour", "/!\\ Un RDV existe déja avec ce courtier à cette plage horaire /!\\");
                return View(rdvToAdd);
            }
            //return View(rdvToAdd);
            

            //var isAlreadyUsed = db.appointements.Where(rdv => rdv.dateHour == rdvToAdd.dateHour);
            //foreach (appointements item in isAlreadyUsed)
            //{
            //    if (item.idBroker == rdvToAdd.idBroker)
            //    {
            //        ModelState.AddModelError("idCustomer", "Un rdv est déja pris avec ce courtier à cette date");
            //    }
            //}
            if (ModelState.IsValid)
            {
                db.appointements.Add(rdvToAdd);
                db.SaveChanges();
                return RedirectToAction("SuccessAddAppointement");
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
        // VUE PARTIELLE pour la liste dans Home
        // Détails du rdv selectionné
        public ActionResult detailsAppointement(int? id)
        {
            var appointements = db.appointements.Include(a => a.brokers).Include(a => a.customers);
            appointements idAppointement = db.appointements.Find(id);
            if (idAppointement == null || id == null)
            {
                return HttpNotFound(); // renvoyer vers page d'erreur
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