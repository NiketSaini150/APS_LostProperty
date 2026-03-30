
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Security.Claims;
namespace APS_LostProperty.Models
{

    public class LostItem
    {
        public int LostItemID { get; set; }

        [Required]
        [MinLength(2, ErrorMessage = " Item Name must be at least 2 characters.")]
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string ItemName { get; set; }

        [StringLength(200)]
        [MinLength(2, ErrorMessage = " Description  must be at least 2 characters.")]
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string? Description { get; set; }

        [Required(ErrorMessage = "Date found is required.")]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(LostItem), nameof(ValidateDateFound))]
        public DateTime DateFound { get; set; }

        public int CategoryID { get; set; }
        [ForeignKey("CategoryID")]
        public Category? Category { get; set; }

        public int LocationID { get; set; }
        [ForeignKey("LocationID")]
        public Location? Location { get; set; }

        public bool IsClaimed { get; set; } = false;

        public ICollection<Claim>? Claims { get; set; }
    
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
