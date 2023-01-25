using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace EPS_Service_API.Model
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter 'Name'.")]
        [MaxLength(150, ErrorMessage = "Maximum length of Name is 150 characters.")]
        public string Name { get; set; }

        public List<PieModel> Pies { get; } = new List<PieModel>();
    }
}