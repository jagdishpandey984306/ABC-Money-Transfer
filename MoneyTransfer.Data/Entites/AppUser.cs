using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MoneyTransfer.Data.Entites
{
    public class AppUser : IdentityUser
    {
        [StringLength(100)]
        [MaxLength(100)]
        [Required]
        public string? FirstName { get; set; }

        [StringLength(100)]
        public string? MiddleName { get; set; }

        [StringLength(100)]
        [Required]
        public string? LastName { get; set; }

        [StringLength(20)]
        [Required]
        public string? Gender { get; set; }

        [StringLength(100)]
        [Required]
        public string? Country { get; set; }

        [StringLength(150)]
        [Required]
        public string? Address { get; set; }
    }
}
