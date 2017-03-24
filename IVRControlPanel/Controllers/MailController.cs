using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ActionMailer.Net.Mvc;

namespace IVRControlPanel.Controllers
{
    public class MailController : MailerBase
    {
        public EmailResult SampleEmail()
        {
            From = "sunilshrestha59@gmail.com";
            To.Add("sunilshrestha59@gmail.com");
            Subject = "Welcome to ActionMailer.ne!";
            return Email("SampleEmail");

        }
    }
}
