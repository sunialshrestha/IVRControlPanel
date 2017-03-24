using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Models;
using System.IO;

namespace IVRControlPanel.Controllers
{ 
    public class ReportController : Controller
    {
        private IVRControlPanelEntities db = new IVRControlPanelEntities();

        //
        // GET: /Report/

        public ViewResult Index()
        {
            var playwavs = db.PlayWavs.Include("Category").Include("Language").Include("User");
            ViewData["CurrentPage"] = "Report";
            return View(playwavs.ToList());
        }

        //
        // GET: /Report/Details/5

        public ViewResult Details(int id)
        {
            PlayWav playwav = db.PlayWavs.Single(p => p.ID == id);
            return View(playwav);
        }

        //public ActionResult myaudio()
        //{
        //    var file = Server.MapPath("~/App_Data/test.wav");
        //    return base.File(file, "audio/wav");
        //}

        public ActionResult Download(string filename)
        {
            var document = Server.MapPath("~/App_Data/Uploads/" + filename);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
              //  FileName = document.FileName, 
                 FileName = Path.GetFileName(document),
                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false, 
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());
            return File(document, Path.GetExtension(document));
        }



        //
        // GET: /Report/Create

        public ActionResult Create()
        {
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName");
            ViewBag.LanguageID = new SelectList(db.Languages, "ID", "Language1");
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name");
            return View();
        } 

        //
        // POST: /Report/Create

        [HttpPost]
        public ActionResult Create(PlayWav playwav)
        {
            if (ModelState.IsValid)
            {
                db.PlayWavs.AddObject(playwav);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", playwav.CategoryID);
            ViewBag.LanguageID = new SelectList(db.Languages, "ID", "Language1", playwav.LanguageID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", playwav.UserID);
            return View(playwav);
        }
        
        //
        // GET: /Report/Edit/5
 
        public ActionResult Edit(int id)
        {
            PlayWav playwav = db.PlayWavs.Single(p => p.ID == id);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", playwav.CategoryID);
            ViewBag.LanguageID = new SelectList(db.Languages, "ID", "Language1", playwav.LanguageID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", playwav.UserID);
            return View(playwav);
        }

        //
        // POST: /Report/Edit/5

        [HttpPost]
        public ActionResult Edit(PlayWav playwav)
        {
            if (ModelState.IsValid)
            {
                db.PlayWavs.Attach(playwav);
                db.ObjectStateManager.ChangeObjectState(playwav, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "CategoryName", playwav.CategoryID);
            ViewBag.LanguageID = new SelectList(db.Languages, "ID", "Language1", playwav.LanguageID);
            ViewBag.UserID = new SelectList(db.Users, "ID", "Name", playwav.UserID);
            return View(playwav);
        }

        //
        // GET: /Report/Delete/5
 
        public ActionResult Delete(int id)
        {
            PlayWav playwav = db.PlayWavs.Single(p => p.ID == id);
            return View(playwav);
        }

        //
        // POST: /Report/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            PlayWav playwav = db.PlayWavs.Single(p => p.ID == id);
            db.PlayWavs.DeleteObject(playwav);
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