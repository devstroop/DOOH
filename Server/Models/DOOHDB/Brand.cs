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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }

        [Required]
        public string BrandName { get; set; }

        public string BrandLogo { get; set; }

        public ICollection<Display> Displays { get; set; }

        public ICollection<Motherboard> Motherboards { get; set; }
    }
}