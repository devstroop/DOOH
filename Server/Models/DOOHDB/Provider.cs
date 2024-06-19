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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProviderId { get; set; }

        [Required]
        public string ContactName { get; set; }

        public string CompanyName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string CityName { get; set; }

        public City City { get; set; }

        [Required]
        public string StateName { get; set; }

        public State State { get; set; }

        [Required]
        public string CountryName { get; set; }

        public Country Country { get; set; }

        public string UserId { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<Earning> Earnings { get; set; }
    }
}