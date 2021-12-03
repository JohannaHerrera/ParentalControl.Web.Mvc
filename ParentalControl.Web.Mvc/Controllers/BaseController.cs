using ParentalControl.Web.Mvc.Business.AppConstants;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ParentalControl.Web.Mvc.Controllers
{
    public class BaseController : Controller
    {
        public UserSessionInfo GetCurrentUserInfo()
        {
            var currentUserSession = Session[AppConstants.CurrentUser] as UserSessionInfo;

            if (currentUserSession == null)
            {
                throw new Exception("La sessión ha finalizado");
            }

            return currentUserSession;
        }

        /// <summary>
        /// SetCurrentUserInfo
        /// </summary>
        /// <param name="studentInfo"></param>
        public void SetCurrentUserInfo(Parent parentInfo)
        {
            UserSessionInfo userSessionInfo = new UserSessionInfo();
            userSessionInfo.Id = parentInfo.ParentId;
            userSessionInfo.UserName = parentInfo.ParentUsername;
            userSessionInfo.Email = parentInfo.ParentEmail;
            Session[AppConstants.CurrentUser] = userSessionInfo;
        }
    }
}