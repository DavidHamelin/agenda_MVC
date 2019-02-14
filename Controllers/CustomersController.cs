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
    public class CustomersController : Controller
    {
        private agendaEntities db = new agendaEntities();
        string regexName = @"^[A-Za-zéèàêâôûùïüç\-]+$";
        string regexMail = @"[0-9a-zA-Z\.\-]+@[0-9a-zA-Z\.\-]+.[a-zA-Z]{2,4}";
        string regexPhone = @"^[0][0-9]{9}";
        // GET: Customers
        public ActionResult AddCustomer()
        {
            return View("AddCustomer");
        }
        // Methode avec TryUpdateModel
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("AddCustomer")] // Nom de la méthode à utiliser
        public ActionResult AddCustomerPost()
        {
            customers customerToAdd = new customers();
            TryUpdateModel(customerToAdd);
            //vérification que le champ lastname n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToAdd.lastname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToAdd.lastname, regexName))
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
            if (!String.IsNullOrEmpty(customerToAdd.firstname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToAdd.firstname, regexName))
                {
                    ModelState.AddModelError("firstname", "Ecrire un prénom valide");
                }
            }
            else
            {
                ModelState.AddModelError("firstname", "Ecrire une adresse mail");
            }
            // vérification que le champ mail n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToAdd.mail))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToAdd.mail, regexMail))
                {
                    ModelState.AddModelError("mail", "Ecrire une adresse mail valide");
                }
            }
            else
            {
                ModelState.AddModelError("mail", "Ecrire un mail");
            }
            // vérification que le champ phoneNumber n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToAdd.phoneNumber))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToAdd.phoneNumber, regexPhone))
                {
                    ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone valide");
                }
            }
            else
            {
                ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone");
            }
            // pas de vérification pour le champ budget
            // pas de vérification pour le champ subject

            if (ModelState.IsValid)
            {
                db.customers.Add(customerToAdd); // insertion dans la bdd avec .Add
                db.SaveChanges(); // enregistrer les changements
                return RedirectToAction("SuccessAddCustomer"); // rediriger vers une page de succès
            }
            else
            {
                return View(customerToAdd); //réaffichage du formulaire
            }
        }
        //[HttpPost]
        //public ActionResult AddCustomer([Bind(Include = "idCustomer,lastname,firstname,mail,phoneNumber,budget,subject")] customers customerToAdd)
        //{
        //    // vérification que le champ lastname n'est pas null ou vide
        //    if (!String.IsNullOrEmpty(customerToAdd.lastname))
        //    {
        //        // Vérif de la validité de l'entrée
        //        if (!Regex.IsMatch(customerToAdd.lastname, regexName))
        //        // (valeur qu'on test, pattern) ! : si ça passe pas --> message erreur
        //        {
        //            ModelState.AddModelError("lastname", "Ecrire un nom valide");
        //            // on ajoute erreur sur lastname et message d'erreur
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("lastname", "Ecrire un nom");
        //    }
        //    // vérification que le champ firstname n'est pas null ou vide
        //    if (!String.IsNullOrEmpty(customerToAdd.firstname))
        //    {
        //        // Vérif de la validité de l'entrée
        //        if (!Regex.IsMatch(customerToAdd.firstname, regexName))
        //        {
        //            ModelState.AddModelError("firstname", "Ecrire un prénom valide");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("firstname", "Ecrire une adresse mail");
        //    }
        //    // vérification que le champ mail n'est pas null ou vide
        //    if (!String.IsNullOrEmpty(customerToAdd.mail))
        //    {
        //        // Vérif de la validité de l'entrée
        //        if (!Regex.IsMatch(customerToAdd.mail, regexMail))
        //        {
        //            ModelState.AddModelError("mail", "Ecrire une adresse mail valide");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("mail", "Ecrire un mail");
        //    }
        //    // vérification que le champ phoneNumber n'est pas null ou vide
        //    if (!String.IsNullOrEmpty(customerToAdd.phoneNumber))
        //    {
        //        // Vérif de la validité de l'entrée
        //        if (!Regex.IsMatch(customerToAdd.phoneNumber, regexPhone))
        //        {
        //            ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone valide");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone");
        //    }
        //    // pas de vérification pour le champ budget
        //    // pas de vérification pour le champ subject

        //    if (ModelState.IsValid)
        //    {
        //        db.customers.Add(customerToAdd); // insertion dans la bdd avec .Add
        //        db.SaveChanges(); // enregistrer les changements
        //        return RedirectToAction("SuccessAddCustomer"); // rediriger vers une page de succès
        //    }
        //    else
        //    {
        //        return View(customerToAdd); //réaffichage du formulaire
        //    }
        //}
        public ActionResult SuccessAddCustomer()
        {
            return View("SuccessAddCustomer");
        }
        public ActionResult ListCustomers()
        {
            return View(db.customers.ToList());
        }
        public ActionResult profilCustomer(int? id)
        {
            customers customers = db.customers.Find(id);
            if (customers == null)
            {
                return HttpNotFound();
            }
            return View(customers);
        }

        [HttpPost]
        public ActionResult profilCustomer([Bind(Include = "idCustomer,lastname,firstname,mail,phoneNumber,budget,subject")] customers customerToEdit)
        {
            //string regexBudget = @"[0-9]";

            // vérification que le champ lastname n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToEdit.lastname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToEdit.lastname, regexName))
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
            if (!String.IsNullOrEmpty(customerToEdit.firstname))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToEdit.firstname, regexName))
                {
                    ModelState.AddModelError("firstname", "Ecrire un prénom valide");
                }
            }
            else
            {
                ModelState.AddModelError("firstname", "Ecrire un prénom");
            }

            // vérification que le champ mail n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToEdit.mail))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToEdit.mail, regexMail))
                {
                    ModelState.AddModelError("mail", "Ecrire une adresse mail valide");
                }
            }
            else
            {
                ModelState.AddModelError("mail", "Ecrire un mail");
            }

            // vérification que le champ phoneNumber n'est pas null ou vide
            if (!String.IsNullOrEmpty(customerToEdit.phoneNumber))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToEdit.phoneNumber, regexPhone))
                {
                    ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone valide");
                }
            }
            else
            {
                ModelState.AddModelError("phoneNumber", "Ecrire un numéro de téléphone");
            }
            // vérification du champ budget
            //if ((customerToEdit.budget) != 0)
            //{
            //    // Vérif de la validité de l'entrée
            //    if (!Regex.IsMatch(customerToEdit.budget, regexBudget))
            //    {
            //        ModelState.AddModelError("budget", "Saisir un budget");
            //    }
            //}
            //else
            //{
            //    ModelState.AddModelError("budget", "Saisir un budget");
            //}

            // pas de vérification pour le champ subject

            // si il n'y a pas d'erreurs
            if (ModelState.IsValid)
            {
                db.Entry(customerToEdit).State = EntityState.Modified;
                db.SaveChanges(); // enregistrer les changements
                return RedirectToAction("SuccessAddCustomer"); // rediriger vers une page de succès
            }
            else
            {
                return View(customerToEdit); //réaffichage du formulaire
            }
        }
        // GET: Delete
        //public ActionResult ListCustomers(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    customers customersupp = db.customers.Find(id);
        //    if (customersupp == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(customersupp);
        //}
        // POST: Delete
        //[HttpPost, ActionName("ListCustomers")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteCustomerConfirm(int id)
        {
            customers customerDel = db.customers.Find(id);
            db.customers.Remove(customerDel);
            db.SaveChanges();
            return View("SuccessDeleteCustomer");
        }
        public ActionResult SuccessDeleteCustomer()
        {
            return View("SuccessDeleteCustomer");
        }
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}
    }
}