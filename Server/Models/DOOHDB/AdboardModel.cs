using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardModel", Schema = "dbo")]
    public partial class AdboardModel
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
        public int AdboardModelId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Model { get; set; }

        [ConcurrencyCheck]
        public int? DisplayId { get; set; }

        public Display Display { get; set; }

        [ConcurrencyCheck]
        public int? MotherboardId { get; set; }

        public Motherboard Motherboard { get; set; }

        [ConcurrencyCheck]
        public string Image { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}