using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IVRControlPanel.Controllers
{
    public class SendEmailController : Controller
    {
        //
        // GET: /SendEmail/

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SendEmail()
        {
            new MailController().SampleEmail().DeliverAsync();
            return View();
        }

    }
}
