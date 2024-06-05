using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Policy", Schema = "dbo")]
    public partial class Policy
    {
        [Key]
        [Required]
        public string Id { get; set; }

        public string Content { get; set; }
    }
}