﻿using Agenda.Models;
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
        string regexSubject = @"^[A-Za-zéèêëâäàçîïôö&-.,'\ ]+$";
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
                ModelState.AddModelError("firstname", "Ecrire un prénom");
            }
            // Vérification que le champs mail n'est pas null, vide ou identique au mail d'un autre client
            if (!String.IsNullOrEmpty(customerToAdd.mail))
            {
                // Vérification de la validité de l'entrée
                var isAlreadyUsed = db.customers.Where(cus => cus.mail == customerToAdd.mail).SingleOrDefault(); // valeur null par defaut
                if (!Regex.IsMatch(customerToAdd.mail, regexMail))
                {
                    // Message d'erreur
                    ModelState.AddModelError("mail", "Ecrire un mail valide");
                }
                // Vérifie si ce mail est déjà utilisé
                else if (isAlreadyUsed != null)
                {
                    ModelState.AddModelError("mail", "Un client existant porte cette adresse mail");
                }
            }
            else
            {
                // Message d'erreur
                ModelState.AddModelError("mail", "Ecrire un email");
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
            // vérification du champ budget
            if (customerToAdd.budget <= 0)
            {
                ModelState.AddModelError("budget", "budget non valide");
            }
            // vérification pour le champ subject
            if (!String.IsNullOrEmpty(customerToAdd.subject))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToAdd.subject, regexSubject))
                {
                    ModelState.AddModelError("subject", "Ecrire un sujet valide");
                }
            }
            else
            {
                ModelState.AddModelError("subject", "Ecrire un sujet");
            }
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

        public ActionResult SuccessAddCustomer()
        {
            return View("SuccessAddCustomer");
        }
        // Methode avec SqlQuery
        public ActionResult ListCustomers()
        {
            var request = "SELECT [idCustomer], [lastname], [firstname], [mail], [phoneNumber], [budget], [subject] " +
                "FROM [dbo].[customers] " +
                "ORDER BY [lastname] DESC";
            var listCustomers = db.customers.SqlQuery(request);
            //ViewData.Model = db.customers.SqlQuery(request);
            //return View("ListCustomers");
            return View(listCustomers);
        }
        // methode sans ToList()
        //public ActionResult ListCustomers()
        //{
        //    return View(db.customers);
        //}
        //public ActionResult ListCustomers()
        //{
        //    return View(db.customers.ToList());
        //}
        // Infos client
        public ActionResult profilCustomer(int? id)
        {
            customers customers = db.customers.Find(id);
            if (customers == null || id == null)
            {
                return HttpNotFound(); // renvoyer vers page d'erreur
            }
            return View(customers);
        }

        [HttpPost]
        public ActionResult profilCustomer([Bind(Include = "idCustomer,lastname,firstname,mail,phoneNumber,budget,subject")] customers customerToEdit)
        {
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
            // Vérification que le champs mail n'est pas null, vide ou identique au mail d'un autre client
            if (!String.IsNullOrEmpty(customerToEdit.mail))
            {
                // Vérification de la validité de l'entrée
                var isAlreadyUsed = db.customers.Where(cus => cus.mail == customerToEdit.mail).SingleOrDefault(); // valeur null par defaut
                if (!Regex.IsMatch(customerToEdit.mail, regexMail))
                {
                    // Message d'erreur
                    ModelState.AddModelError("mail", "Ecrire un mail valide");
                }
                // Vérifie si ce mail est déjà utilisé
                else if (isAlreadyUsed != null)
                {
                    ModelState.AddModelError("mail", "Un client existant porte cette adresse mail");
                }
            }
            else
            {
                // Message d'erreur
                ModelState.AddModelError("mail", "Ecrire un email");
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
            if (customerToEdit.budget <= 0)
            {
                ModelState.AddModelError("budget", "budget non valide");
            }
            // vérification pour le champ subject
            if (!String.IsNullOrEmpty(customerToEdit.subject))
            {
                // Vérif de la validité de l'entrée
                if (!Regex.IsMatch(customerToEdit.subject, regexSubject))
                {
                    ModelState.AddModelError("subject", "Ecrire un sujet valide");
                }
            }
            else
            {
                ModelState.AddModelError("subject", "Ecrire un sujet");
            }

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
        //[ActionName="ListCustomers"]
        //public ActionResult SearchBar()
        //{
        //    var cus = Request.Form("searchBar");
        //    var request = "SELECT [idCustomer], [lastname], [firstname], [mail], [phoneNumber], [budget], [subject] " +
        //        "FROM [dbo].[customers] " +
        //        "WHERE [lastname] =" + cus;
        //    var searchCus = db.customers.SqlQuery(request);
        //    return View(searchCus);
        //}

        [HttpGet, ActionName("ListCustomers")]
        public ActionResult SearchBar(string option, string search)
        {
            if (option == "Subject")
            {
                //return View(db.customers.Where(x => x.subject == search || search == null).ToList());
                return View(db.customers.Where(x => x.subject.StartsWith(search) || search == null).ToList());
            }
            else if (option == "FirstName")
            {
                //return View(db.customers.Where(x => x.firstname == search || search == null).ToList());
                return View(db.customers.Where(x => x.firstname.StartsWith(search) || search == null).ToList());
            }
            else
            {
                return View(db.customers.Where(x => x.lastname.StartsWith(search) || search == null).ToList());
            }
        }
    }
}