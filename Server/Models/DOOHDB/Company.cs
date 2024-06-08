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

        public string LogoDark { get; set; }

        public string LogoLight { get; set; }

        public string Favicon { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string GSTIN { get; set; }

        public string PAN { get; set; }

        public string CIN { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string CityName { get; set; }

        public City City { get; set; }

        public string StateName { get; set; }

        public State State { get; set; }

        public string CountryName { get; set; }

        public Country Country { get; set; }
    }
}