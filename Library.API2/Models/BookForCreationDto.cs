using System.ComponentModel.DataAnnotations;

namespace Library.API2.Models
{
    public class BookForCreationDto
    {
        [Required(ErrorMessage = "You should fill out a title")]
        [MaxLength(100, ErrorMessage = "The title shouldn't have more than 100 characters")]
        public string Title { get; set; }
        
        [MaxLength(500, ErrorMessage = "The description shoudn't have more than 500 characters")]
        public string Description { get; set; }
    }
}