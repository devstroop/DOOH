using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Country", Schema = "dbo")]
    public partial class Country
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
        [Required]
        public string CountryName { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<Provider> Providers { get; set; }

        public ICollection<State> States { get; set; }
    }
}