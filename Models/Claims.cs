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
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 30 characters.")]
        [RegularExpression("^[A-Za-z]+( [A-Za-z]+)*$", ErrorMessage = "Only letters and single spaces between words are allowed.")]
        public string ItemName { get; set; }

        [StringLength(200, MinimumLength = 2, ErrorMessage = "Hall name must be between 2 and 30 characters.")]
      
        public string? Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateLost { get; set; }

        [DataType(DataType.Date)]
        public DateTime DateSubmitted { get; set; } = DateTime.Now;

        public ClaimStatus Status { get; set; } = ClaimStatus.Submitted;

        public int? MatchedLostItemID { get; set; }

        [ForeignKey("MatchedLostItemID")]
        public LostItem? MatchedLostItem { get; set; }
    }

    public enum ClaimStatus
    {
        Submitted,
        Approved,
        Rejected,
        Collected
    }
}

