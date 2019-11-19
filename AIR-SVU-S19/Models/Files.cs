using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AIR_SVU_S19.Models
{
    public class Files
    {
        [Key]
        public int File_ID { get; set; }
        [StringLength(100, MinimumLength = 1, ErrorMessage = "field must be atleast 1 character")]
        public string File_Name { get; set; }
        public string File_content { get; set; }
    }
}