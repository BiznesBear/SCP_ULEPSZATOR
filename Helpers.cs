using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
using static Respawning.RespawnTokensManager;
namespace SCP_ULEPSZATOR;


public static class Helpers
{
    public static Vector3 ModifyVector3(this Vector3 vector, Func<float, float> fun)
    {
        return new Vector3(fun.Invoke(vector.x), fun.Invoke(vector.y), fun.Invoke(vector.z));
    }

    public static List<Milestone> GetMilestones(this int[] numbers)
    {
        var lista = new List<Milestone>();
        foreach (var token in numbers)
            lista.Add(new(token));
        return lista;
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


public struct RGB(byte r, byte g, byte b)
{
    public byte R { get; set; } = r;
    public byte G { get; set; } = g;
    public byte B { get; set; } = b;
    public readonly Color GetColor() => new(R/255, G/255, B / 255);    
    public static RGB FromColor(Color color) => new ((byte) (color.r * 255), (byte) (color.g * 255), (byte) (color.b * 255));    
}
