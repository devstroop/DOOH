using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardToken", Schema = "dbo")]
    public partial class AdboardToken
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

        [ConcurrencyCheck]
        public string RefreshToken { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime RefreshTokenExpiry { get; set; }
    }
}