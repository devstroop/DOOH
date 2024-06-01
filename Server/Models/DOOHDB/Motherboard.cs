using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Motherboard", Schema = "dbo")]
    public partial class Motherboard
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
        public int MotherboardId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Model { get; set; }

        [ConcurrencyCheck]
        public string Cpu { get; set; }

        [ConcurrencyCheck]
        public string Ram { get; set; }

        [ConcurrencyCheck]
        public string Rom { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}