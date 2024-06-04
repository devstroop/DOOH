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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int MotherboardId { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public string Cpu { get; set; }

        public string Ram { get; set; }

        public string Rom { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}