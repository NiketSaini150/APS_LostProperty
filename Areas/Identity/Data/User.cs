using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using APS_LostProperty.Models;

namespace APS_LostProperty.Areas.Identity.Data
{ 
    // Add profile data for application users by adding properties to the User class
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]      
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        public DateTime DateRegistered { get; set; } = DateTime.Now;

        // Navigation property - resolved to APS_LostProperty.Models.Claim
        public ICollection<Claim>? Claims { get; set; }
    }
}





