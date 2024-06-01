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
        public int AdboardId { get; set; }

        [ConcurrencyCheck]
        public int? ProviderId { get; set; }

        public Provider Provider { get; set; }

        [ConcurrencyCheck]
        public int? DisplayId { get; set; }

        public Display Display { get; set; }

        [ConcurrencyCheck]
        public int? MotherboardId { get; set; }

        public Motherboard Motherboard { get; set; }

        [ConcurrencyCheck]
        public int? CategoryId { get; set; }

        public Category Category { get; set; }

        [ConcurrencyCheck]
        public decimal? BaseRatePerSecond { get; set; }

        [ConcurrencyCheck]
        public double? Latitude { get; set; }

        [ConcurrencyCheck]
        public double? Longitude { get; set; }

        [ConcurrencyCheck]
        public string Address { get; set; }

        [ConcurrencyCheck]
        public string CityName { get; set; }

        public City City { get; set; }

        [ConcurrencyCheck]
        public string StateName { get; set; }

        public State State { get; set; }

        [ConcurrencyCheck]
        public string CountryName { get; set; }

        public Country Country { get; set; }

        [ConcurrencyCheck]
        public bool IsActive { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<AdboardImage> AdboardImages { get; set; }

        public ICollection<AdboardNetwork> AdboardNetworks { get; set; }

        public ICollection<AdboardWifi> AdboardWifis { get; set; }

        public ICollection<Analytic> Analytics { get; set; }

        public ICollection<CampaignAdboard> CampaignAdboards { get; set; }

        public ICollection<Earning> Earnings { get; set; }
    }
}