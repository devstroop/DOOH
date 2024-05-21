using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Provider", Schema = "dbo")]
    public partial class Provider
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
        public int ProviderId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ProviderName { get; set; }

        [ConcurrencyCheck]
        public string ProviderCompany { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ProviderEmail { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string ProviderPhone { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Address { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string CityName { get; set; }

        public City City { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string StateName { get; set; }

        public State State { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string CountryName { get; set; }

        public Country Country { get; set; }

        [ConcurrencyCheck]
        public string UserId { get; set; }

        [ConcurrencyCheck]
        public bool IsActive { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<Earning> Earnings { get; set; }
    }
}