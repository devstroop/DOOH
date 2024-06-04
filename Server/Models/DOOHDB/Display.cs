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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DisplayId { get; set; }

        [Required]
        public int BrandId { get; set; }

        public Brand Brand { get; set; }

        [Required]
        public string Model { get; set; }

        public int? PixelWidth { get; set; }

        public int? PixelHeight { get; set; }

        public double? ScreenWidth { get; set; }

        public double? ScreenHeight { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}