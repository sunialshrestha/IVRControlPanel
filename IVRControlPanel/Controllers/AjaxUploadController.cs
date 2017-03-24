using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IVRControlPanel.Models;

namespace IVRControlPanel.Controllers
{
    [Authorize]
    public class AjaxUploadController : Controller
    {
        //
        // GET: /AjaxUpload/
        IVRControlPanelRepository repository = new IVRControlPanelRepository();
        IVRControlPanelEntities db = new IVRControlPanelEntities();
        public ActionResult Index()
        {
            CategoryController category = new CategoryController();
            List<Category> cat = category.display_children(null, 0);
            List<SelectListItem> items = new SelectList(cat, "ID", "CategoryName").ToList();
            ViewBag.ParentCategoryID = items;

            var query = db.Languages.Select(m => new { m.Language1, m.ID }).Distinct();
            ViewBag.lang = new SelectList(query.AsEnumerable(), "ID", "Language1");
            ViewData["CurrentPage"] = "Upload";

            return View();


        }

        [HttpPost]
        public ActionResult Upload(HttpPostedFileBase qqfile, string param1, string param2, string param3)
        {
            //retreiving parent category 
            CategoryController category = new CategoryController();
            var parentcategory = category.GetParentCategory(int.Parse(param2));


            string categorypath = string.Empty;

            parentcategory.Reverse();

            foreach (var item in parentcategory)
            {
                categorypath += item.CategoryName + "/";
            }
            categorypath.Replace(" ", "_");

            // Rename the file name
            var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + param1 + param2 + Request["qqfile"];
            var filename = filenam.Replace(" ", "_");

            // create the directory
            String path = Server.MapPath("~/App_Data/Uploads/") + categorypath;

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            var filepath = Path.Combine(path, Path.GetFileName(filename));
            //var filepath = Path.Combine(Server.MapPath("~/App_Data/Uploads"), Path.GetFileName(filename));
          

            if (param2 != null || param2 != "")
            {
                var cat = repository.GetCategoryName(int.Parse(param2));
                var lang = repository.GetLang(int.Parse(param1));
                var parent = repository.GetParentCategory(int.Parse(param2));

                var wav = new PlayWav
                {
                    Name = filename,
                    CategoryID = int.Parse(param2),
                    UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                    LanguageID = int.Parse(param1),
                    UploadDateTime = DateTime.Now,
                    ActiveDateTime = Convert.ToDateTime(param3),
                    FilePath = filepath
                };

                db.AddToPlayWavs(wav);


                if (qqfile != null)
                {
                    // this works for IE
                    //var filenamess = Path.Combine(Server.MapPath("~/App_Data/Uploads"), Path.GetFileName(qqfile.FileName));
                    //var filenamess = Path.Combine(Server.MapPath("~/App_Data/Uploads"), Path.GetFileName(filename));
                    qqfile.SaveAs(filepath);

                    db.SaveChanges();

                    return Json(new { success = true, filename, lang, cat, parent }, "text/html");
                }
                else
                {
                    // this works for Firefox, Chrome
                    //var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + Request["qqfile"];
                    //var filename = filenam.Replace(" ", "_");
                    if (!string.IsNullOrEmpty(filepath))
                    {

                        // filename = Path.Combine(Server.MapPath("~/App_Data/Uploads"), Path.GetFileName(filename));
                        using (var output = System.IO.File.Create(filepath))
                        {
                            Request.InputStream.CopyTo(output);
                        }

                        db.SaveChanges();

                        return Json(new { success = true, filename, lang, cat, parent });
                    }
                }
            }
            return Json(new { success = false });
        }
    }

}
