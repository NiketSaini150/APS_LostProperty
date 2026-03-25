
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace APS_LostProperty.Models

{
        public class Category
        {
            public int CategoryID { get; set; }

            [Required]
            [StringLength(50)]
            public string Name { get; set; }

            // Navigation property
            public ICollection<LostItem>? LostItems { get; set; }
        }
    
}
