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
    [Authorize]
    public class UserProfileController : Controller
    {
        private IVRControlPanelEntities db = new IVRControlPanelEntities();

        //
        // GET: /UserProfile/
        
       public void Index()
       {
          // var users = db.Users.Include("Role");
           // return View(users.ToList());
           this.Details();
       }
        /*
       //
       // GET: /UserProfile/Details/5
       
       public ViewResult Details(int id)
       {
           User user = db.Users.Single(u => u.ID == id);
           return View(user);
       }
       */
        public ViewResult Details()
        {
            User user = db.Users.Single(u => u.UserName == User.Identity.Name);
            ViewData["CurrentPage"] = "profile";
            return View(user);
        }
        //
        // GET: /UserProfile/Create
        /*
        public ActionResult Create()
        {
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name");
            return View();
        } 

        //
        // POST: /UserProfile/Create

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.AddObject(user);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }
        */
        //
        // GET: /UserProfile/Edit/5
 
        public ActionResult Edit()
        {
            User user = db.Users.Single(u => u.UserName == User.Identity.Name);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", user.RoleID);
            
            return View(user);
        }

        //
        // POST: /UserProfile/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                db.Users.Attach(user);
                UpdateModel(user);
                //db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Details");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        //
        // GET: /UserProfile/Delete/5
 /*
        public ActionResult Delete(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            return View(user);
        }

        //
        // POST: /UserProfile/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            User user = db.Users.Single(u => u.ID == id);
            db.Users.DeleteObject(user);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
        */
        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}