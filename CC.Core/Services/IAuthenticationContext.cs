﻿using System;
using System.Web;
using System.Web.Security;
using CC.Security;

namespace CC.Core.Services
{
    public interface IAuthenticationContext
    {
        string ThisUserHasBeenAuthenticated(IUser username, bool rememberMe);
        void SignOut();
    }

    public class WebAuthenticationContext : IAuthenticationContext
    {
        public string ThisUserHasBeenAuthenticated(IUser user,  bool rememberMe)
        {
            string userData = String.Empty;
            userData = userData + "UserId=" + user.EntityId + "|CompanyId=" + user.CompanyId;
            var ticket = new FormsAuthenticationTicket(1, user.FullNameLNF, DateTime.Now, DateTime.Now.AddMinutes(30),
                                                       rememberMe, userData);
            string encTicket = FormsAuthentication.Encrypt(ticket);
            var faCookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
            HttpContext.Current.Response.Cookies.Add(faCookie);
            return FormsAuthentication.GetRedirectUrl(user.FullNameLNF, false);
        }

        public void SignOut()
        {
            SignOutFunc();
        }

        public Action<string, bool> SetAuthCookieFunc = FormsAuthentication.SetAuthCookie;
        public Action SignOutFunc = FormsAuthentication.SignOut;
    }
}