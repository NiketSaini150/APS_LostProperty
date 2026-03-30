
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
        public class Category
        {
            public int CategoryID { get; set; }

            [Required]
            
        [Display(Name = "Category Name")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 30 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string Name { get; set; }

            // Navigation property
            public ICollection<LostItem>? LostItems { get; set; }
        }
    
}
