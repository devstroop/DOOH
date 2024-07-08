using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Schedule", Schema = "dbo")]
    public partial class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [Required]
        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        public int Rotation { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public string Label { get; set; }

        public ICollection<ScheduleAdboard> ScheduleAdboards { get; set; }

        public ICollection<ScheduleAdvertisement> ScheduleAdvertisements { get; set; }
    }
}