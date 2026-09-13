using LabApi.Loader.Features.Plugins;
using LabApi.Features.Wrappers;

namespace SCP_ULEPSZATOR;

internal class UlepszatorPlugin : Plugin
{
    public override string Name => "SCP_ULEPSZATOR";

    public override string Description => "Mikołaj władcą świata";

    public override string Author => "My";

    public override Version RequiredApiVersion => Version;


    public override void Enable()
    {
        if (Core.config.MtfCapitanSpawnChance >= 0)
            RespawnWaves.PrimaryMtfWave.CaptainsPercentage = Core.config.MtfCapitanSpawnChance;

        if (Core.config.MtfSergantSpawnChance >= 0)
            RespawnWaves.PrimaryMtfWave.SergeantsPercentage = Core.config.MtfSergantSpawnChance;

        if (Core.config.ChaosLogicerSpawnChance >= 0)
            RespawnWaves.PrimaryChaosWave.LogicerPercent = Core.config.ChaosLogicerSpawnChance;

        if (Core.config.ChaosShotgunSpawnChance >= 0)
            RespawnWaves.PrimaryChaosWave.ShotgunPercent = Core.config.ChaosShotgunSpawnChance;

        // Respawn tokens

        if (Core.config.ChaosRespawnTokens >= 0)
            RespawnWaves.PrimaryChaosWave.RespawnTokens = Core.config.ChaosRespawnTokens;

        if (Core.config.ChaosRespawnTokens >= 0)
            RespawnWaves.PrimaryChaosWave.RespawnTokens = Core.config.ChaosRespawnTokens;
    }
    public override void Disable()
    {

    }
}
