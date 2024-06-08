using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("City", Schema = "dbo")]
    public partial class City
    {
        [Key]
        [Required]
        public string CityName { get; set; }

        [Required]
        public string StateName { get; set; }

        public State State { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<Company> Companies { get; set; }

        public ICollection<Provider> Providers { get; set; }
    }
}