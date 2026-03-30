
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace APS_LostProperty.Models

{
    public class Location
    {
        public int LocationID { get; set; }

        [Required]
        [ MinLength (2,ErrorMessage = " Location name must be At least 2 Characters.")]
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string LocationName { get; set; }

        // Navigation property
        public ICollection<LostItem>? LostItems { get; set; }
    }
}

