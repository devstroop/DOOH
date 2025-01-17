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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TaxId { get; set; }

        [Required]
        public string TaxName { get; set; }

        public decimal TaxRate { get; set; }

        public int? ParentTaxId { get; set; }

        public Tax Tax1 { get; set; }

        public ICollection<Tax> Taxes1 { get; set; }
    }
}