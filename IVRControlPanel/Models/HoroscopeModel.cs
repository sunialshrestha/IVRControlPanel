using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IVRControlPanel.Models
{
    public class HoroscopeModel
    {
        public DateTime ActiveDateTime { get; set; }

        public int Language { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Aries { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Taurus { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Gemini { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Cancer { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Leo { get; set; }


        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Virgo { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Libra { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Scorpio { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Sagittarius { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Capricorn { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Aquarius { get; set; }

        [File(AllowedFileExtensions = new string[] { ".jpg", ".gif", ".tiff", ".png", ".pdf" }, MaxContentLength = 1024 * 1024 * 8, ErrorMessage = "Invalid File")]
        public HttpPostedFileBase Pisces { get; set; }
    }
}