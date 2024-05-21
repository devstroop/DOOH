using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Analytic", Schema = "dbo")]
    public partial class Analytic
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
        public int AnalyticId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        [Required]
        [ConcurrencyCheck]
        public DateTime StartedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime StoppedAt { get; set; }

        [ConcurrencyCheck]
        public int? Duration { get; set; }

        [ConcurrencyCheck]
        public int? TotalAttention { get; set; }

        [ConcurrencyCheck]
        public int? UniqueAttention { get; set; }

        [ConcurrencyCheck]
        public DateTime UpdatedAt { get; set; }

        public ICollection<Billing> Billings { get; set; }

        public ICollection<Earning> Earnings { get; set; }
    }
}