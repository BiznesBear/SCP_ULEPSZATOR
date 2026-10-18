
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
namespace SCP_ULEPSZATOR;
public static class Helpers
{
    public static Vector3 ModifyVector3(this Vector3 vector, Func<float, float> fun)
    {
        return new Vector3(fun.Invoke(vector.x), fun.Invoke(vector.y), fun.Invoke(vector.z));
    }
}

public class JsonConverterFloat : JsonConverter<float>
{
    public override float Read(ref Utf8JsonReader reader,
        Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.GetSingle();
    }

    public override void Write(Utf8JsonWriter writer, float value,
        JsonSerializerOptions options)
    {
        writer.WriteRawValue(value.ToString("0.##",
            CultureInfo.InvariantCulture));
    }
}