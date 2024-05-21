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

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        [JsonPropertyName("@odata.etag")]
        public string ETag
        {
            get;
            set;
        }

        [Key]
        [Required]
        public string AttachmentKey { get; set; }

        [ConcurrencyCheck]
        public string FileName { get; set; }

        [ConcurrencyCheck]
        public string Thumbnail { get; set; }

        [ConcurrencyCheck]
        public long? Size { get; set; }

        [ConcurrencyCheck]
        public string ContentType { get; set; }

        [ConcurrencyCheck]
        public string AspectRatio { get; set; }

        [ConcurrencyCheck]
        public double? Duration { get; set; }

        [ConcurrencyCheck]
        public string CodecName { get; set; }

        [ConcurrencyCheck]
        public int? Height { get; set; }

        [ConcurrencyCheck]
        public int? Width { get; set; }

        [ConcurrencyCheck]
        public int? BitRate { get; set; }

        [ConcurrencyCheck]
        public string FrameRate { get; set; }

        [ConcurrencyCheck]
        public DateTime CreatedAt { get; set; }

        [ConcurrencyCheck]
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Adboard> Adboards { get; set; }

        public ICollection<AdboardModel> AdboardModels { get; set; }

        public ICollection<Advertisement> Advertisements { get; set; }

        public ICollection<Brand> Brands { get; set; }
    }
}