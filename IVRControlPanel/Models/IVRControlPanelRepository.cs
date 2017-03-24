using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Security.Cryptography;
using System.Net.Mail;
using System.Net;
using IVRControlPanel.Controllers;
using IVRControlPanel.Helpers;
using System.Web.Mvc;
using System.Data.Objects.SqlClient;
using System.Globalization;

namespace IVRControlPanel.Models
{
    public class IVRControlPanelRepository
    {

        #region Variables

        private IVRControlPanelEntities entities = new IVRControlPanelEntities();

        private const string MissingRole = "Role does not exist";
        private const string MissingUser = "User does not exist";
        private const string TooManyUser = "User already exists";
        private const string TooManyRole = "Role already exists";
        private const string AssignedRole = "Cannot delete a role with assigned users";

        #endregion

        #region Properties

        public int NumberOfUsers
        {
            get
            {
                return this.entities.Users.Count();
            }
        }

        public int NumberOfRoles
        {
            get
            {
                return this.entities.Roles.Count();
            }
        }

        #endregion

        #region Constructors

        public IVRControlPanelRepository()
        {
            this.entities = new IVRControlPanelEntities();
        }

        #endregion

        #region Query Methods

        public IQueryable<User> GetAllUsers()
        {
            return from user in entities.Users
                   orderby user.UserName
                   select user;
        }

        public IQueryable<User> GetLatestUsers()
        {

            return (from u in entities.Users where u.CreatedDate  <= DateTime.Now 
                        orderby u.CreatedDate descending 
                        select u).Take(3);
        }

        public User GetUser(int id)
        {
            return entities.Users.SingleOrDefault(user => user.ID == id);
        }

        public User GetUser(string userName)
        {
            return entities.Users.SingleOrDefault(user => user.UserName == userName);
        }


        public IQueryable<User> GetUsersForRole(string roleName)
        {
            return GetUsersForRole(GetRole(roleName));
        }

        public IQueryable<User> GetUsersForRole(int id)
        {
            return GetUsersForRole(GetRole(id));
        }

        public IQueryable<User> GetUsersForRole(Role role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            return from user in entities.Users
                   where user.RoleID == role.ID
                   orderby user.UserName
                   select user;
        }

        public IQueryable<Role> GetAllUserRoles()
        {
            return from role in entities.Roles
                   orderby role.Name
                   select role;
        }

        public Role GetRole(int id)
        {
            return entities.Roles.SingleOrDefault(role => role.ID == id);
        }

        public Role GetRole(string name)
        {
            return entities.Roles.SingleOrDefault(role => role.Name == name);
        }

        public Role GetRoleForUser(string userName)
        {
            return GetRoleForUser(GetUser(userName));
        }

        public Role GetRoleForUser(int id)
        {
            return GetRoleForUser(GetUser(id));
        }

        public Role GetRoleForUser(User user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            return user.Role;
        }

        #endregion

        #region Insert/Delete

        private void AddUser(User user)
        {
            if (UserExists(user))
                throw new ArgumentException(TooManyUser);

            entities.Users.AddObject(user);
        }

        public void CreateUser(string username, string name, string password, string email, string roleName, string question, string answer)
        {
            Role role = GetRole(roleName);

            if (string.IsNullOrEmpty(username.Trim()))
                throw new ArgumentException("The user name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(name.Trim()))
                throw new ArgumentException("The name provided is invalid. Please check the value and try again.");
            if (string.IsNullOrEmpty(password.Trim()))
                throw new ArgumentException("The password provided is invalid. Please enter a valid password value.");
            if (string.IsNullOrEmpty(email.Trim()))
                throw new ArgumentException("The e-mail address provided is invalid. Please check the value and try again.");
            if (!RoleExists(role))
                throw new ArgumentException("The role selected for this user does not exist! Contact an administrator!");
            if (this.entities.Users.Any(user => user.UserName == username))
                throw new ArgumentException("Username already exists. Please enter a different user name.");
            if (this.entities.Users.Any(user => user.Email == email))
                throw new ArgumentException("Email already exists. Please enter a different Email Address.");

            string PasswordSalt = CreateSalt();
            string emailkey = GenerateKey();
            User newUser = new User()
            {
                UserName = username,
                Name = name,

                //Password = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5"),
                PasswordSalt = PasswordSalt,
                Password = CreatePasswordHash(password, PasswordSalt),
                CreatedDate = DateTime.Now,
                IsActivated = false,
                IsLockedOut = false,
                LastLockedOutDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                NewEmailKey = emailkey,
                Question = question,
                Answer = answer,
                Email = email,
                FileName = "BlankProfile.gif",
                RoleID = role.ID
            };

            try
            {
                AddUser(newUser);
                SendEmail(username,email,emailkey);

            }
            catch (ArgumentException ae)
            {
                throw ae;
            }
            catch (Exception e)
            {
                throw new ArgumentException("The authentication provider returned an error. Please verify your entry and try again. " +
                    "If the problem persists, please contact your system administrator." + e);
            }

            // Immediately persist the user data
            Save();
        }

        public bool ChangePassword(string username, string oldPassword, string newPassword)
        {

            IVRControlPanelMembershipProvider memberservice = new IVRControlPanelMembershipProvider();
            if (!memberservice.ValidateUser(username, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
            {
                return false;
            }
            else
            {
               
                using (IVRControlPanelEntities db = new IVRControlPanelEntities())
                {
                 /*   user = GetUser(username);
                   // string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");
                    string PasswordSalt = CreateSalt();
                    string email = user.Email;
                    user.PasswordSalt = PasswordSalt;
                    user.Password = CreatePasswordHash(newPassword, PasswordSalt);

                    db.SaveChanges();
                  * */

                    User user = db.Users.FirstOrDefault(x => x.UserName == username);
                    string PasswordSalt = CreateSalt();
                    user.PasswordSalt = PasswordSalt;
                    user.Password = CreatePasswordHash(newPassword, PasswordSalt);
                    db.SaveChanges();
                   
                }
               return true;
            }

        }

        public bool ChangeResetPassword(string username,  string newPassword)
        {
               using (IVRControlPanelEntities db = new IVRControlPanelEntities())
                {

                    User user = db.Users.FirstOrDefault(x => x.UserName == username);
                    string PasswordSalt = CreateSalt();
                    user.PasswordSalt = PasswordSalt;
                    user.Password = CreatePasswordHash(newPassword, PasswordSalt);
                    db.SaveChanges();

                }
                return true;
            

        }
        public void DeleteUser(User user)
        {
            if (!UserExists(user))
                throw new ArgumentException(MissingUser);

            entities.Users.DeleteObject(user);
        }

        public void DeleteUser(string userName)
        {
            DeleteUser(GetUser(userName));
        }

        public void AddRole(Role role)
        {
            if (RoleExists(role))
                throw new ArgumentException(TooManyRole);

            entities.Roles.AddObject(role);
        }

        public void AddRole(string roleName)
        {
            Role role = new Role()
            {
                Name = roleName
            };

            AddRole(role);
        }

        public void DeleteRole(Role role)
        {
            if (!RoleExists(role))
                throw new ArgumentException(MissingRole);

            if (GetUsersForRole(role).Count() > 0)
                throw new ArgumentException(AssignedRole);

            entities.Roles.DeleteObject(role);
        }

        public void DeleteRole(string roleName)
        {
            DeleteRole(GetRole(roleName));
        }

        #endregion

        #region Persistence

        public void Save()
        {
            entities.SaveChanges();
        }

        #endregion

        #region Helper Methods

        public bool UserExists(User user)
        {
            if (user == null)
                return false;

            return (entities.Users.SingleOrDefault(u => u.ID == user.ID || u.UserName == user.UserName) != null);
        }

        public bool UserExists(String user, String email)
        {
            if (user == null || user == "")
                return false;

            return (entities.Users.SingleOrDefault(u => u.UserName == user && u.Email == email) != null);
        }

        public bool RoleExists(Role role)
        {
            if (role == null)
                return false;

            return (entities.Roles.SingleOrDefault(r => r.ID == role.ID || r.Name == role.Name) != null);
        }

        #endregion

        public string GetUserNameByEmail(string email)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.Email == email) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.UserName;
                }
                else
                {
                    return "";
                }
            }
        }
        /*
        public MembershipUser GetUser(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    string _username = dbuser.UserName;
                    int _providerUserKey = dbuser.ID;
                    string _email = dbuser.Email;
                    string _passwordQuestion = "";
                    string _comment = dbuser.Comments;
                    bool _isApproved = dbuser.IsActivated;
                    bool _isLockedOut = dbuser.IsLockedOut;
                    DateTime _creationDate = (DateTime)dbuser.CreatedDate;
                    DateTime _lastLoginDate = dbuser.LastLoginDate;
                    DateTime _lastActivityDate = DateTime.Now;
                    DateTime _lastPasswordChangedDate = DateTime.Now;
                    DateTime _lastLockedOutDate = dbuser.LastLockedOutDate;

                    MembershipUser user = new MembershipUser("CustomMembershipProvider",
                                                              _username,
                                                              _providerUserKey,
                                                              _email,
                                                              _passwordQuestion,
                                                              _comment,
                                                              _isApproved,
                                                              _isLockedOut,
                                                              _creationDate,
                                                              _lastLoginDate,
                                                              _lastActivityDate,
                                                              _lastPasswordChangedDate,
                                                              _lastLockedOutDate);

                    return user;
                }
                else
                {
                    return null;
                }
            }
        }
        
        */

        private static string CreateSalt()
        {
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[32];
            rng.GetBytes(buff);

            return Convert.ToBase64String(buff);
        }
        public static string CreatePasswordHash(string pwd, string salt)
        {
            string saltAndPwd = String.Concat(pwd, salt);
            string hashedPwd =
                    FormsAuthentication.HashPasswordForStoringInConfigFile(
                    saltAndPwd, "sha1");
            return hashedPwd;
        }

        private static string GenerateKey()
        {
            Guid emailKey = Guid.NewGuid();

            return emailKey.ToString();
        }

        private static void SendEmail(string UserName, string Email, string NewEmailKey)
        {
            string ActivationLink = "http://localhost/ivrcontrolpanel/Account/Activate/" +
                                      UserName + "/" + NewEmailKey;
            /*
            var message = new MailMessage("sunilshrestha59@gmail.com", Email)
            {
                Subject = "Activate your account",
                Body = ActivationLink
            };

            var client = new SmtpClient("SERVER");
            client.Credentials = new System.Net.NetworkCredential("USERNAME", "PASSWORD");
            client.UseDefaultCredentials = false;


            client.Send(message);
            */

         /*   MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            SmtpServer.Credentials = new NetworkCredential("sunilshrestha59@gmail.com", "g0ldki5t");
            SmtpServer.UseDefaultCredentials = false;

            mail.From = new MailAddress("sunilshrestha59@gmail.com");
            mail.To.Add(Email);
            mail.Subject = "Activation Account Link";
            mail.Body = "Activate the account by Clicking on Link: " + ActivationLink;

            SmtpServer.Send(mail);
          * */
            MailMessage mail = new MailMessage();

            NetworkCredential cred = new NetworkCredential("sunilshrestha59@gmail.com", "g0ldki5t");
            mail.To.Add(Email);
            mail.Subject = "Activation Account Link";
            mail.From = new MailAddress("sunilshrestha59@gmail.com");

            mail.IsBodyHtml = true;
            mail.Body = "Activate the account by Clicking on Link: " + ActivationLink;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = cred;
            smtp.Port = 587;
            smtp.Send(mail);
        }

        public void ResetLink(string username, string email)
        {
            string tempPassword = CreateRandomPassword(8);
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                User user = db.Users.FirstOrDefault(x => x.UserName == username && x.Email == email);
                user.NewPasswordKey = tempPassword;
                user.NewPasswordRequested = DateTime.Now;
                db.SaveChanges();

            }
            EmailResetPassword(username, email, tempPassword);

        }

        private static void EmailResetPassword(string UserName, string Email, string tempPassword)
        {
            string newPasswordLink = "http://localhost/ivrcontrolpanel/Account/ResetPassword/" +
                                      UserName + "/" + tempPassword;
          
            MailMessage mail = new MailMessage();

            NetworkCredential cred = new NetworkCredential("sunilshrestha59@gmail.com", "g0ldki5t");
            mail.To.Add(Email);
            mail.Subject = "Reset password for IVR Control Panel ";
            mail.From = new MailAddress("sunilshrestha59@gmail.com");

            mail.IsBodyHtml = true;
            mail.Body = "please click the link for reseting your password is: " + newPasswordLink;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com");
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            smtp.Credentials = cred;
            smtp.Port = 587;
            smtp.Send(mail);
        }

       public bool CheckTempPassword(string username, string tempPassword)
       {
           
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.NewPasswordKey == tempPassword && dbuser.NewPasswordKey != null)
                    {
                        dbuser.LastModifiedDate = DateTime.Now;
                        dbuser.NewPasswordKey = null;

                        db.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
       }


        public bool ActivateUser(string username, string key)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.NewEmailKey == key)
                    {
                        dbuser.IsActivated = true;
                        dbuser.LastModifiedDate = DateTime.Now;
                        dbuser.NewEmailKey = null;

                        db.SaveChanges();

                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    return false;
                }
            }
        }

        /*
        public void UpdateLastLoginDate(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    dbuser.LastLoginDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
        */
        public DateTime LastLoginDate(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;
                var dbuser = result.First();
                return (DateTime)dbuser.LastLoginDate;

            }
        }

        public int GetUserID(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;
                var dbuser = result.First();
                return dbuser.ID;

            }

        }
        //Update the particular date column using LINQ
        public void UpdateLast(string username, Action<User> action)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();
                    action(dbuser);
                    db.SaveChanges();
                }
            }
        }
        /*
        public void UpdateLastModifiedDate(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    dbuser.LastModifiedDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
         */
        public DateTime LastModifiedDate(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;
                var dbuser = result.First();
                return (DateTime)dbuser.LastModifiedDate;

            }
        }
        /*
        public void UpdateLastLogoutDate(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    dbuser.LastLockedOutDate = DateTime.Now;
                    db.SaveChanges();
                }
            }
        }
        */

        public string LastLoginIP()
        {
            return HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
        }

        public string ImageName(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;
                var dbuser = result.First();
                return dbuser.FileName;

            }
        }

        public string GetCategoryName(int id)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Categories where (u.ID == id) select u;
                var cat = result.First();
                return cat.CategoryName;
            }
        }

        public string GetLang(int id)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Languages where (u.ID == id) select u;
                var cat = result.First();
                return cat.Language1;
            }
        }

        public string GetParentCategory(int id)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Categories where (u.ID == id) select u;
                var cat = result.First();
                var parent = cat.ParentCategoryID;
                if (parent == null)
                {
                    return "root";
                }
                else
                {
                    var resultparent = from u in db.Categories where (u.ID == parent) select u;
                    var catparent = resultparent.First();
                    return catparent.CategoryName;
                }
            }
        }

        public bool CategoryExist(string name,int? parentid)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                //var results = (parentid == null) ? from u in db.Categories.Where(x => object.Equals(x.ParentCategoryID, parentid) && y => object.Equals(yx.CategoryName, name) ) select u : from u in db.Categories where (u.ParentCategoryID == parentid) select u;
                //var cats = results.Any();

               // var result = from u in db.Categories where (u.CategoryName == name) select u;
               // var cat = result.Any();

              // var result =  from item in db.Categories where item.CategoryName == name && object.Equals(item.ParentCategoryID, parentid) select item;
                var result = from item in db.Categories where item.CategoryName == name && (parentid.HasValue ? item.ParentCategoryID == parentid : item.ParentCategoryID.Equals(null)) select item;

               var cat = result.Any();

                return cat;
            }
        }

        public string GetSecurityQuestion(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.Question;
                }

                else
                {
                    return "";
                }
            }
        }

        public string GetSecurityAnswer(string username)
        {
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.FirstOrDefault();

                    return dbuser.Answer;
                }
                else
                {
                    return "";
                }
            }

        }

        public bool CheckAnswer(string username, string question, string answer)
        {
            if (username == null || username == "" || question == null || question == "" || answer == null || answer == "")
                return false;

            return (entities.Users.SingleOrDefault(u => u.UserName == username && u.Answer == answer && u.Question == question) != null);
        }

        private static string CreateRandomPassword(int passwordLength)
        {
            string allowedChars = "abcdefghijkmnopqrstuvwxyzABCDEFGHJKLMNOPQRSTUVWXYZ0123456789!@$?_-";
            char[] chars = new char[passwordLength];
            Random rd = new Random();

            for (int i = 0; i < passwordLength; i++)
            {
                chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
            }

            return new string(chars);
        }
        
        public Dictionary<String,int> GraphData(String start, String end)
        {
            DateTime startDate = DateTime.Parse(start, CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.Parse(end, CultureInfo.InvariantCulture);
          //  DateTime startDate = Convert.ToDateTime(start);
          //  DateTime endDate = Convert.ToDateTime(end);
            NEPALIVREntities db = new NEPALIVREntities();
            Dictionary<String, int> dictionary = new Dictionary<String, int>();
            System.TimeSpan duration = new System.TimeSpan(1, 0, 0, 0);
               for (DateTime dates = startDate; dates <= endDate;)
                 {
                     DateTime BeginingOfDay = new DateTime(dates.Year, dates.Month, dates.Day, 0, 0, 0);
                     DateTime EndOfDay = BeginingOfDay.AddDays(1);
                     int count = (from u in db.CDRs where (u.StartTime >= BeginingOfDay && u.StartTime <= EndOfDay) select u).Count();                     
                     dictionary.Add(dates.ToString("MM/dd/yyyy"), count);
                     dates = dates.AddDays(1);
                     
                 }

           
                 return dictionary;
                 

        }

        public string[,] ExcelData(String start, String end)
        {
            DateTime startTime = Convert.ToDateTime(start);
            DateTime endTime = Convert.ToDateTime(end);
              NEPALIVREntities db = new NEPALIVREntities();
                var result = db.CDRs.Where (m => m.StartTime >= startTime && m.StartTime<= endTime).OrderByDescending(m => m.StartTime)
                    .Select(m => new {    startTime = m.StartTime,
                     endTime = m.EndTime,
                     ano = m.Ano,
                     bno = m.Bno,
                     duration = SqlFunctions.DateDiff("s", m.StartTime, m.EndTime)
                });

            String[,] names = new String[result.Count(),6];

        
            int i = 1;
            names[0, 0] = "Caller";
            names[0, 1] = "Receiver";
            names[0, 2] = "Start Time";
            names[0, 3] = "End Time";
            names[0, 4] = "Duration";

            foreach(var item in result)
            {
                if (i == result.Count())
                {
                    break;
                }
             
                if (item.ano == null)
                {
                    names[i, 0] = "null";
                }
                else
                {
                    names[i, 0] = item.ano.ToString();
                }
                if (item.bno == null)
                {
                    names[i, 1] = "null";
                }
                else
                {
                    names[i, 1] = item.bno.ToString();
                }
                names[i,2] = item.startTime.ToString();
                names[i,3] = item.endTime.ToString();
                names[i,4] = ((Double)item.duration/60.0).ToString();

                i++;

             
                
            }
                return names;

        }




    }
}