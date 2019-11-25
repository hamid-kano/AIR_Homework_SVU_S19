using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AIR_SVU_S19.Models
{
    public class Term_Document
    {
        [Key]
        public int ID { get; set; }
        public string Terms { get; set; }
        public string Docs { get; set; }
        public string Freg_Term_in_docs { get; set; }

    }
}