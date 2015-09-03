namespace BlastOFF.Services.Models.ChatModels
{
    using System;
    using BlastOFF.Models.ChatModels;

    public class MessageViewModel
    {
        public static MessageViewModel Create(Message m)
        {
            return new MessageViewModel
            {
                MessageId = m.Id,
                From = m.SenderId,
                To = m.ReceiverId,
                Message = m.Content,
                Time = m.SentDateTime
            };
        }

        public int MessageId { get; set; }

        public string From { get; set; }

        public string To { get; set; }

        public string Message { get; set; }

        public DateTime Time { get; set; }
    }
}