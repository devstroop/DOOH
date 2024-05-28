using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Category", Schema = "dbo")]
    public partial class Category
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
        public int CategoryId { get; set; }

        [ConcurrencyCheck]
        public string CategoryName { get; set; }

        [ConcurrencyCheck]
        public decimal? Commission { get; set; }

        [ConcurrencyCheck]
        public string CategoryDescription { get; set; }

        [ConcurrencyCheck]
        public string CategoryColor { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}