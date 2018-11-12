using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Role
    {
        [Key]
        public int RoleID { get; set; }

        [Required]
        public string RoleName { get; set; }

        public int ScopeID { get; set; }

        public virtual Scope Scope { get; set; }
    }
}