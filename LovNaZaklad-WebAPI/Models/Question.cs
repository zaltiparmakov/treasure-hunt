using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Question
    {
        [Key]
        public int QuestionID { get; set; }

        [Required]
        [Display(Name = "Question")]
        public string QuestionValue { get; set; }

        [Required]
        public string Answer { get; set; }

        public int LocationID { get; set; }

        public int? NextLocationID { get; set; }

        [ForeignKey("LocationID")]
        [InverseProperty("LocationQuestions")]
        public virtual Location Location { get; set; }

        [ForeignKey("NextLocationID")]
        [InverseProperty("NextLocationQuestions")]
        public virtual Location NextLocation { get; set; }
    }
}