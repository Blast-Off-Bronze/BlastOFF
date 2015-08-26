
namespace BlastOFF.Services.Controllers
{
    using System.Web.Http;
    using BlastOFF.Data.Interfaces;

    public abstract class BaseApiController : ApiController
    {
        private IBlastOFFData data;

        protected BaseApiController(IBlastOFFData data)
        {
            this.data = data;
        }

        protected IBlastOFFData Data
        {
            get { return this.data; }
        }
    }
}