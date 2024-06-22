using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("UserInformation", Schema = "dbo")]
    public partial class UserInformation
    {
        [Key]
        [Required]
        public string UserId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public DateTime? SuspendedAt { get; set; }

        public bool? IsSuspended { get; set; }

        public ICollection<Upload> Uploads { get; set; }
    }
}