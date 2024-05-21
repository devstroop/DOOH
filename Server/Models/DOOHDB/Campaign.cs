using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Campaign", Schema = "dbo")]
    public partial class Campaign
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
        public int CampaignId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string CampaignName { get; set; }

        [ConcurrencyCheck]
        public DateTime StartDate { get; set; }

        [ConcurrencyCheck]
        public DateTime? EndDate { get; set; }

        [ConcurrencyCheck]
        public string BudgetType { get; set; }

        [ConcurrencyCheck]
        public decimal? Budget { get; set; }

        [ConcurrencyCheck]
        public bool IsActive { get; set; }

        [ConcurrencyCheck]
        public bool IsSuspended { get; set; }

        [ConcurrencyCheck]
        public string UserId { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }

        public ICollection<CampaignAdboard> CampaignAdboards { get; set; }
    }
}