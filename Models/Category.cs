using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
    // This model represents a category that a lost item can belong to
    // Example categories: Electronics, Clothing, Bags
    public class Category
    {
        // Primary key for the Category table
        // This uniquely identifies each category in the database
        public int CategoryID { get; set; }

        // This field is required, meaning a category cannot be created without a name
        [Required]

        // Changes the display label in forms to "Category Name"
        [Display(Name = "Category Name")]

        // Validation rule ensuring the category name has at least 2 characters
        [MinLength(2, ErrorMessage = " Category Name must be at least 2 characters.")]

        // Regular expression validation
        // Allows only letters and numbers with single spaces between words
        // Prevents invalid characters such as symbols
        [RegularExpression("^[A-Za-z0-9]+( [A-Za-z0-9]+)*$", ErrorMessage = "Only letters, numbers, and single spaces between words are allowed.")]
        public string Name { get; set; }

        // Navigation property
        // This creates a relationship where one Category can have many LostItems
        // Entity Framework uses this to link the Category table with the LostItem table
        public ICollection<LostItem>? LostItems { get; set; }
    }

}