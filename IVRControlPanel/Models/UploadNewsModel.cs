using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Collections;

namespace IVRControlPanel.Models
{
    public class UploadNewsModel
    {
      public DateTime ActiveDateTime { get; set; }

      public int Language { get; set; }

      //[File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
      //public IList<HttpPostedFileBase> news { get; set; }
      //public HttpPostedFileBase news { get; set; }
      [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase GeneralNews { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
      public HttpPostedFileBase SportNews { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
      public HttpPostedFileBase BusiNews { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
      public HttpPostedFileBase InterNews { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
      public HttpPostedFileBase EntertaintNews { get; set; }
    }


   

}