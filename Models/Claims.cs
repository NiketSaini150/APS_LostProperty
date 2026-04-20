using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using APS_LostProperty.Areas.Identity.Data;

namespace APS_LostProperty.Models
{
    // This model represents a claim submitted by a user
    // A claim is created when a user has lost a item and is in search for it 
    public class Claim
    {
        // Primary Key for the Claim table
        // Uniquely identifies each claim
        public int ClaimID { get; set; }

        // Foreign Key linking this claim to the ASP.NET Identity user
        // This stores the ID of the logged-in user who submitted the claim
        // FK to AspNetUsers.Id
        public string UserID { get; set; }

        // Navigation property linking the claim to the User object
        // Allows Entity Framework to access the full user details
        [ForeignKey("UserID")]
        public User? IdentityUser { get; set; }


        // Name of the item the user is claiming
        // This field is required
        [Required]

        // Validation rule ensuring the item name has at least 2 characters
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters.")]

        // Regular expression validation
        // Allows letters, numbers, spaces, apostrophes, and dashes
        // Prevents special characters such as @ or #
        [RegularExpression("^[A-Za-z0-9'\\- ]+$", ErrorMessage = "Only letters, numbers, spaces, apostrophes, and dashes are allowed.")]
        public string ClaimedItemName { get; set; }

    

        // Regex validation allowing words, spaces, punctuation, and common symbols
        // Prevents unsafe or unexpected characters
        [RegularExpression(@"^[\w\s.,!?'""-]*$", ErrorMessage = "Description contains invalid characters.")]
        public string? ClaimedDescription { get; set; }

        // Date the user says the item was lost
        // This field is required
        [Required]

        // Ensures only the date (not time) is stored
        [DataType(DataType.Date)]

        // Custom validation method used to check if the date is valid
        // Prevents future dates and dates older than one year
        
        public DateTime DateLost { get; set; }

        // Date the claim was submitted to the system
        // Automatically set to today's date
        [DataType(DataType.Date)]

        public DateTime DateSubmitted { get; set; } = DateTime.Today;

        // Status of the claim in the system
        // Default value is "Submitted"
        // This status changes when staff review the claim
        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        // Foreign Key linking the claim to a matched lost item
        // Nullable because the claim may not be matched immediately
        public int? MatchedLostItemID { get; set; }

        // Navigation property linking to the matched LostItem
        // Allows staff to connect a claim with a found item
        [ForeignKey("MatchedLostItemID")]
        public LostItem? MatchedLostItem { get; set; }


        // Custom validation method used for DateLost
        // This method checks that:
        // 1. The date cannot be in the future
        // 2. The date cannot be more than 1 year old
       


    }

    // Enum representing the different statuses a claim can have
    // Used to track the progress of a claim
    public enum ClaimStatus
    {
        Submitted,   // Claim has been submitted but not reviewed
        Approved,    // Claim has been verified and approved
        Rejected,    // Claim was reviewed but rejected
        Collected    // Item has been collected by the claimant
    }


}