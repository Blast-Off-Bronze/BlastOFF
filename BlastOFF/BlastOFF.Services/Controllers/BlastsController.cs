namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Web.Http;
    using Microsoft.AspNet.Identity;

    using Data;
    using Models.BlastModels;
    using BlastOFF.Models.BlastModels;
    using System.Linq;

    public class BlastsController : ApiController
    {
        private readonly BlastOFFData data = new BlastOFFData();


        [HttpGet]
        [Route("api/blasts")]
        public IHttpActionResult GetAll()
        {
            var blasts = this.data.Blasts.All().Select(BlastViewModel.Create);

            return this.Ok(blasts);
        }


        [HttpPost]
        [Authorize]
        [Route("api/blasts")]
        public IHttpActionResult CreateNewBlast([FromBody]BlastBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            var newBlast = new Blast
            {
                AuthorId = loggedUserId,
                Content = model.Content,
                PostedOn = DateTime.Now,
                BlastType = model.BlastType
            };

            this.data.Blasts.Add(newBlast);
            this.data.SaveChanges();

            return Ok();
        }

        [HttpPut]
        [Authorize]
        [Route("api/blasts/{id}")]
        public IHttpActionResult UpdateBlast(int id, [FromBody]BlastBindingModel model)
        {
            var loggedUserId = this.User.Identity.GetUserId();
            var oldBlast = this.data.Blasts.Find(id);

            if (loggedUserId == null)
            {
                return this.Unauthorized();
            }

            if (oldBlast == null)
            {
                return this.NotFound();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest("Wrong or missing input parameters");
            }

            if (oldBlast.AuthorId != loggedUserId)
            {
                return this.Unauthorized();
            }

            oldBlast.Content = model.Content;
            oldBlast.BlastType = model.BlastType;
            this.data.SaveChanges();

            return Ok();
        }
    }
}
