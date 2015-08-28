using System;
using System.Linq;
using System.Web.Http;
using BlastOFF.Data;
using BlastOFF.Data.Interfaces;
using BlastOFF.Models.ChatModels;
using BlastOFF.Services.Models.ChatModels;
using Microsoft.AspNet.Identity;

namespace BlastOFF.Services.Controllers
{
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
        public IHttpActionResult GetMessage(string chatRoom)
        {
            var validateChatRoom = this.Data.Chats.All().FirstOrDefault(c => c.ChatRoomName == chatRoom);

            if (validateChatRoom == null)
            {
                return this.NotFound();
            }

            var message = this.Data.Chats.All()
                .Where(c => c.ChatRoomName == chatRoom)
                .Select(ChatViewModel.DisplayResult);

            return this.Ok(message);
        }

        // POST api/message
        [HttpPost]
        [Route("api/message")]
        public IHttpActionResult PostMessage(ChatBindingModel model)
        {
            var senderUsername = User.Identity.GetUserName();

            if (senderUsername == model.ReceiverUsername)
            {
                return this.BadRequest("Enter a valid, diffrent from yours, receiver-username.");
            }

            var user = this.Data.Users.All()
                .Where(u => u.UserName == model.ReceiverUsername)
                .Select(u => u.UserName)
                .FirstOrDefault();

            if (user == null && model.ReceiverUsername.ToLower() != "all")
            {
                return this.BadRequest("Enter a valid receiver-username or send the message to 'all'.");
            }

            var message = new Chat()
            {
                ChatRoomName = model.ChatRoomName,
                SenderUsername = senderUsername,
                ReceiverUsername = model.ReceiverUsername,
                Content = model.Content,
                PostedOn = DateTime.Now.ToString("HH:mm (d MMM yyyy)")
            };

            this.Data.Chats.Add(message);

            this.Data.SaveChanges();

            return this.Ok(message);
        }

        // PUT api/message
        [HttpPut]
        [Route("api/message")]
        public IHttpActionResult Edit(int id, ChatBindingModel model)
        {
            var username = User.Identity.GetUserName();

            var message = this.Data.Chats.All().FirstOrDefault(c => c.Id == id);

            if (message == null)
            {
                return this.NotFound();
            }

            if (username != message.SenderUsername)
            {
                return this.BadRequest("Enter a message id that you posted.");
            }

            message.Content = model.Content;

            this.Data.SaveChanges();

            return this.Ok(message);
        }

        // DELETE api/message
        [HttpDelete]
        [Route("api/message")]
        public IHttpActionResult Delete(int id)
        {
            var username = User.Identity.GetUserName();

            var message = this.Data.Chats.All().FirstOrDefault(c => c.Id == id);

            if (message == null)
            {
                return this.NotFound();
            }

            if (username != message.SenderUsername)
            {
                return this.BadRequest("Enter a message id that you posted.");
            }

            this.Data.Chats.Delete(id);

            this.Data.SaveChanges();

            return this.Ok("Message delete it.");
        }
    }
}