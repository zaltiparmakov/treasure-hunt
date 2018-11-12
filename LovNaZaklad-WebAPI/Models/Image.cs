using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace LovNaZaklad_WebAPI.Models
{
    public class Image
    {
        [Key]
        public int ImageID { get; set; }

        public string Path { get; set; }

        public string Features { get; set; }
    }
}