using System.Text.Json;
using System.Text.Json.Serialization;

namespace Notely.Server.Converters;

public class NullableGuidConverter : JsonConverter<Guid?>
{
    public override Guid? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null)
        {
            return null;
        }

        if (reader.TokenType == JsonTokenType.String)
        {
            var stringValue = reader.GetString();
            
            // Handle empty string as null
            if (string.IsNullOrEmpty(stringValue))
            {
                return null;
            }

            // Try to parse the Guid
            if (Guid.TryParse(stringValue, out var guid))
            {
                return guid;
            }
        }

        throw new JsonException($"Unable to convert \"{reader.GetString()}\" to Guid.");
    }

    public override void Write(Utf8JsonWriter writer, Guid? value, JsonSerializerOptions options)
    {
        if (value.HasValue)
        {
            writer.WriteStringValue(value.Value.ToString());
        }
        else
        {
            writer.WriteNullValue();
        }
    }
}