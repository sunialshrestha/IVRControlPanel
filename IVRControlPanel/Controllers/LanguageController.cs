using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Models;

namespace IVRControlPanel.Controllers
{ 
    public class LanguageController : Controller
    {
        private IVRControlPanelEntities db = new IVRControlPanelEntities();

        //
        // GET: /Language/

        public ViewResult Index()
        {
            ViewData["CurrentPage"] = "Category";
            return View(db.Languages.ToList());
        }

        //
        // GET: /Language/Details/5

        public ViewResult Details(int id)
        {
            Language language = db.Languages.Single(l => l.ID == id);
            return View(language);
        }

        //
        // GET: /Language/Create

        public ActionResult Create()
        {
            return View();
        } 

        //
        // POST: /Language/Create

        [HttpPost]
        public ActionResult Create(Language language)
        {
            if (ModelState.IsValid)
            {
                db.Languages.AddObject(language);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            return View(language);
        }
        
        //
        // GET: /Language/Edit/5
 
        public ActionResult Edit(int id)
        {
            Language language = db.Languages.Single(l => l.ID == id);
            return View(language);
        }

        //
        // POST: /Language/Edit/5

        [HttpPost]
        public ActionResult Edit(Language language)
        {
            if (ModelState.IsValid)
            {
                db.Languages.Attach(language);
                db.ObjectStateManager.ChangeObjectState(language, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(language);
        }

        //
        // GET: /Language/Delete/5
 
        public ActionResult Delete(int id)
        {
            Language language = db.Languages.Single(l => l.ID == id);
            return View(language);
        }

        //
        // POST: /Language/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            Language language = db.Languages.Single(l => l.ID == id);
            db.Languages.DeleteObject(language);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}