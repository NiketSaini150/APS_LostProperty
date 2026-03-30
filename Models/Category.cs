
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
        public class Category
        {
            public int CategoryID { get; set; }

            [Required]
            
        [Display(Name = "Category Name")]
        [MinLength(2, ErrorMessage = " Category Name must be at least 2 characters.")]
        [RegularExpression("^[A-Za-z0-9]+( [A-Za-z0-9]+)*$", ErrorMessage = "Only letters, numbers, and single spaces between words are allowed.")]
        public string Name { get; set; }

            // Navigation property
            public ICollection<LostItem>? LostItems { get; set; }
        }
    
}
