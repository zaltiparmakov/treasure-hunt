using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Location
    {
        [Key]
        public int LocationID { get; set; }

        [Required]
        [Display(Name = "Location Name")]
        public string Name { get; set; }

        [Required]
        public string Latitude { get; set; }

        [Required]
        public string Longitude { get; set; }

        public virtual ICollection<Question> LocationQuestions { get; set; }

        public virtual ICollection<Question> NextLocationQuestions { get; set; }
    }
}