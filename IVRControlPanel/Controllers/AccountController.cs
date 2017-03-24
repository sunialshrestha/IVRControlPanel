using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using IVRControlPanel.Models;
using IVRControlPanel.Helpers;

namespace IVRControlPanel.Controllers
{
    public class AccountController : Controller
    {

        public IVRControlPanelMembershipProvider MembershipService { get; set; }
        public IVRControlPanelRoleProvider AuthorizationService { get; set; }

        protected override void Initialize(RequestContext requestContext)
        {
            if (MembershipService == null)
                MembershipService = new IVRControlPanelMembershipProvider();
            if (AuthorizationService == null)
                AuthorizationService = new IVRControlPanelRoleProvider();

            base.Initialize(requestContext);
        }

        //
        // GET: /Account/LogOn
        public ActionResult LogOn()
        {
            return View();
        }

        //
        // POST: /Account/LogOn
        [HttpPost]
        public ActionResult LogOn(LogOnModel model, string returnUrl)
        {
            returnUrl = HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["returnUrl"];


            if (ModelState.IsValid)
            {
                if (MembershipService.ValidateUser(model.UserName, model.Password))
                {

                    IVRControlPanelRepository _user = new IVRControlPanelRepository();

                    DateTime LastLogin = _user.LastLoginDate(model.UserName);
                    string imagename = _user.ImageName(model.UserName);

                    HttpCookie MyCookie = new HttpCookie("LoginInfo");
                    
                    MyCookie.Values["LastLogin"] = LastLogin.ToString();
                    MyCookie.Values["ImageName"] = imagename;
                    Response.Cookies.Add(MyCookie);

                    FormsAuthentication.SetAuthCookie(model.UserName, model.RememberMe);

                    var LastLoginIP = _user.LastLoginIP();

                    // Update: /Last Login Date
                    _user.UpdateLast(model.UserName, user =>
                    {
                        user.LastLoginDate = DateTime.Now;
                        user.LastLoginIp = LastLoginIP;
                    });
                    //_user.UpdateLast(model.UserName, user => user.LastLoginIp = LastLoginIP);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                      return RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    ModelState.AddModelError("", "The user name or password provided is incorrect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/LogOff
        public ActionResult LogOff()
        {

            IVRControlPanelRepository _user = new IVRControlPanelRepository();
            _user.UpdateLast(User.Identity.Name, u => u.LastLockedOutDate = DateTime.Now);

            FormsAuthentication.SignOut();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            List<SelectListItem> question = new List<SelectListItem>();

            //Add city names in city list.
            question.Add(new SelectListItem
            {
                Text = "What is your favourite food?",
                Value = "What is your favourite food?"
            });

            question.Add(new SelectListItem
            {
                Text = "What is your childhood nick name?",
                Value = "What is your childhood nick name?"
            });

            question.Add(new SelectListItem
            {
                Text = "What school did you attend for sixth grade?",
                Value = "What school did you attend for sixth grade?"
            });

          
            ViewBag.QuestionList = question;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                
                // Attempt to register the user
                try
                {
                    MembershipService.CreateUser(model.UserName, model.Name, model.Password, model.Email, "Member",model.Question, model.Answer);
                    
                    //FormsAuthentication.SetAuthCookie(model.UserName, false);
                    //return RedirectToAction("Index", "Home");
                    return RedirectToAction("Welcome", "Home");
                }
                catch (ArgumentException ae)
                {
                    ModelState.AddModelError("", ae.Message);
                }
            }

            List<SelectListItem> question = new List<SelectListItem>();

            //Add city names in city list.
            question.Add(new SelectListItem
            {
                Text = "What is your favourite food?",
                Value = "What is your favourite food?"
            });

            question.Add(new SelectListItem
            {
                Text = "What is your childhood nick name?",
                Value = "What is your childhood nick name?"
            });

            question.Add(new SelectListItem
            {
                Text = "What school did you attend for sixth grade?",
                Value = "What school did you attend for sixth grade?"
            });


            ViewBag.QuestionList = question;
           

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword
        [Authorize]
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Account/ChangePassword
        [Authorize]
        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                if (MembershipService.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword))
                    return RedirectToAction("ChangePasswordSuccess");
                else
                    ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePasswordSuccess
        public ActionResult ChangePasswordSuccess()
        {
            return View();
        }

        public ActionResult Activate(string username, string key)
        {
            IVRControlPanelRepository _user = new IVRControlPanelRepository();
            if (_user.ActivateUser(username, key) == false)
                return RedirectToAction("Index", "Home");
            else
                return RedirectToAction("LogOn");
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public ActionResult CheckUsername()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckUsername(FormCollection collection)
        {
            string username = collection["username"];
            string email = collection["email"];
            IVRControlPanelRepository _user = new IVRControlPanelRepository();
            if (_user.UserExists(username, email) == false)
            {
                return RedirectToAction("CheckUsername", "Account");
            }
            else
            {
                string question = _user.GetSecurityQuestion(username);
                ViewBag.question = question;
                ViewBag.username = username;
                ViewBag.email = email;

                return View("SecurityAnswerQuestion");
            }
        }

        [HttpPost]
        public ActionResult CheckAnswer(FormCollection collection)
        {
            string answer = collection["answer"];
            string question = collection["question"];
            string username = collection["username"];
            string email = collection["email"];

            IVRControlPanelRepository _user = new IVRControlPanelRepository();
            if (_user.CheckAnswer(username, question, answer) == false)
            {
                ViewBag.question = question;
                ViewBag.username = username;
                return View("SecurityAnswerQuestion");
            }
            else
            {
                _user.ResetLink(username, email);
                return View("LogOn");
            }

        }

        public ActionResult ResetPassword(string username, string tempPassword)
        {
            IVRControlPanelRepository _user = new IVRControlPanelRepository();
            if (_user.CheckTempPassword(username, tempPassword) == false)
                return RedirectToAction("LogOn");
            else
                ViewBag.username = username;
                return View("ResetPassword");

        }

        [HttpPost]
        public ActionResult EnterResetPassword(FormCollection collection)
        {
            string password = collection["password"];
            string confirmpassword = collection["confirmpassword"];
            string username = collection["username"];

            if ((password == confirmpassword) && password.Length > 6)
            {

                IVRControlPanelRepository _user = new IVRControlPanelRepository();
                if (_user.ChangeResetPassword(username, password) == true)
                    return RedirectToAction("ChangePasswordSuccess");
            }
  
            // If we got this far, something failed, redisplay form
            ViewBag.error = "Check the whether the password is matched or greater than 6 character";
            ViewBag.username = username;
            return View("ResetPassword");
        }

    }
}
