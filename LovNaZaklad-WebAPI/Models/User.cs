using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [EmailAddress]
        [Required]
        public string Email { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(5)]
        public string Password { get; set; }

        [MinLength(5)]
        [MaxLength(30)]
        public string FirstName { get; set; }

        [MinLength(5)]
        [MaxLength(50)]
        public string LastName { get; set; }

        public virtual string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirth { get; set; }

        public int Points { get; set; }

        public int RoleID { get; set; }

        public virtual Role Role { get; set; }
    }
}