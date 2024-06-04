using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("AdboardNetwork", Schema = "dbo")]
    public partial class AdboardNetwork
    {
        [Key]
        [Required]
        public int AdboardId { get; set; }

        public Adboard Adboard { get; set; }

        public string PhysicalAddress { get; set; }

        public string LocalIP { get; set; }

        public string PublicIP { get; set; }

        public string Subnet { get; set; }

        public string Gateway { get; set; }

        public string DNS { get; set; }
    }
}