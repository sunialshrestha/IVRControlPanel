using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using IVRControlPanel.Models;

namespace IVRControlPanel.Controllers
{
    public class NewsUploadController : Controller
    {
        //
        // GET: /NewsUpload/
        IVRControlPanelEntities db = new IVRControlPanelEntities();
        IVRControlPanelRepository repository = new IVRControlPanelRepository();

        public ActionResult Index()
        {
            var query = db.Languages.Select(m => new { m.Language1, m.ID }).Distinct();
            ViewBag.lang = new SelectList(query.AsEnumerable(), "ID", "Language1");
            ViewData["CurrentPage"] = "Upload";
            return View();
        }

        /*
        [HttpPost]
        public ActionResult Index(UploadNewsModel newsmodel, IEnumerable<HttpPostedFileBase> news)
        {
            var filepath = Server.MapPath("~/App_Data/Uploads/News/");
            if (ModelState.IsValid)
            {
                foreach (var file in news)
                {
                    if (file != null && file.ContentLength > 0)
                    {
                        var fileName = Path.GetFileName(file.FileName);
                        var serverpath = Server.MapPath("~/App_Data/uploads/News");
                        var path = Path.Combine(serverpath, fileName);
                        if (!Directory.Exists(serverpath))
                        {
                            Directory.CreateDirectory(serverpath);
                        }

                        file.SaveAs(path);
                    }

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + "news" + Request["file"];
                    var filename = filenam.Replace(" ", "_");
                    var wav = new PlayWav
                    {
                        Name = filename,
                        CategoryID = int.Parse("1"),
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = int.Parse("1"),
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(newsmodel.ActiveDateTime),
                        FilePath = filepath
                    };
                     db.AddToPlayWavs(wav);
                }
            }
            var query = db.Languages.Select(m => new { m.Language1, m.ID }).Distinct();
            ViewBag.lang = new SelectList(query.AsEnumerable(), "ID", "Language1");
            ViewData["CurrentPage"] = "Upload";
            return View(news);
        }
        */

        [HttpPost]
        public ActionResult Index(UploadNewsModel newsmodel)
        {
            /*      if (ModelState.IsValid)
       {
           foreach (var file in newsmodel.news)
           {
               if (file != null && file.ContentLength > 0)
               {
                   var fileName = Path.GetFileName(file.FileName);
                   var serverpath = Server.MapPath("~/App_Data/uploads/News");
                   var path = Path.Combine(serverpath, fileName);
                   if (!Directory.Exists(serverpath))
                   {
                       Directory.CreateDirectory(serverpath);
                   }

                   file.SaveAs(path);
               }

           }
       }
  * */

            HttpPostedFileBase general = newsmodel.GeneralNews;
            HttpPostedFileBase sport = newsmodel.SportNews;
            HttpPostedFileBase business = newsmodel.BusiNews;
            HttpPostedFileBase international = newsmodel.InterNews;
            HttpPostedFileBase entertaintment = newsmodel.EntertaintNews;

            var lang = newsmodel.Language;
            var activeDate = newsmodel.ActiveDateTime;
            

            if (ModelState.IsValid)
            {
                if (general != null && general.ContentLength > 0)
                {
                  
                    var filenames = Path.GetFileName(general.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 1 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/News/General News/"), fileName);
                    general.SaveAs(path);
                    var wav1 = new PlayWav
                    {
                        Name = fileName,
                        CategoryID = 1,
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = lang,
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(activeDate),
                        FilePath = path
                    };
                    db.AddToPlayWavs(wav1);
                    db.SaveChanges();
                }
               /* else
                {
                    ModelState.AddModelError("GenearlNews", "The file should not be null.");
                }
                * */
                if (sport != null && sport.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(general.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 2 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/News/Sport News/"), fileName);
                    sport.SaveAs(path);
                    var wav2 = new PlayWav
                    {
                        Name = fileName,
                        CategoryID = 2,
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = lang,
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(activeDate),
                        FilePath = path
                    };
                    db.AddToPlayWavs(wav2);
                    db.SaveChanges();
                }
               /*  else
                {
                    ModelState.AddModelError("SportNews", "The file should not be null.");
                }
                * */
                if (business != null && business.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(general.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 3 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/News/Business News/"), fileName);
                    business.SaveAs(path);
                    var wav3 = new PlayWav
                    {
                        Name = fileName,
                        CategoryID = 3,
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = lang,
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(activeDate),
                        FilePath = path
                    };
                    db.AddToPlayWavs(wav3);
                    db.SaveChanges();
                }
                    /* 
                else
                {
                    ModelState.AddModelError("BusiNews", "The file should not be null.");
                }
                     * */
                if (international != null && international.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(general.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 4 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");  

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/News/International News/"), fileName);
                    international.SaveAs(path);
                    var wav4 = new PlayWav
                    {
                        Name = fileName,
                        CategoryID = 4,
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = lang,
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(activeDate),
                        FilePath = path
                    };
                    db.AddToPlayWavs(wav4);
                    db.SaveChanges();
                }
                    /*
                else
                {
                    ModelState.AddModelError("InterNews", "The file should not be null.");
                }
                     * */
                if ( entertaintment != null && entertaintment.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(general.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 5 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/News/Entertaintment News/"), fileName);
                    entertaintment.SaveAs(path);

                    var wav5 = new PlayWav
                    {
                        Name = fileName,
                        CategoryID = 5,
                        UserID = repository.GetUserID(HttpContext.User.Identity.Name),
                        LanguageID = lang,
                        UploadDateTime = DateTime.Now,
                        ActiveDateTime = Convert.ToDateTime(activeDate),
                        FilePath = path
                    };
                    db.AddToPlayWavs(wav5);
                    db.SaveChanges();
                }
              
                    /*
                else
                {
                    ModelState.AddModelError("EntertaintNews", "The file should not be null.");
                }
                     * */
            }


            var query = db.Languages.Select(m => new { m.Language1, m.ID }).Distinct();
            ViewBag.lang = new SelectList(query.AsEnumerable(), "ID", "Language1");
            ViewData["CurrentPage"] = "Upload";
            return View(newsmodel);
        }
    }
}