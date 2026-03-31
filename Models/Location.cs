using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
namespace APS_LostProperty.Models

{
    // This model represents a location where a lost item was found
    // Examples: Library, Gym, Cafeteria
    public class Location
    {
        // Primary Key for the Location table
        // This uniquely identifies each location in the database
        public int LocationID { get; set; }

        // The name of the location where the item was found
        // This field is required when creating or editing a location
        [Required]

        // Validation rule ensuring the location name has at least 2 characters
        [MinLength(2, ErrorMessage = " Location name must be At least 2 Characters.")]

        // Regular expression validation
        // Allows only letters, numbers, spaces, apostrophes, and dashes
        // Prevents special characters such as @, #, or %
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string LocationName { get; set; }


        // Navigation property
        // This represents the relationship between Location and LostItem
        // One location can have many lost items found there
        public ICollection<LostItem>? LostItems { get; set; }
    }
}