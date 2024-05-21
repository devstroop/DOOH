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

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string SSID { get; set; }

        [ConcurrencyCheck]
        public string Password { get; set; }

        [ConcurrencyCheck]
        public DateTime? ConnectedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        [ConcurrencyCheck]
        public bool? HasConnected { get; set; }
    }
}