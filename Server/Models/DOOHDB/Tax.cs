using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Tax", Schema = "dbo")]
    public partial class Tax
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
        public int TaxId { get; set; }

        [Required]
        [ConcurrencyCheck]
        public string TaxName { get; set; }

        [ConcurrencyCheck]
        public decimal TaxRate { get; set; }

        [ConcurrencyCheck]
        public int? ParentTaxId { get; set; }

        public Tax Tax1 { get; set; }

        public ICollection<Billing> Billings { get; set; }

        public ICollection<Tax> Taxes1 { get; set; }
    }
}