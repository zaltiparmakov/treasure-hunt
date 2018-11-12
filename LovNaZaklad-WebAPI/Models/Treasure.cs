using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Treasure
    {
        [Key]
        public int TreasureID { get; set; }

        [Required]
        public string Name { get; set; }

        public int LocationID { get; set; }

        public virtual Location Location { get; set; }

        public virtual Image Image { get; set; }
    }
}