using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Models;
using MvcSiteMapProvider;

namespace IVRControlPanel.Controllers
{
    public class CategoryController : Controller
    {
        private IVRControlPanelEntities db = new IVRControlPanelEntities();
        private IVRControlPanelRepository repo = new IVRControlPanelRepository();
        List<Category> categories = new List<Category>();

        //
        // GET: /Category/
        public ViewResult Index()
        {
            List<Category> cat = this.display_children(null, 0);
            ViewData["CurrentPage"] = "Category";
            //return View(db.Categories.ToList());
            return View(cat);
        }

        //
        // GET: /Category/Details/5

        public ViewResult Details(int id)
        {
            Category category = db.Categories.Single(c => c.ID == id);
            return View(category);
        }

        public List<Category> display_children(int? parent, int level) 
        {

            var cat = (parent == null) ? from u in db.Categories.Where(x => object.Equals(x.ParentCategoryID, parent)) select u : from u in db.Categories where (u.ParentCategoryID == parent) select u;
           
               //var cat = from u in db.Categories where (u.ParentCategoryID == parent) select u;
                    
            
           
            foreach(var item in cat)
            {
                string tabs = new String('-', level);
                categories.Add(new Category() { ID = item.ID, CategoryName = tabs + item.CategoryName, ParentCategoryID = item.ParentCategoryID });
                display_children(item.ID, level + 1);
            }
            return categories;
        }

        [Authorize(Roles = "administrator")]
        public ActionResult Create()
        {
            List<Category> cat = this.display_children(null, 0);
            List<SelectListItem> items = new SelectList(cat, "ID", "CategoryName").ToList();
            items.Insert(0, (new SelectListItem { Text = "root", Value = "" }));

            //ViewBag.ParentCategoryID = new SelectList(cat, "ID", "CategoryName");
            ViewBag.ParentCategoryID = items;
            return View();
        } 

        //
        // POST: /Category/Create

        [HttpPost]
        public ActionResult Create(Category category)
        {
            

            if (ModelState.IsValid)
            {
                if (repo.CategoryExist(category.CategoryName, category.ParentCategoryID))
                {
                    ModelState.AddModelError("CategoryName", "The Category already exist, duplicate category can not be added");
                    List<Category> cats = this.display_children(null, 0);
                    List<SelectListItem> itemss = new SelectList(cats, "ID", "CategoryName").ToList();
                    itemss.Insert(0, (new SelectListItem { Text = "root", Value = "" }));

                    //ViewBag.ParentCategoryID = new SelectList(cat, "ID", "CategoryName");
                    ViewBag.ParentCategoryID = itemss;
                    return View(category);

                }
                db.Categories.AddObject(category);
                db.SaveChanges();
                return RedirectToAction("Index");  
            }

            List<Category> cat = this.display_children(null, 0);
            List<SelectListItem> items = new SelectList(cat, "ID", "CategoryName").ToList();
            items.Insert(0, (new SelectListItem { Text = "root", Value = "" }));

            //ViewBag.ParentCategoryID = new SelectList(cat, "ID", "CategoryName");
            ViewBag.ParentCategoryID = items;
            return View(category);
        }
        
        //
        // GET: /Category/Edit/5
        [Authorize(Roles = "administrator")]
        public ActionResult Edit(int id)
        {
            Category category = db.Categories.Single(c => c.ID == id);
            List<Category> cat = this.display_children(null, 0);
            List<SelectListItem> items = new SelectList(cat, "ID", "CategoryName",category.ParentCategoryID).ToList();
            items.Insert(0, (new SelectListItem { Text = "root", Value = "" }));

            ViewBag.ParentCategoryID = items;
            
           
            return View(category);
        }

        //
        // POST: /Category/Edit/5

        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                db.Categories.Attach(category);
                db.ObjectStateManager.ChangeObjectState(category, EntityState.Modified);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return this.Edit(category.ID);
        }

        public List<Category> GetChildCategory(int? parent)
        {
            var cat = (parent == null) ? from u in db.Categories.Where(x => object.Equals(x.ParentCategoryID, parent)) select u : from u in db.Categories where (u.ParentCategoryID == parent) select u;

            foreach (var item in cat)
            {
                categories.Add(item);
                GetChildCategory(item.ID);
            }
            return categories;
        }


        public List<Category> GetParentCategory(int? id)
        {

            var cat = from u in db.Categories where (u.ID == id) select u;

            foreach (var item in cat)
            {
                categories.Add(item);
                if (item.ParentCategoryID == null)
                {
                    break;
                }
                GetParentCategory(item.ParentCategoryID);

            }
            return categories;
        }

        //
        // GET: /Category/Delete/5
        [Authorize(Roles = "administrator")]
        public ActionResult Delete(int id)
        {
           // Category category = db.Categories.Single(c => c.ID == id);
            List<Category> category = GetChildCategory(id);
            Category rootcategory = db.Categories.Single(c => c.ID == id);
            category.Add(rootcategory);

            return View(category);
        }

        //
        // POST: /Category/Delete/5


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {            
           
            List<Category> category = GetChildCategory(id);
            Category rootcategory = db.Categories.Single(c => c.ID == id);
            category.Add(rootcategory);

            foreach (var item in category)
            {
                db.Categories.DeleteObject(item);
            }

            
            //db.Categories.DeleteObject(category);
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