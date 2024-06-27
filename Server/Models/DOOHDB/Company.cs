using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Company", Schema = "dbo")]
    public partial class Company
    {
        [Key]
        [Required]
        public string Key { get; set; }

        [Required]
        public string CompanyName { get; set; }

        public string Slogan { get; set; }

        public string Icon { get; set; }

        public string Logo { get; set; }

        public string AdminLogo { get; set; }

        public string ProviderLogo { get; set; }

        public string LoginLogo { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string GSTIN { get; set; }

        public string PAN { get; set; }

        public string CIN { get; set; }

        public string Address { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }
    }
}