using Agenda.Models; //lien avec namespace qu'on avait dans AgendaDB.Context.cs : lien avec le model
using System.Text.RegularExpressions; // pour utiliser regex
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net; // pour utiliser HttpStatusCode dans listBrokers
using System.Data.Entity;

namespace Agenda.Controllers
{
    public class BrokersController : Controller
    {
        private agendaEntities db = new agendaEntities();
        string regexName = @"^[A-Za-zéèàêâôûùïüç\-]+$";
        string regexMail = @"[0-9a-zA-Z\.\-]+@[0-9a-zA-Z\.\-]+.[a-zA-Z]{2,4}";
        string regexPhone = @"^[0][0-9]{9}";
        // GET: Brokers
        public ActionResult AddBroker()
        {
            return View("AddBroker");
        }
        [HttpPost]
        public ActionResult AddBroker([Bind(Include = "idBroker,lastname,firstname,mail,phoneNumber")] brokers brokerToAdd)
        {
            // vérification que le champ lastname n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToAdd.lastname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToAdd.lastname, regexName))
                // (valeur qu'on test, pattern) ! : si ça passe pas --> message erreur
                {
                    ModelState.AddModelError("lastname", "Ecrire un nom valide");
                    // on ajoute erreur sur lastname et message d'erreur
                }
            }
            else
            {
                ModelState.AddModelError("lastname", "Ecrire un nom");
            }
            // vérification que le champ firstname n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToAdd.firstname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToAdd.firstname, regexName))
                {
                    ModelState.AddModelError("firstname", "Ecrire un prénom valide");
                }
            }
            else
            {
                ModelState.AddModelError("firstname", "Ecrire une adresse mail");
            }
            // vérification que le champ mail n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToAdd.mail))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToAdd.mail, regexMail))
                {
                    ModelState.AddModelError("mail", "Ecrire une adresse mail valide");
                }
            }
            else
            {
                ModelState.AddModelError("mail", "Ecrire un mail");
            }
            // vérification que le champ phoneNumber n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToAdd.phoneNumber))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToAdd.phoneNumber, regexPhone))
                {
                    ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone valide");
                }
            }
            else
            {
                ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone");
            }
            // si il n'y a pas d'erreurs
            if (ModelState.IsValid)
            {
                db.brokers.Add(brokerToAdd); // insertion dans la bdd avec .Add
                db.SaveChanges(); // enregistrer les changements
                return RedirectToAction("SuccessAddBroker"); // rediriger vers une page de succès
            }
            else
            {
                return View(brokerToAdd); //réaffichage du formulaire
            }
        }
        public ActionResult SuccessAddBroker()
        {
            return View("SuccessAddBroker");
        }
        
        public ActionResult ListBrokers()
        {
            return View(db.brokers.ToList());
        }
        public ActionResult profilBroker(int? id) // ? : entier nullable
        {
            brokers brokerById = db.brokers.Find(id);
            if (brokerById == null)
            {
                return HttpNotFound();
            }
            return View(brokerById);
        }
        [HttpPost]
        public ActionResult profilBroker([Bind(Include = "idBroker,lastname,firstname,mail,phoneNumber")] brokers brokerToEdit)
        {
            // vérification que le champ lastname n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToEdit.lastname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToEdit.lastname, regexName))
                // (valeur qu'on test, pattern) ! : si ça passe pas --> message erreur
                {
                    ModelState.AddModelError("lastname", "Ecrire un nom valide");
                    // on ajoute erreur sur lastname et message d'erreur
                }
            }
            else
            {
                ModelState.AddModelError("lastname", "Ecrire un nom");
            }

            // vérification que le champ firstname n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToEdit.firstname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToEdit.firstname, regexName))
                {
                    ModelState.AddModelError("firstname", "Ecrire un prénom valide");
                }
            }
            else
            {
                ModelState.AddModelError("firstname", "Ecrire un prénom");
            }

            // vérification que le champ mail n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToEdit.mail))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToEdit.mail, regexMail))
                {
                    ModelState.AddModelError("mail", "Ecrire une adresse mail valide");
                }
            }
            else
            {
                ModelState.AddModelError("mail", "Ecrire un mail");
            }

            // vérification que le champ phoneNumber n'est pas null ou vide
            if (!String.IsNullOrEmpty(brokerToEdit.phoneNumber))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(brokerToEdit.phoneNumber, regexPhone))
                {
                    ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone valide");
                }
            }
            else
            {
                ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone");
            }

            // si il n'y a pas d'erreurs
            if (ModelState.IsValid)
            {
                db.Entry(brokerToEdit).State = EntityState.Modified;
                db.SaveChanges(); // enregistrer les changements
                return RedirectToAction("SuccessAddBroker"); // rediriger vers une page de succès
            }
            else
            {
                return View(brokerToEdit); //réaffichage du formulaire
            }
        }
        
    }
}