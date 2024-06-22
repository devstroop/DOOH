using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DOOH.Server.Models.DOOHDB
{
    [Table("Attachment", Schema = "dbo")]
    public partial class Attachment
    {
        [Key]
        [Required]
        public string AttachmentKey { get; set; }

        public string FileName { get; set; }

        public string Thumbnail { get; set; }

        public long? Size { get; set; }

        public string ContentType { get; set; }

        public string AspectRatio { get; set; }

        public double? Duration { get; set; }

        public string CodecName { get; set; }

        public int? Height { get; set; }

        public int? Width { get; set; }

        public int? BitRate { get; set; }

        public string FrameRate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public string Owner { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }
    }
}