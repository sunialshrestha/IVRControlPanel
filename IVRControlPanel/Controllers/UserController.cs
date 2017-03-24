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
    [Authorize(Roles = "administrator")]
    public class UserController : Controller
    {
        private IVRControlPanelEntities db = new IVRControlPanelEntities();
        

        //
        // GET: /User/
        public ViewResult Index()
        {
            var users = db.Users.Include("Role");
            ViewData["CurrentPage"] = "User";
            return View(users.ToList());
        }

        //
        // GET: /User/Details/5

        public ViewResult Details(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            return View(user);
        }

        //
        // GET: /User/Edit/5
 
        public ActionResult Edit(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        //
        // POST: /User/Edit/5

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                
                db.Users.Attach(user);

                IVRControlPanelRepository repository = new IVRControlPanelRepository();
                repository.UpdateLast(user.UserName, u => { u.LastModifiedDate = DateTime.Now; });

                UpdateModel(user);
                //db.ObjectStateManager.ChangeObjectState(user, EntityState.Modified);
                
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.RoleID = new SelectList(db.Roles, "ID", "Name", user.RoleID);
            return View(user);
        }

        //
        // GET: /User/Delete/5
 
        public ActionResult Delete(int id)
        {
            User user = db.Users.Single(u => u.ID == id);
            return View(user);
        }

        //
        // POST: /User/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
            User user = db.Users.Single(u => u.ID == id);
            db.Users.DeleteObject(user);
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