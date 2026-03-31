using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using APS_LostProperty.Models;

namespace APS_LostProperty.Areas.Identity.Data
{
    // This class extends the ASP.NET IdentityUser class to add
    // additional profile data specific to our application.
    // It represents a user of the APS Lost Property system.
    public class User : IdentityUser
    {
        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty; // User's first name

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty; // User's last name

        public DateTime DateRegistered { get; set; } = DateTime.Now; // The date the user registered in the system

        // Navigation property linking this user to their submitted claims
        // This allows querying all claims associated with this user
        public ICollection<Claim>? Claims { get; set; }
    }
}