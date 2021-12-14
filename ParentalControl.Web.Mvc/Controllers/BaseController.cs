using ParentalControl.Web.Mvc.Business.AppConstants;
using ParentalControl.Web.Mvc.Data;
using ParentalControl.Web.Mvc.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using static ParentalControl.Web.Mvc.Models.Enum;

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

        public void Alert(string message, NotificationType notificationType)
        {
            var msg = "<script language='javascript'>Swal.fire('" + notificationType.ToString().ToUpper() + "', '" + message + "','" + notificationType + "')" + "</script>";
            //var msg = "<script language='javascript'>Swal.fire({title:'',text: '" + message + "',type:'" + notificationType + "',allowOutsideClick: false,allowEscapeKey: false,allowEnterKey: false})" + "</script>";
            TempData["notification"] = msg;
        }

        /// </summary>
        /// <param name="message">The message to display to the user.</param>
        /// <param name="notifyType">The type of notification to display to the user: Success, Error or Warning.</param>
        public void Message(string message, NotificationType notifyType)
        {
            TempData["Notification2"] = message;

            switch (notifyType)
            {
                case NotificationType.success:
                    TempData["NotificationCSS"] = "alert-box success";
                    break;
                case NotificationType.error:
                    TempData["NotificationCSS"] = "alert-box errors";
                    break;
                case NotificationType.warning:
                    TempData["NotificationCSS"] = "alert-box warning";
                    break;

                case NotificationType.info:
                    TempData["NotificationCSS"] = "alert-box notice";
                    break;
            }
        }
    }
}