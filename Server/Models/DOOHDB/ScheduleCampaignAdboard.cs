using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("ScheduleCampaignAdboard", Schema = "dbo")]
    public partial class ScheduleCampaignAdboard
    {
        [Key]
        [Required]
        public int ScheduleId { get; set; }

        public Schedule Schedule { get; set; }

        [Key]
        [Required]
        public int AdboardId { get; set; }

        public CampaignAdboard CampaignAdboard { get; set; }

        [Key]
        [Required]
        public int CampaignId { get; set; }
    }
}