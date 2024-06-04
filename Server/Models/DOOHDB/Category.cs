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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }

        public string CategoryName { get; set; }

        public decimal? Commission { get; set; }

        public string CategoryDescription { get; set; }

        public string CategoryColor { get; set; }

        public ICollection<Adboard> Adboards { get; set; }
    }
}