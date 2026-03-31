using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Claims;
namespace APS_LostProperty.Models
{

    // This model represents an item that has been found and recorded in the lost property system
    // Staff or administrators add lost items when they are discovered
    public class LostItem
    {
        // Primary Key for the LostItem table
        // Uniquely identifies each lost item in the database
        public int LostItemID { get; set; }

        // Name of the item that was found
        // This field is required when creating or editing a lost item
        [Required]

        // Validation rule ensuring the item name has at least 2 characters
        [MinLength(2, ErrorMessage = " Item Name must be at least 2 characters.")]

        // Regular expression validation
        // Allows only letters, numbers, spaces, apostrophes, and dashes
        // Prevents invalid characters such as @ or #
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string ItemName { get; set; }

        // Optional description of the lost item
        // Maximum length allowed is 200 characters
        [StringLength(200)]

        // Ensures the description has at least 2 characters if provided
        [MinLength(2, ErrorMessage = " Description  must be at least 2 characters.")]

        // Regular expression validation to ensure only allowed characters are entered
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string? Description { get; set; }

        // Date when the item was found
        // This field is required
        [Required(ErrorMessage = "Date found is required.")]

        // Ensures the field stores only the date (not the time)
        [DataType(DataType.Date)]

        // Custom validation method used to check that the date is not in the future
        [CustomValidation(typeof(LostItem), nameof(ValidateDateFound))]
        public DateTime DateFound { get; set; }

        // Foreign Key linking the lost item to a category
        // Example categories: Electronics, Clothing, Bags
        public int CategoryID { get; set; }

        // Navigation property linking the lost item to the Category table
        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        // Foreign Key linking the lost item to the location where it was found
        // Example locations: Library, Gym, Classroom
        public int LocationID { get; set; }

        // Navigation property linking the lost item to the Location table
        [ForeignKey("LocationID")]
        public Location? Location { get; set; }

        // Boolean value that indicates whether the lost item has been claimed
        // Default value is false when the item is first recorded
        public bool IsClaimed { get; set; } = false;

        // Navigation property representing the relationship between LostItem and Claim
        // One lost item can have multiple claims from different users
        public ICollection<Claim>? Claims { get; set; }

        // Custom validation method used for DateFound
        // Ensures that the date entered is not in the future
        public static ValidationResult ValidateDateFound(DateTime date, ValidationContext context)
        {
            if (date.Date <= DateTime.Today)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult("Date found cannot be in the future.");
        }
    }
}