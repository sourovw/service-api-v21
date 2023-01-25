using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class PieModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter 'Name'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 150 characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Please enter 'Description'.")]
        [MaxLength(500, ErrorMessage = "Maximum length of Description is 500 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter 'Price'.")]
        public double Price { get; set; }

        [DisplayName("Image Url")]
        [Required(ErrorMessage = "Please enter 'Image Url'.")]
        [MaxLength(250, ErrorMessage = "Maximum length of Image Url is 250 characters.")]
        public string ImageUrl { get; set; }

        [DisplayName("In Stock")]
        public bool InStock { get; set; }
    }
}