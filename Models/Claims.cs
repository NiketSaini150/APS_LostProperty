using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APS_LostProperty.Areas.Identity.Data;

namespace APS_LostProperty.Models
{
    public class Claim
    {
        public int ClaimID { get; set; }

        // FK to AspNetUsers.Id
        public string UserID { get; set; }

        [ForeignKey("UserID")]
        public User? IdentityUser { get; set; }


        [Required]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string ItemName { get; set; }

        [MinLength(2, ErrorMessage = " Description must be at least 2 characters.")]
        [RegularExpression(@"^[\w\s.,!?'""-]*$", ErrorMessage = "Description contains invalid characters.")]
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [CustomValidation(typeof(Claim), nameof(ValidateDateLost))]
        public DateTime DateLost { get; set; }

        [DataType(DataType.Date)]

        public DateTime DateSubmitted { get; set; } = DateTime.Today;

        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        public int? MatchedLostItemID { get; set; }

        [ForeignKey("MatchedLostItemID")]
        public LostItem? MatchedLostItem { get; set; }


        public static ValidationResult ValidateDateLost(DateTime date, ValidationContext context)
        {
            var today = DateTime.Today;
            var oneYearAgo = today.AddYears(-1);

            if (date > today)
                return new ValidationResult("Date lost cannot be in the future.");

            if (date < oneYearAgo)
                return new ValidationResult("Date lost cannot be more than 1 year ago.");

            return ValidationResult.Success;
        }


    }
    public enum ClaimStatus
    {
        Submitted,
        Approved,
        Rejected,
        Collected
    }


}


