
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
    public class Location
    {
        public int LocationID { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 30 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string LocationName { get; set; }

        // Navigation property
        public ICollection<LostItem>? LostItems { get; set; }
    }
}

