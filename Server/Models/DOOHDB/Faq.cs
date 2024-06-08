using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Faq", Schema = "dbo")]
    public partial class Faq
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FaqId { get; set; }

        [Required]
        public string Question { get; set; }

        public string Answer { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}