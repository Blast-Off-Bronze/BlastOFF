using System;
using System.Linq.Expressions;
using BlastOFF.Models.ChatModels;

namespace BlastOFF.Services.Models.ChatModels
{
    public class ChatViewModel
    {
        public static Expression<Func<Chat, ChatViewModel>> DisplayResult
        {
            get
            {
                return c => new ChatViewModel
                {
                    MessageId = c.Id,
                    From = c.SenderUsername,
                    To = c.ReceiverUsername,
                    Message = c.Content,
                    Time = c.PostedOn
                };
            }

        }

        public int MessageId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Message { get; set; }

        public string Time { get; set; }
    }
}