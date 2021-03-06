﻿namespace BlastOFF.Services.Controllers
{
    using System;
    using System.Linq;
    using System.Web.Http;
    using BlastOFF.Data;
    using BlastOFF.Data.Interfaces;
    using BlastOFF.Models.ChatModels;
    using BlastOFF.Services.Models.ChatModels;
    using Microsoft.AspNet.Identity;

    using BlastOFF.Services.Constants;

    [Authorize]
    public class ChatController : BaseApiController
    {
        public ChatController()
            : this(new BlastOFFData())
        {
        }

        public ChatController(IBlastOFFData data)
            : base(data)
        {
        }

        // GET api/message
        [HttpGet]
        [Route("api/message")]
        public IHttpActionResult GetMessages([FromUri] string partnerId, [FromUri] int CurrentPage = MainConstants.DefaultPage, 
            [FromUri] int PageSize = MainConstants.PageSize)
        {
            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (partnerId == user.Id)
            {
                return this.BadRequest("You cannot chat with youself.");
            }

            var messages = this.Data.Messages.All().Where(m => (m.SenderId == user.Id && m.ReceiverId == partnerId)
            || (m.SenderId == partnerId && m.ReceiverId == user.Id) && m.Deleted == false)
                .OrderBy(m => m.SentDateTime)
                .Skip(CurrentPage * PageSize)
                .Take(PageSize)
                .ToList()
                .Select(m => MessageViewModel.Create(m));

            return this.Ok(messages);
        }

        // POST api/message
        [HttpPost]
        [Route("api/message")]
        public IHttpActionResult PostMessage([FromBody] MessageCreateBindingModel model)
        {
            if (model == null)
            {
                return this.BadRequest();
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var currentUser = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (currentUser.Id == model.ReceiverId)
            {
                return this.BadRequest("Enter a valid, different from yours, receiver-username.");
            }

            var receiverUser = this.Data.Users.Find(model.ReceiverId);

            if (receiverUser == null)
            {
                return this.BadRequest("Invalid recipient Id.");
            }

            var message = new Message()
            {
                SenderId = currentUser.Id,
                ReceiverId = receiverUser.Id,
                Content = model.Content,
                SentDateTime = DateTime.Now
            };

            this.Data.Messages.Add(message);

            this.Data.SaveChanges();

            var resultItem = MessageViewModel.Create(message);

            return this.Ok(resultItem);
        }

        // DELETE api/message
        [HttpDelete]
        [Route("api/message/{id}")]
        public IHttpActionResult Delete([FromBody] int id)
        {
            var message = this.Data.Messages.Find(id);

            if (message == null)
            {
                return this.NotFound();
            }

            var user = this.Data.Users.Find(this.User.Identity.GetUserId());

            if (message.SenderId != user.Id)
            {
                return this.BadRequest("Enter a message id that you posted.");
            }

            message.Deleted = true;

            this.Data.Messages.Update(message);

            this.Data.SaveChanges();

            return this.Ok("The message is deleted.");
        }
    }
}