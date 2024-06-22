using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardStatus", Schema = "dbo")]
    public partial class AdboardStatus
    {
        [Key]
        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        public bool Connected { get; set; }

        public DateTime ConnectedAt { get; set; }

        public int Delay { get; set; }
    }
}