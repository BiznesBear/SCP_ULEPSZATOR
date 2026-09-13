using HarmonyLib;
using LabApi.Features.Wrappers;
using MelonLoader;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp096;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.PlayableScps.Scp939;
using System.Text.Json;
using UnityEngine;

[assembly: MelonInfo(typeof(SCP_ULEPSZATOR.Core), "SCP_ULEPSZATOR", "1.0.0", "BadWaterGames", null)]
[assembly: MelonGame("Northwood", "SCPSL")]

namespace SCP_ULEPSZATOR;

public class Core : MelonMod
{
    private static readonly string configPath = Application.persistentDataPath + "/ulepszator-config.json";

    private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

    internal static UlepszatorConfig config = new();

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg($"Initialized SCP ULEPSZATOR at ({DateTime.Now:T}).");


        // Load/rebuilt config
        LoadConfig();

        // Patch
        HarmonyInstance.PatchAll();
    }


    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LoggerInstance.Msg($"Initialized {buildIndex}, {sceneName}");


        // Special chaos & mtf classes spawn chances
        if (buildIndex == 2) // Facility
        {
            if (config.MtfCapitanSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.CaptainsPercentage = config.MtfCapitanSpawnChance;

            if (config.MtfSergantSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.SergeantsPercentage = config.MtfSergantSpawnChance;

            if (config.ChaosLogicerSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.LogicerPercent = config.ChaosLogicerSpawnChance;

            if (config.ChaosShotgunSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.ShotgunPercent = config.ChaosShotgunSpawnChance;

            // Respawn tokens

            if (config.MtfRespawnTokens >= 0)
                RespawnWaves.PrimaryMtfWave.RespawnTokens = config.MtfRespawnTokens;

            if (config.ChaosRespawnTokens >= 0)
                RespawnWaves.PrimaryChaosWave.RespawnTokens = config.ChaosRespawnTokens;
        }

    }

    private void LoadConfig()
    {
        try
        {
            if (File.Exists(configPath))
            {
                var file = File.ReadAllText(configPath);
                config = JsonSerializer.Deserialize<UlepszatorConfig>(file) ?? new UlepszatorConfig();
                LoggerInstance.Msg($"Config read from ({configPath}).");
            }
            else
            {
                LoggerInstance.Msg($"Rebuilt config at ({configPath}).");
                File.WriteAllText(configPath, JsonSerializer.Serialize(config, jsonSerializerOptions));
            }
        }
        catch(Exception ex)
        {
            LoggerInstance.Msg($"Cannot load config: " + ex.Message);
        }
        finally
        {
            LoggerInstance.Msg($"Loaded config at ({configPath}).");
        }
        
    }

}

#region ScpPatches

[HarmonyPatch(typeof(Scp049Role), nameof(Scp049Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class PlagaPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.PlagaSpawnChace >= 0)
        {
            MelonLogger.Msg("Walić plage (choc i tak cie kocham slodziaku)");

            __result = Core.config.PlagaSpawnChace;
        }
        return false;
    }
}

[HarmonyPatch(typeof(Scp096Role), nameof(Scp096Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class NiesmialekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.SzarekSpawnChace >= 0)
        {
            MelonLogger.Msg("Dewaj mi tego szarka kuzwa (4 fucking pixels guy)");

            __result = Core.config.SzarekSpawnChace;
        }
        return false;

    }

}

[HarmonyPatch(typeof(Scp939Role), nameof(Scp939Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class JaszczurPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.JaszczurSpawnChace >= 0)
        {
            MelonLogger.Msg("Walić jaszczur (choc i tak cie kocham slodziaku)");

            __result = Core.config.JaszczurSpawnChace;
        }
        return false;

    }
}

[HarmonyPatch(typeof(Scp106Role), nameof(Scp106Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class DziadekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.DziadekSpawnChace >= 0)
        {
            MelonLogger.Msg("Walić dziadka (ciebie akurat niecierpie)");

            __result = Core.config.DziadekSpawnChace;
        }
        return false;

    }
}

[HarmonyPatch(typeof(Scp173Role), nameof(Scp173Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class OrzeszekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.OrzechSpawnChace >= 0)
        {
            MelonLogger.Msg("Walić orzeszka (ciebie akurat niecierpie)");

            __result = Core.config.OrzechSpawnChace;
        }
        return false;


    }
}

[HarmonyPatch(typeof(Scp079Role), nameof(Scp079Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class KomputerekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.config.KomputerekSpawnChace >= 0)
        {
            MelonLogger.Msg("Walić orzeszka (ciebie akurat niecierpie)");

            __result = Core.config.KomputerekSpawnChace;
        }
        return false;


    }
}
#endregion


//[HarmonyPatch(typeof(RespawnTokensManager), "get_Milestones")]
//public static class MilestonesPatch
//{
//    private static bool Prefix(ref Dictionary<Faction, List<Milestone>> __result)
//    {
//        MelonLogger.Msg($"Zminone milestony (Fundacja: {string.Join(", ", RespawnTokensManager.Milestones[Faction.FoundationStaff])}, Choas: {string.Join(", ", RespawnTokensManager.Milestones[Faction.FoundationEnemy])})");

//        __result = new Dictionary<Faction, List<Milestone>>
//        {
//            [Faction.FoundationStaff] = Core.config.FundacjaMilestones,
//            [Faction.FoundationEnemy] = Core.config.ChaosMilestones
//        };

//        return false;
//    }
//}