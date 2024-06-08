using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("State", Schema = "dbo")]
    public partial class State
    {
        [Key]
        [Required]
        public string StateName { get; set; }

        [Required]
        public string CountryName { get; set; }

        public Country Country { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<City> Cities { get; set; }

        public ICollection<Company> Companies { get; set; }

        public ICollection<Provider> Providers { get; set; }
    }
}