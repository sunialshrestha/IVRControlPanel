using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVRControlPanel.Controllers
{
    public class ErrorsController : Controller
    {
        //
        // GET: /Errors/

        public ActionResult General(Exception exception)
        {
           // return Content("General failure", "text/plain");
            return View("CustomError");
        }

        public ActionResult Http404()
        {
           // return Content("Not found", "text/plain");
            return View("CustomError");
        }

        public ActionResult Http403()
        {
            //return Content("Forbidden", "text/plain");
            return View("CustomError");
        }


    }
}
