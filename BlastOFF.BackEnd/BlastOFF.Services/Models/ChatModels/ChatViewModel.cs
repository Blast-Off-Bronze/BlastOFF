using System;
using System.Linq.Expressions;
using BlastOFF.Models.ChatModels;

namespace BlastOFF.Services.Models.ChatModels
{
    public class ChatViewModel
    {
        public static Expression<Func<Message, ChatViewModel>> DisplayResult
        {
            get
            {
                return c => new ChatViewModel
                {
                    MessageId = c.Id,
                    From = c.SenderId,
                    To = c.ReceiverId,
                    Message = c.Content,
                    Time = c.PostedOn
                };
            }

        }

        public int MessageId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }
    }
}