using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("CampaignAdboard", Schema = "dbo")]
    public partial class CampaignAdboard
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
        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        [Key]
        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }
    }
}