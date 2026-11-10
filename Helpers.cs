using LabApi.Features.Wrappers;
using PlayerRoles;
using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using UnityEngine;
using static Respawning.RespawnTokensManager;
namespace SCP_ULEPSZATOR;


public static class Helpers
{
    /// <summary>
    /// This excludes 049-2, 096 and 079
    /// </summary>
    /// <param name="roles"></param>
    /// <returns></returns>
    public static bool ContainsStandardScp(this List<RoleTypeId> roles, out int count)
    {
        List<RoleTypeId> list = new()
        {
            RoleTypeId.Scp049,
            RoleTypeId.Scp106,
            RoleTypeId.Scp173,
            RoleTypeId.Scp3114,
            RoleTypeId.Scp939
        };

        count = 0;

        foreach(var scp in list)
        {
            if (list.Contains(scp))
                count++;
        }

        return count > 0;
    }
    public static Vector3 ModifyVector3(this Vector3 vector, Func<float, float> fun)
    {
        return new Vector3(fun.Invoke(vector.x), fun.Invoke(vector.y), fun.Invoke(vector.z));
    }

    public static string GetHexString(this Color color)
    {
        Color32 c = color;
        return $"#{c.r:X2}{c.g:X2}{c.b:X2}";
    }

    public static Color GetHexColor(this string hex)
    {
        if (string.IsNullOrWhiteSpace(hex))
            return Color.white;

        hex = hex.Trim().TrimStart('#');

        if (hex.Length != 6 && hex.Length != 8)
            return Color.white;

        if (!ColorUtility.TryParseHtmlString("#" + hex, out Color color))
            return Color.white;

        return color;
    }

    public static List<Milestone> GetMilestones(this int[] numbers)
    {
        var lista = new List<Milestone>();
        foreach (var token in numbers)
            lista.Add(new(token));
        return lista;
    }

    public static void GivePlayerIfNeeded(this Player p, ItemType type)
    {
        if (p?.Items.Count(x => x.Type == type) > 0)
            return;

        Core.Instance.LogAll($"Added item ({type}) to: {p.DisplayName} ");
        p?.AddItem(type);
    }

    public static RGB HexadecimalToRGB(string hex)
    {
        if (hex.StartsWith("#"))
            hex = hex.Remove(0, 1);

        byte r = (byte)HexadecimalToDecimal(hex.Substring(0, 2));
        byte g = (byte)HexadecimalToDecimal(hex.Substring(2, 2));
        byte b = (byte)HexadecimalToDecimal(hex.Substring(4, 2));

        return new RGB(r, g, b);
    }

    private static int HexadecimalToDecimal(string hex)
    {
        hex = hex.ToUpper();

        int hexLength = hex.Length;
        double dec = 0;

        for (int i = 0; i < hexLength; ++i)
        {
            byte b = (byte)hex[i];

            if (b >= 48 && b <= 57)
                b -= 48;
            else if (b >= 65 && b <= 70)
                b -= 55;

            dec += b * Math.Pow(16, ((hexLength - i) - 1));
        }

        return (int)dec;
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
}
