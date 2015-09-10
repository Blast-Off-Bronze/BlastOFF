namespace BlastOFF.Models
{
    using System.ComponentModel.DataAnnotations;

    using BlastOFF.Models.Enumerations;
    using BlastOFF.Models.GalleryModels;
    using BlastOFF.Models.MusicModels;
    using BlastOFF.Models.UserModel;

    public class Notification
    {
        public Notification()
        {
            this.IsSeen = false;
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public ApplicationUser Recipient { get; set; }

        public NotificationType NotificationType { get; set; }

        public bool IsSeen { get; set; }

        public int? ImageAlbumId { get; set; }

        public virtual ImageAlbum ImageAlbum { get; set; }

        public int? MusicAlbumId { get; set; }

        public virtual MusicAlbum MusicAlbum { get; set; }

        public string SenderId { get; set; }

        public ApplicationUser Sender { get; set; }
    }
}