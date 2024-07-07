using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Earning", Schema = "dbo")]
    public partial class Earning
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int EarningId { get; set; }

        [Required]
        public int ProviderId { get; set; }

        public Provider Provider { get; set; }

        [Required]
        public int AnalyticId { get; set; }

        public Analytic Analytic { get; set; }

        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        public int TotalDuration { get; set; }

        public decimal? RatePerSecond { get; set; }

        public decimal? TotalAmountBeforeTax { get; set; }

        public decimal? EarningPercent { get; set; }

        public decimal? EarningAmount { get; set; }
    }
}