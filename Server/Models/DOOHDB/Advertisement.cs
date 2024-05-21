using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Advertisement", Schema = "dbo")]
    public partial class Advertisement
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
        public int AdvertisementId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string AttachmentKey { get; set; }

        public Attachment Attachment { get; set; }

        [Required]
        [ConcurrencyCheck]
        public double Duration { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        [ConcurrencyCheck]
        public bool? IsVerified { get; set; }

        [ConcurrencyCheck]
        public DateTime? VerifiedAt { get; set; }

        public ICollection<Analytic> Analytics { get; set; }
    }
}