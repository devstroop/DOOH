using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("CampaignCriteria", Schema = "dbo")]
    public partial class CampaignCriterion
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CampaignCriteriaId { get; set; }

        public long? MinRotationPerDay { get; set; }

        public long? MaxRotationPerDay { get; set; }

        public long? MinPlaytimePerDay { get; set; }

        public long? MaxPlaytimePerDay { get; set; }

        public long? MinAdboardPerCampaign { get; set; }

        public long? MaxAdboardPerCampaign { get; set; }
    }
}