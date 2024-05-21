using System.Text.Json;
using System.Xml.Linq;

namespace DOOH.Adboard.Extensions
{
    public static class HttpResponseMessageExtensions
    {

        public static async Task<T> ReadAsync<T>(this HttpResponseMessage response)
        {
            try
            {
                response.EnsureSuccessStatusCode();
                using Stream stream = await response.Content.ReadAsStreamAsync();
                return (stream.Length <= 0) ? default(T) : (await JsonSerializer.DeserializeAsync<T>(stream, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    PropertyNameCaseInsensitive = true
                }));
            }
            catch
            {
                string text = await response.Content.ReadAsStringAsync();
                if (!string.IsNullOrEmpty(text))
                {
                    if (response.Content.Headers.ContentType.MediaType == "application/json")
                    {
                        JsonDocument jsonDocument;
                        try
                        {
                            jsonDocument = JsonDocument.Parse(text);
                        }
                        catch
                        {
                            throw new Exception("Unable to parse the response.");
                        }

                        if (jsonDocument.RootElement.TryGetProperty("error", out var value) && value.TryGetProperty("message", out var value2))
                        {
                            throw new Exception(value2.GetString());
                        }
                    }
                    else
                    {
                        XElement xElement2;
                        try
                        {
                            XDocument xDocument = XDocument.Parse(text);
                            XElement xElement = xDocument.Descendants().SingleOrDefault((XElement p) => p.Name.LocalName == "internalexception");
                            xElement2 = ((xElement == null) ? xDocument.Descendants().SingleOrDefault((XElement p) => p.Name.LocalName == "error") : xElement);
                        }
                        catch
                        {
                            throw new Exception("Unable to parse the response.");
                        }

                        if (xElement2 != null)
                        {
                            throw new Exception(xElement2.Value);
                        }
                    }
                }

                throw;
            }
        }
    }
}