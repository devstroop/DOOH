using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("ScheduleAdvertisement", Schema = "dbo")]
    public partial class ScheduleAdvertisement
    {
        [Key]
        [Required]
        public int ScheduleId { get; set; }

        public Schedule Schedule { get; set; }

        [Key]
        [Required]
        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }
    }
}