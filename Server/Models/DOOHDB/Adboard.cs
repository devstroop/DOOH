using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Adboard", Schema = "dbo")]
    public partial class Adboard
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdboardId { get; set; }

        public int? ProviderId { get; set; }

        public Provider Provider { get; set; }

        public int? DisplayId { get; set; }

        public Display Display { get; set; }

        public int? MotherboardId { get; set; }

        public Motherboard Motherboard { get; set; }

        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        public int? DailyCapacityInSeconds { get; set; }

        public decimal? BaseRatePerSecond { get; set; }

        public double Latitude { get; set; }

        public double Longitude { get; set; }

        public string SignName { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<AdboardImage> AdboardImages { get; set; }

        public ICollection<AdboardNetwork> AdboardNetworks { get; set; }

        public ICollection<AdboardStatus> AdboardStatuses { get; set; }

        public ICollection<AdboardWifi> AdboardWifis { get; set; }

        public ICollection<Analytic> Analytics { get; set; }

        public ICollection<CampaignAdboard> CampaignAdboards { get; set; }

        public ICollection<Earning> Earnings { get; set; }

        public ICollection<ScheduleAdboard> ScheduleAdboards { get; set; }
    }
}