using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using IVRControlPanel.Helpers;
using System.IO;

namespace IVRControlPanel.Controllers
{
    public class UploaderController : Controller
    {
        //
        // GET: /Uploader/

        public ActionResult Index()
        {
            return View();
        }

     /*   [HttpPost]
        public WrappedJsonResult UploadWav(HttpPostedFileBase wavfile)
        {
            if (wavfile == null || wavfile.ContentLength == 0)
            {
                return new WrappedJsonResult
                {
                    Data = new
                    {
                        IsValid = false,
                        Message = "No file was uploaded.",
                        ImagePath = string.Empty
                    }
                };

            }

            var fileName = String.Format("{0}.mp3", Guid.NewGuid().ToString());
            var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Content/UserImages")), fileName);

            wavfile.SaveAs(imagePath);

            return new WrappedJsonResult
            {
                Data = new
                {
                    IsValid = true,
                    Message = string.Empty,
                    ImagePath = Url.Content(String.Format("~/Content/UserImages/{0}", fileName))
                }
            };
        }
       */
        [HttpPost]
        public WrappedJsonResult UploadImage(HttpPostedFileWrapper imageFile)
        {

            if (imageFile == null || imageFile.ContentLength == 0)
            {
                return new WrappedJsonResult
                {
                    Data = new
                    {
                        IsValid = false,
                        Message = "No file was uploaded.",
                        ImagePath = string.Empty
                    }
                };
            }

            var fileName = String.Format("{0}.jpg", Guid.NewGuid().ToString());
            var imagePath = Path.Combine(Server.MapPath(Url.Content("~/Content/UserImages")), fileName);

            imageFile.SaveAs(imagePath);

            return new WrappedJsonResult
            {
                Data = new
                {
                    IsValid = true,
                    Message = string.Empty,
                    ImagePath = Url.Content(String.Format("~/Content/UserImages/{0}", fileName))
                }
            };
        }
        
    }
}
