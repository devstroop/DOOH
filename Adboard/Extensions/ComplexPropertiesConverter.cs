using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;

namespace DOOH.Adboard.Extensions
{

    public class ComplexPropertiesConverter<T> : JsonConverter<T>
    {
        //
        // Summary:
        //     The complex properties
        private IEnumerable<string> complexProperties;

        //
        // Summary:
        //     Initializes a new instance of the Radzen.ComplexPropertiesConverter`1 class.
        //
        //
        // Parameters:
        //   complexProperties:
        //     The complex properties.
        public ComplexPropertiesConverter(IEnumerable<string> complexProperties)
        {
            this.complexProperties = complexProperties;
        }

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
        //     T.
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return JsonSerializer.Deserialize<T>(ref reader, options);
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
        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            JsonSerializerOptions jsonSerializerOptions = new JsonSerializerOptions();
            jsonSerializerOptions.Converters.Add(new DateTimeConverterUsingDateTimeParse());
            using (JsonDocument jsonDocument = JsonDocument.Parse(JsonSerializer.Serialize(value, jsonSerializerOptions)))
            {
                foreach (JsonProperty item in jsonDocument.RootElement.EnumerateObject())
                {
                    if (!complexProperties.Contains(item.Name))
                    {
                        item.WriteTo(writer);
                    }
                }
            }

            writer.WriteEndObject();
        }
    }
}
