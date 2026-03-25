using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace APS_LostProperty.Models
{
   
 
        public class Claim
        {
            public int ClaimID { get; set; }

            public string UserID { get; set; }
            [ForeignKey("UserID")]
           // public ApplicationUser User { get; set; }

            [Required]
            [StringLength(100)]
            public string ItemName { get; set; }

            [StringLength(200)]
            public string? Description { get; set; }

            [Required]
            public DateTime DateLost { get; set; }

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

