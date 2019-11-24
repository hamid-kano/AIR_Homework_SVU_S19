using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AIR_SVU_S19.Models
{
    public class OrderTerms_DocsBoolean
    {
        [Key]
        public int ID { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "field must be atleast 1 character")]
        public string Term { get; set; }
        [StringLength(200, MinimumLength = 1, ErrorMessage = "field must be atleast 1 character")]
        public string Docs { get; set; }
    }
}