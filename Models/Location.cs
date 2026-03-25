
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
    public class Location
    {
        public int LocationID { get; set; }

        [Required]
        [StringLength(100)]
        public string LocationName { get; set; }

        // Navigation property
        public ICollection<LostItem>? LostItems { get; set; }
    }
}

