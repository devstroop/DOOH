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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertisementId { get; set; }

        [Required]
        public int CampaignId { get; set; }

        public Campaign Campaign { get; set; }

        [Required]
        public string UploadKey { get; set; }

        public Upload Upload { get; set; }

        [Required]
        public double Duration { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public bool? IsVerified { get; set; }

        public DateTime? VerifiedAt { get; set; }

        public ICollection<Analytic> Analytics { get; set; }

        public ICollection<ScheduleAdvertisement> ScheduleAdvertisements { get; set; }
    }
}