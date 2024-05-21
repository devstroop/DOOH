using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DOOH.Adboard.Models
{
    public class ODataServiceResult<T>
    {
        //
        // Summary:
        //     Gets or sets the count.
        //
        // Value:
        //     The count.
        [JsonPropertyName("@odata.count")]
        public int Count { get; set; }

        //
        // Summary:
        //     Gets or sets the value.
        //
        // Value:
        //     The value.
        public IEnumerable<T> Value { get; set; }
    }
}
