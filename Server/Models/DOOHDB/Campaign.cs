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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CampaignId { get; set; }

        [Required]
        public string CampaignName { get; set; }

        public string BudgetType { get; set; }

        public decimal? Budget { get; set; }

        public int? ScheduleId { get; set; }

        public Schedule Schedule { get; set; }

        public int? StatusId { get; set; }

        public Status Status { get; set; }

        public bool IsDraft { get; set; }

        public bool IsSuspended { get; set; }

        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }

        public ICollection<CampaignAdboard> CampaignAdboards { get; set; }
    }
}