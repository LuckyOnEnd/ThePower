using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThePower.Models
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? IsPaid { get; set; }
        public DateTime? Term { get; set; }
        public string? City { get; set; }
        public string? StreetAdress { get; set; }
        public string? PostalCode { get; set; }
    }
}
