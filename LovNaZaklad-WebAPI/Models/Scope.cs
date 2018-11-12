using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Scope
    {
        [Key]
        public int ScopeID { get; set; }

        public string ScopeValue { get; set; }
    }
}