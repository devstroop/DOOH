using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Page", Schema = "dbo")]
    public partial class Page
    {
        [Key]
        [Required]
        public string Slag { get; set; }

        public string Content { get; set; }
    }
}