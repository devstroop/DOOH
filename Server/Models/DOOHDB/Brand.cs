using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Brand", Schema = "dbo")]
    public partial class Brand
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
        public int BrandId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string BrandName { get; set; }

        [ConcurrencyCheck]
        public string BrandLogo { get; set; }

        public ICollection<Display> Displays { get; set; }

        public ICollection<Motherboard> Motherboards { get; set; }
    }
}