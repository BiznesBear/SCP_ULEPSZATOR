using PlayerRoles;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.PlayableScps.Scp079;
using PlayerRoles.PlayableScps.Scp096;
using PlayerRoles.PlayableScps.Scp106;
using PlayerRoles.PlayableScps.Scp173;
using PlayerRoles.PlayableScps.Scp939;
using PlayerRoles.PlayableScps.Scp3114;
using Respawning;
using HarmonyLib;

namespace SCP_ULEPSZATOR;

#region ScpPatches

[HarmonyPatch(typeof(Scp049Role), nameof(Scp049Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class PlagaPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.PlagaSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić plage (choc i tak cie kocham slodziaku)");

            __result = Core.Instance.Config.PlagaSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy plagi");

        return true;
    }
}

[HarmonyPatch(typeof(Scp096Role), nameof(Scp096Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class NiesmialekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.SzarekSpawnChace >= 0)
        {
            Core.Instance.LogAll("Dewaj mi tego szarka kuzwa (4 fucking pixels guy)");

            __result = Core.Instance.Config.SzarekSpawnChace;
            return false;

        }
        else Core.Instance.LogAll("Niewalimy niesmialeka");

        return true;
    }

}

[HarmonyPatch(typeof(Scp939Role), nameof(Scp939Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class JaszczurPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.JaszczurSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić jaszczur (choc i tak cie kocham slodziaku)");

            __result = Core.Instance.Config.JaszczurSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy jaszczur");


        return true;

    }
}

[HarmonyPatch(typeof(Scp106Role), nameof(Scp106Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class DziadekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.DziadekSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić dziadka (ciebie akurat niecierpie)");

            __result = Core.Instance.Config.DziadekSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy dziadka");


        return true;

    }
}

[HarmonyPatch(typeof(Scp173Role), nameof(Scp173Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class OrzeszekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.OrzechSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić orzeszka (ciebie akurat niecierpie)");

            __result = Core.Instance.Config.OrzechSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy orzeszka");

        return true;

    }
}

[HarmonyPatch(typeof(Scp079Role), nameof(Scp079Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class KomputerekPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.KomputerekSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić komputerka (ciebie nawet nie znam xd)");

            __result = Core.Instance.Config.KomputerekSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy komputerka");

        return true;

    }

}


[HarmonyPatch(typeof(Scp3114Role), nameof(Scp3114Role.GetSpawnChance), [typeof(List<RoleTypeId>)])]
internal static class SzkieletorPatch
{
    private static bool Prefix(List<RoleTypeId> alreadySpawned, ref float __result)
    {
        if (Core.Instance.Config.SzkieletorSpawnChace >= 0)
        {
            Core.Instance.LogAll("Walić szielkietorka (ciebie nawet nie znam xd)");

            __result = Core.Instance.Config.SzkieletorSpawnChace;
            return false;
        }
        else Core.Instance.LogAll("Niewalimy szelkietorka");

        return true;
        
    }
}

#endregion


[HarmonyPatch(typeof(RespawnTokensManager), nameof(RespawnTokensManager.Milestones), MethodType.Getter)]
internal static class MilestonesPatch
{
    // TODO: Naprawić UI, jeżeli możliwe - 10.10.2026: pierdole to
    private static void Postfix(ref Dictionary<Faction, List<RespawnTokensManager.Milestone>> __result)
    {
        __result[Faction.FoundationStaff] = Core.Instance.Config.FundacjaTokeny.GetMilestones();
        __result[Faction.FoundationEnemy] = Core.Instance.Config.ChaosTokeny.GetMilestones();
    }
    
}