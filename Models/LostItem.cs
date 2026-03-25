
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
            [StringLength(100)]
            public string ItemName { get; set; }

            [StringLength(200)]
            public string? Description { get; set; }

            [Required]
            public DateTime DateFound { get; set; }

            public int CategoryID { get; set; }
            [ForeignKey("CategoryID")]
            public Category? Category { get; set; }

            public int LocationID { get; set; }
            [ForeignKey("LocationID")]
            public Location? Location { get; set; }

            public bool IsClaimed { get; set; } = false;

            public ICollection<Claim>? Claims { get; set; }
    }
 }

