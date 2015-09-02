﻿using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using BlastOFF.Data;
using BlastOFF.Data.Interfaces;

namespace BlastOFF.Services.UserSessionUtils
{
    public class SessionAuthorizeAttribute : AuthorizeAttribute
    {
        public SessionAuthorizeAttribute()
            : this(new BlastOFFData())
        {
        }

        public SessionAuthorizeAttribute(IBlastOFFData data)
        {
            this.Data = data;
        }

        protected IBlastOFFData Data { get; private set; }

        public override void OnAuthorization(HttpActionContext actionContext)
        {
            if (SkipAuthorization(actionContext))
            {
                return;
            }

            var requestProperties = actionContext.Request.GetOwinContext();
            var userSessionManager = new UserSessionManager(requestProperties);
            if (userSessionManager.ReValidateSession())
            {
                base.OnAuthorization(actionContext);
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(
                    HttpStatusCode.Unauthorized, "Session token expired or not valid.");
            }
        }

        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().Any();
        }
    }
}