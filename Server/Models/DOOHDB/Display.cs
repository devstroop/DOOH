using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Display", Schema = "dbo")]
    public partial class Display
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
        public int DisplayId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string Model { get; set; }

        [ConcurrencyCheck]
        public int? PixelWidth { get; set; }

        [ConcurrencyCheck]
        public int? PixelHeight { get; set; }

        [ConcurrencyCheck]
        public double? ScreenWidth { get; set; }

        [ConcurrencyCheck]
        public double? ScreenHeight { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}