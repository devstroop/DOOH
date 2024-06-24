using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("CampaignSchedule", Schema = "dbo")]
    public partial class CampaignSchedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required]
        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        public string Label { get; set; }

        public int Rotation { get; set; }

        public DateTime Start { get; set; }

        [Required]
        public DateTime End { get; set; }
    }
}