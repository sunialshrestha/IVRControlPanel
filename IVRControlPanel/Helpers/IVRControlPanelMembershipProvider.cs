using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using IVRControlPanel.Models;
using System.Collections.Specialized;

namespace IVRControlPanel.Helpers
{
    public class IVRControlPanelMembershipProvider : MembershipProvider
    {
        /*
        int PageIndex = 15;
        int PageSize = 5;
        int totalRecords = 50;
         */
        //
        // Properties from web.config, default all to False
        //
        private string _ApplicationName;
        private bool _EnablePasswordReset;
        private bool _EnablePasswordRetrieval = false;
        private bool _RequiresQuestionAndAnswer = false;
        private bool _RequiresUniqueEmail = true;
        private int _MaxInvalidPasswordAttempts;
        private int _PasswordAttemptWindow;
        private int _MinRequiredPasswordLength;
        private int _MinRequiredNonalphanumericCharacters;
        private string _PasswordStrengthRegularExpression;
        private MembershipPasswordFormat _PasswordFormat = MembershipPasswordFormat.Hashed;
        
        private IVRControlPanelRepository repository;

        //
        // A helper function to retrieve config values from the configuration file.
        //  

        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }

        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "CustomMembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }

            base.Initialize(name, config);

            _ApplicationName = GetConfigValue(config["applicationName"],
                          System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _MaxInvalidPasswordAttempts = Convert.ToInt32(
                          GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _PasswordAttemptWindow = Convert.ToInt32(
                          GetConfigValue(config["passwordAttemptWindow"], "10"));
            _MinRequiredNonalphanumericCharacters = Convert.ToInt32(
                          GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _MinRequiredPasswordLength = Convert.ToInt32(
                          GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _EnablePasswordReset = Convert.ToBoolean(
                          GetConfigValue(config["enablePasswordReset"], "true"));
            _PasswordStrengthRegularExpression = Convert.ToString(
                           GetConfigValue(config["passwordStrengthRegularExpression"], ""));

        }

        public int MinPasswordLength
        {
            get
            {
                return 5;
            }
        }

		public override int MinRequiredPasswordLength
		{
			get
			{
				return MinPasswordLength;
			}
		}

        public IVRControlPanelMembershipProvider()
        {
            this.repository = new IVRControlPanelRepository();
        }

        public override bool ValidateUser(string username, string password)
        {
            if (string.IsNullOrEmpty(password.Trim()) || string.IsNullOrEmpty(username.Trim()))
                return false;
            using (IVRControlPanelEntities db = new IVRControlPanelEntities())
            {
                var result = from u in db.Users where (u.UserName == username) select u;

                if (result.Count() != 0)
                {
                    var dbuser = result.First();

                    if (dbuser.Password == IVRControlPanelRepository.CreatePasswordHash(password, dbuser.PasswordSalt) && dbuser.IsActivated == true)
                        return true;
                    else
                        return false;
                }
                else
                {
                    return false;
                }
            }
            //string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(password.Trim(), "md5");

            //return this.repository.GetAllUsers().Any(user => (user.UserName == username.Trim()) && (user.Password == hash));
        }

        public void CreateUser(string username, string fullName, string password, string email, string roleName, string question, string answer)
        {
            this.repository.CreateUser(username, fullName, password, email, roleName, question, answer);
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
          /*  if (!ValidateUser(username, oldPassword) || string.IsNullOrEmpty(newPassword.Trim()))
                return false;

            User user = repository.GetUser(username);
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(newPassword.Trim(), "md5");

            user.Password = hash;
            repository.Save();

            return true;
           * */
            return repository.ChangePassword(username, oldPassword, newPassword);
        }

        #region Not Implemented MembershipProvider Methods

        #region Properties

        /// <summary>
        /// This property is not implemented.
        /// </summary>
        public override string ApplicationName
        {
            get { return _ApplicationName; }
            set { _ApplicationName = value; }
        }

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MaxInvalidPasswordAttempts
		{
			get { return _MaxInvalidPasswordAttempts; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int MinRequiredNonAlphanumericCharacters
		{
			get { return _MinRequiredNonalphanumericCharacters; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override int PasswordAttemptWindow
		{
			get { return _PasswordAttemptWindow; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override MembershipPasswordFormat PasswordFormat
		{
			get { return _PasswordFormat; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override string PasswordStrengthRegularExpression
		{
			get { return _PasswordStrengthRegularExpression; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresQuestionAndAnswer
		{
			get { return _RequiresQuestionAndAnswer; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool RequiresUniqueEmail
		{
			get { return _RequiresUniqueEmail; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordReset
		{
			get { return _EnablePasswordReset; }
		}

		/// <summary>
		/// This property is not implemented.
		/// </summary>
		public override bool EnablePasswordRetrieval
		{
            get { return _EnablePasswordRetrieval; }
		}

        #endregion

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            return false;
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string GetUserNameByEmail(string email)
        {
            IVRControlPanelRepository _user = new IVRControlPanelRepository();

            return _user.GetUserNameByEmail(email);
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This function is not implemented.
        /// </summary>
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// This method is not implemented.
        /// </summary>
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        #endregion
        /*
        public void GetAllUsers()
        {
            
            MembershipUserCollection allUsers = Membership.GetAllUsers(this.PageIndex, this.PageSize, out totalRecords);
            MembershipUserCollection filteredUsers = new MembershipUserCollection();

            bool isOnline = true;
            foreach (MembershipUser user in allUsers)
            {
                // if user is currently online, add to gridview list
                if (user.IsOnline == isOnline)
                {
                    filteredUsers.Add(user);
                }
            }
        }
         */ 


    }
}