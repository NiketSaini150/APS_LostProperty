
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
        public class Category
        {
            public int CategoryID { get; set; }

            [Required]
            
        [Display(Name = "Category Name")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string Name { get; set; }

            // Navigation property
            public ICollection<LostItem>? LostItems { get; set; }
        }
    
}
