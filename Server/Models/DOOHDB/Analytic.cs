using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Analytic", Schema = "dbo")]
    public partial class Analytic
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AnalyticId { get; set; }

        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        [Required]
        public int AdvertisementId { get; set; }

        public Advertisement Advertisement { get; set; }

        [Required]
        public DateTime StartedAt { get; set; }

        public DateTime StoppedAt { get; set; }

        public int? Duration { get; set; }

        public int? TotalAttention { get; set; }

        public int? UniqueAttention { get; set; }

        public DateTime UpdatedAt { get; set; }

        public ICollection<Billing> Billings { get; set; }

        public ICollection<Earning> Earnings { get; set; }
    }
}