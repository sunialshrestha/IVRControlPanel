using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Models;
using System.IO;

namespace IVRControlPanel.Controllers
{
    public class HoroscopeUploadController : Controller
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
        [HttpPost]
        public ActionResult Index(HoroscopeModel horoscopemodel)
        {
            HttpPostedFileBase aries = horoscopemodel.Aries;
            HttpPostedFileBase taurus = horoscopemodel.Taurus;
            HttpPostedFileBase gemini = horoscopemodel.Gemini;
            HttpPostedFileBase cancer = horoscopemodel.Cancer;
            HttpPostedFileBase leo = horoscopemodel.Leo;
            HttpPostedFileBase virgo = horoscopemodel.Virgo;
            HttpPostedFileBase libra = horoscopemodel.Libra;
            HttpPostedFileBase scorpio = horoscopemodel.Scorpio;
            HttpPostedFileBase sagittarius = horoscopemodel.Sagittarius;
            HttpPostedFileBase capricorn = horoscopemodel.Capricorn;
            HttpPostedFileBase aquarius = horoscopemodel.Aquarius;
            HttpPostedFileBase pisces = horoscopemodel.Pisces;

            var lang = horoscopemodel.Language;
            var activeDate = horoscopemodel.ActiveDateTime;


            if (ModelState.IsValid)
            {
                if (aries != null && aries.ContentLength > 0)
                {

                    var filenames = Path.GetFileName(aries.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 1 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/aries/"), fileName);
                    aries.SaveAs(path);
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

                if (taurus != null && taurus.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(taurus.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 2 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/taurus/"), fileName);
                    taurus.SaveAs(path);
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

                if (gemini != null && gemini.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(gemini.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 3 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/gemini/"), fileName);
                    gemini.SaveAs(path);
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
  
                if (cancer != null && cancer.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(cancer.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 4 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/cancer/"), fileName);
                    cancer.SaveAs(path);
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

                if (leo != null && leo.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(leo.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 5 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/leo/"), fileName);
                    leo.SaveAs(path);

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

                if (virgo != null && virgo.ContentLength > 0)
                {

                    var filenames = Path.GetFileName(virgo.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 6 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/virgo/"), fileName);
                    virgo.SaveAs(path);
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

                if (libra != null && libra.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(libra.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 7 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");
                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/libra/"), fileName);
                    libra.SaveAs(path);
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

                if (scorpio != null && scorpio.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(scorpio.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 8 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/scorpio/"), fileName);
                    scorpio.SaveAs(path);
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

                if (sagittarius != null && sagittarius.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(sagittarius.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 9 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/sagittarius/"), fileName);
                    sagittarius.SaveAs(path);
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

                if (capricorn != null && capricorn.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(capricorn.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 9 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/capricorn/"), fileName);
                    capricorn.SaveAs(path);
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

                if (aquarius != null && aquarius.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(aquarius.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 10 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/aquarius/"), fileName);
                    aquarius.SaveAs(path);

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
                if (pisces != null && pisces.ContentLength > 0)
                {
                    var filenames = Path.GetFileName(pisces.FileName);

                    var filenam = DateTime.Now.ToString("yyyyMMddhhmmss") + 11 + lang + filenames;
                    var fileName = filenam.Replace(" ", "_");

                    var path = Path.Combine(Server.MapPath("~/App_Data/uploads/Horoscope/Daily Horoscope/pisces/"), fileName);
                    pisces.SaveAs(path);

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
            }


            var query = db.Languages.Select(m => new { m.Language1, m.ID }).Distinct();
            ViewBag.lang = new SelectList(query.AsEnumerable(), "ID", "Language1");
            ViewData["CurrentPage"] = "Upload";
            return View(horoscopemodel);
        }

    }
}
