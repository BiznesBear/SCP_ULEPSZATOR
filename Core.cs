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
using Respawning;
using System.Text.Json;

[assembly: MelonInfo(typeof(SCP_ULEPSZATOR.Core), "SCP_ULEPSZATOR", "1.0.0", "BadWaterGames", null)]
[assembly: MelonGame("Northwood", "SCPSL")]

namespace SCP_ULEPSZATOR;

public class Core : MelonMod
{
    private string ConfigPath { get; set; }



    private static readonly JsonSerializerOptions jsonSerializerOptions = new() { WriteIndented = true };

    private const int FacilityScene = 2; 

    internal static UlepszatorConfig config = new();

    public override void OnInitializeMelon()
    {
        LoggerInstance.Msg($"Initialized SCP ULEPSZATOR at {MelonAssembly.Location}.");

        ConfigPath = Path.GetDirectoryName(MelonAssembly.Location) + "\\ulepszator-config.json";
        LoggerInstance.Msg($"Config location is set to: {ConfigPath}.");
        
        // Load/rebuilt config
        LoadConfig();

        // Patch
        HarmonyInstance.PatchAll();
        
    }


    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LoggerInstance.Msg($"Loaded scene: (index: {buildIndex}; name: {sceneName})");


        // Special chaos & mtf classes spawn chances
        if (buildIndex == FacilityScene) // Facility
        {
            if (config.MtfCapitanSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.CaptainsPercentage = config.MtfCapitanSpawnChance;

            if (config.MtfSergantSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.SergeantsPercentage = config.MtfSergantSpawnChance;

            if (config.ChaosLogicerSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.LogicerPercent = config.ChaosLogicerSpawnChance;

            if (config.ChaosShotgunSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.ShotgunPercent = config.ChaosShotgunSpawnChance;


            if(config.MiniWaveSpecjalist)
            {
                RespawnWaves.MiniMtfWave.DefaultRole = RoleTypeId.NtfSpecialist;
                RespawnWaves.MiniChaosWave.DefaultRole = RoleTypeId.ChaosMarauder;
            }


            LabApi.Events.Handlers.ServerEvents.AchievedMilestone += ServerEvents_AchievedMilestone;
        }

    }

    private void ServerEvents_AchievedMilestone(LabApi.Events.Arguments.ServerEvents.AchievedMilestoneEventArgs ev)
    {
        LoggerInstance.Msg(
            $"{ev.Faction} faction reached {ev.MilestoneIndex} milestone ({ev.Threshold} - threshold) from [{string.Join(",", RespawnTokensManager.Milestones[ev.Faction].Select(m => m.Threshold))}]. \n" +
            $"Now chaos now has {RespawnWaves.PrimaryChaosWave.RespawnTokens} tokens, {RespawnWaves.PrimaryChaosWave.Influence} influence & fundation now has {RespawnWaves.PrimaryMtfWave.RespawnTokens} tokens, {RespawnWaves.PrimaryMtfWave.Influence} influence");
    }



    private void LoadConfig()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var file = File.ReadAllText(ConfigPath);
                config = JsonSerializer.Deserialize<UlepszatorConfig>(file) ?? new UlepszatorConfig();
                LoggerInstance.Msg($"Config read from: ({ConfigPath}).");
            }
            else
            {
                LoggerInstance.Msg($"Rebuilt config at location: ({ConfigPath}).");
                File.WriteAllText(ConfigPath, JsonSerializer.Serialize(config, jsonSerializerOptions));
            }
        }
        catch(Exception ex)
        {
            LoggerInstance.Msg($"Cannot load config: " + ex.Message);
        }
        finally
        {
            LoggerInstance.Msg($"Config successfuly loaded from: ({ConfigPath}).");
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
            MelonLogger.Msg("Walić komputerka (ciebie nawet nie znam xd)");

            __result = Core.config.KomputerekSpawnChace;
        }
        return false;

    }
}
#endregion


[HarmonyPatch(typeof(RespawnTokensManager), nameof(RespawnTokensManager.Milestones), MethodType.Getter)]
public static class MilestonesPatch
{
    // TODO: Naprawić UI, jeżeli możliwe
    private static void Postfix(ref Dictionary<Faction, List<RespawnTokensManager.Milestone>> __result)
    {
        __result[Faction.FoundationStaff] = Core.config.GetMilestones(Core.config.FundacjaTokeny);
        __result[Faction.FoundationEnemy] = Core.config.GetMilestones(Core.config.ChaosTokeny);
    }
}