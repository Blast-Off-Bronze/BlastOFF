namespace BlastOFF.Models.GalleryModels
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Album
    {
        private ICollection<Image> images;

        public Album()
        {
            this.images = new HashSet<Image>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public int CreatedById { get; set; }

        public virtual ICollection<Image> Images { get; set; }
    }
}