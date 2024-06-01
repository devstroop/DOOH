using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardImage", Schema = "dbo")]
    public partial class AdboardImage
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdboardImageId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Image { get; set; }
    }
}