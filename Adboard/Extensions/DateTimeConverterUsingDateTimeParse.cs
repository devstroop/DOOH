using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DOOH.Adboard.Extensions
{
    public class DateTimeConverterUsingDateTimeParse : JsonConverter<DateTime>
    {
        //
        // Summary:
        //     Reads the specified reader.
        //
        // Parameters:
        //   reader:
        //     The reader.
        //
        //   typeToConvert:
        //     The type to convert.
        //
        //   options:
        //     The options.
        //
        // Returns:
        //     DateTime.
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return DateTime.Parse(reader.GetString());
        }

        //
        // Summary:
        //     Writes the specified writer.
        //
        // Parameters:
        //   writer:
        //     The writer.
        //
        //   value:
        //     The value.
        //
        //   options:
        //     The options.
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("yyyy-MM-ddTHH:mm:ss.fffZ", CultureInfo.InvariantCulture));
        }
    }
}
