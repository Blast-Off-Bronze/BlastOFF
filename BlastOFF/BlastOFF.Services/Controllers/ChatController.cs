using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Security;
using System.Web.UI.WebControls;
using BlastOFF.Data;
using BlastOFF.Data.Interfaces;
using BlastOFF.Models.ChatModels;
using BlastOFF.Models.GalleryModels;
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
                return this.BadRequest("There is no such a room.");
            }

            var message = this.Data.Chats.All()
                .Where(c => c.ChatRoomName == chatRoom)
                .Select(a => new
                {
                    Sender = a.SenderUsername,
                    Receiver = a.ReceiverUsername,
                    a.Content,
                    Time = a.PostedOn
                });

            return this.Ok(message);
        }

        // POST api/message
        [HttpPost]
        [Route("api/message")]
        public IHttpActionResult PostMessage([FromBody]ChatBindingModel model)
        {
            var senderUsername = User.Identity.GetUserName();

            if (senderUsername == model.RecieverUsername)
            {
                return this.BadRequest("You cannot send a message to yourself.");
            }

            var user = this.Data.Users.All()
                .Where(u => u.UserName == model.RecieverUsername)
                .Select(u => u.UserName)
                .FirstOrDefault();


            if (user == null && model.RecieverUsername != "All")
            {
                return this.BadRequest("There is no such user.");
            }

            var message = new Chat()
            {
                Content = model.Content,
                ChatRoomName = model.ChatRoomName,
                SenderUsername = senderUsername,
                ReceiverUsername = model.RecieverUsername,
                PostedOn = DateTime.Now.ToString("HH:mm (d MMM yyyy)")
            };

            this.Data.Chats.Add(message);

            this.Data.SaveChanges();

            return this.Ok(message);
        }
    }
}