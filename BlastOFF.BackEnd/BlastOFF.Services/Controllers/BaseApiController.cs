namespace BlastOFF.Services.Controllers
{
    using System.Web.Http;

    using Data.Interfaces;

    using Microsoft.AspNet.Identity;

    public abstract class BaseApiController : ApiController
    {
        private IBlastOFFData data;

        protected BaseApiController(IBlastOFFData data)
        {
            this.data = data;
        }

        protected IBlastOFFData Data
        {
            get
            {
                return this.data;
            }
        }

        protected IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return this.InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error);
                    }
                }

                if (this.ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return this.BadRequest();
                }

                return this.BadRequest(this.ModelState);
            }

            return null;
        }
    }
}