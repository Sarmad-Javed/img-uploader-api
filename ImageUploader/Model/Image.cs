using System.ComponentModel.DataAnnotations;

namespace ImageUploader.Model
{
    public class Image
    {
        public int id { get; set; }

        [Required]
        public string imageName { get; set; }
        [Required]
        public string imageUrl { get; set; }
        public string imageType { get; set; }
        public DateTime createdAt { get; set; }

    }
}
