using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardNetwork", Schema = "dbo")]
    public partial class AdboardNetwork
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
        public string PhysicalAddress { get; set; }

        [ConcurrencyCheck]
        public string LocalIP { get; set; }

        [ConcurrencyCheck]
        public string PublicIP { get; set; }

        [ConcurrencyCheck]
        public string Subnet { get; set; }

        [ConcurrencyCheck]
        public string Gateway { get; set; }

        [ConcurrencyCheck]
        public string DNS { get; set; }
    }
}