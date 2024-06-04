using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardWifi", Schema = "dbo")]
    public partial class AdboardWifi
    {
        [Key]
        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [Required]
        public string SSID { get; set; }

        public string Password { get; set; }

        public DateTime? ConnectedAt { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? HasConnected { get; set; }
    }
}