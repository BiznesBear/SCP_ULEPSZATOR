using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MEC;
using MelonLoader;
using PlayerRoles;
using PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers;
using Respawning;
using System.Net;
using System.Text.Json;
using UnityEngine;



[assembly: MelonInfo(typeof(SCP_ULEPSZATOR.Core), "SCP_ULEPSZATOR", "1.0.0", "BadWaterGames", null)]
[assembly: MelonGame("Northwood", "SCPSL")]

namespace SCP_ULEPSZATOR;

public class Core : MelonMod
{
    public const string WarheadDetonationMessage = "Generator over load .G2 .G3 .G7 .G6 .G5 .G5 ALL GENERATORS .G5 .G3 .G6 .G6 .G6 warhead activated and .G3 .G6 .G5 locked .G6 .G5 .G2 ALL PERSON CAN DETONATION .G3 .G3";

   
    public static Core Instance { get; private set; }
    public string ConfigPath { get; private set; }
    public UlepszatorConfig Config { get; private set; } = new();
    public UlepszatorPlugin UlepszatorPlugin { get; private set; }

    public bool LoadedPlugin { get; private set; }

    public static readonly JsonSerializerOptions jsonSerializerOptions = new() { 
        WriteIndented = true,
        Converters = { new JsonConverterFloat() }  
    };
    
    
    private const int FacilitySceneIndex = 2; 


    public override void OnInitializeMelon()
    {
        Instance = this;
        LogAll($"Initialized SCP ULEPSZATOR at {MelonAssembly.Location}.");

        ConfigPath = Path.GetDirectoryName(MelonAssembly.Location) + "\\ulepszator-Config.json";
        LogAll($"Config location is set to: {ConfigPath}.");
        
        // Load/rebuilt Config
        LoadConfig();

        // Patch
        HarmonyInstance.PatchAll();
    }

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LogAll($"Loaded scene: (index: {buildIndex}; name: {sceneName})");


        if (!LoadedPlugin)
        {
            UlepszatorPlugin ??= new();

            UlepszatorPlugin.Enable();
            RegisterEvents();

            LoadedPlugin = true;
        }

        if(buildIndex == FacilitySceneIndex)
        {
            // Load all necesery things for scene here 
            ApplyValues();
        }
        
    }


    public void ApplyValues()
    {
        LogAll("Applying custom values...");

        if (Config.MtfCapitanSpawnChance >= 0)
            RespawnWaves.PrimaryMtfWave.CaptainsPercentage = Config.MtfCapitanSpawnChance;

        if (Config.MtfSergantSpawnChance >= 0)
            RespawnWaves.PrimaryMtfWave.SergeantsPercentage = Config.MtfSergantSpawnChance;

        if (Config.ChaosLogicerSpawnChance >= 0)
            RespawnWaves.PrimaryChaosWave.LogicerPercent = Config.ChaosLogicerSpawnChance;

        if (Config.ChaosShotgunSpawnChance >= 0)
            RespawnWaves.PrimaryChaosWave.ShotgunPercent = Config.ChaosShotgunSpawnChance;


        if (Config.MiniWaveSpecjalist)
        {
            RespawnWaves.MiniMtfWave.DefaultRole = RoleTypeId.NtfSpecialist;
            RespawnWaves.MiniChaosWave.DefaultRole = RoleTypeId.ChaosMarauder;
        }

    }

    public void ApplyModes()
    {
        LogAll("Applying custom modes...");
        LightsOutMode(Config.LightsOutMode);
    }

    public void ApplyCustomLights()
    {
        if (!Config.CustomColors)
            return;

        if (Config.RandomRoomColors)
        {
            foreach (var l in LightsController.List)
                l.OverrideLightsColor = new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));
        }
        else if (Config.CustomRoomColors.Count > 1)
        {
            foreach (var l in LightsController.List)
                l.OverrideLightsColor = Config.GetRoomColors()[UnityEngine.Random.Range(0, Config.CustomRoomColors.Count)];
        }
    }


    public void Reload()
    {
        LoadConfig();
        
        ApplyValues();
        ApplyModes();
        ApplyCustomLights();
    }


    public void RegisterEvents()
    {
        LogAll("Registering events...");
        PlayerEvents.Kicking += (e) =>
        {
            LogAll($"Server is kicking: {e.Player.DisplayName} for {e.Reason}; Issuer: {e.Issuer.DisplayName}; ");
        };
        

        PlayerEvents.Spawned += (e) =>
        {
            // ahhh. odwleczanie w czasie czegoś co pewnie można zrobić asynchronicznie albo normalnie jak człowiek.
            // ale nie, bo po co skoro można iść drogą inteligenta i po prostu dodać delay =))))
            Timing.CallDelayed(1, () => {
                e.Player?.Scale = Vector3.one;

                if (Config.LightsOutMode)
                    e.Player.GivePlayerIfNeeded(ItemType.Flashlight);
                    
            });
        };


        ServerEvents.WaveRespawning += (_) =>
        {
            ApplyValues();
        };


        ServerEvents.RoundStarted += () =>
        {
            ApplyCustomLights();
            Timing.CallDelayed(Config.CustomModesApplyDelay, ApplyModes);
        };


        PlayerEvents.FlippedCoin += (e) =>
        {
            if (!Config.CoinFlipCustomMechanic)
                return;

            
            if (e.IsTails)
            {
                if (KrasnalizePlayer(e.Player))
                    LogAll("Successfullly Krasnalized " + e.Player.DisplayName);
                else
                    LogAll($"{e.Player.DisplayName} jest juz wystarczajoco krasnalem");

            }
            else
            {
                if(GigantizePlayer(e.Player))
                    LogAll("Successfullly Gigantized " + e.Player.DisplayName);
                else
                    LogAll($"{e.Player.DisplayName} jest juz wystarczajoco gigantem");

            }
        };


        ServerEvents.AchievedMilestone += (ev) =>
        {
            LogAll(
               $"{ev.Faction} faction reached {ev.MilestoneIndex} milestone ({ev.Threshold} - threshold) from [{string.Join(",", RespawnTokensManager.Milestones[ev.Faction].Select(m => m.Threshold))}]. \n" +
               $"Now chaos now has {RespawnWaves.PrimaryChaosWave.RespawnTokens} tokens, {RespawnWaves.PrimaryChaosWave.Influence} influence & fundation now has {RespawnWaves.PrimaryMtfWave.RespawnTokens} tokens, {RespawnWaves.PrimaryMtfWave.Influence} influence");
            LogAll($"Fundation: {RespawnTokensManager.TryGetNextThreshold(Faction.FoundationStaff, FactionInfluenceManager.Get(Faction.FoundationStaff), out var threshold)} / {threshold}");
            LogAll($"Chaos: {RespawnTokensManager.TryGetNextThreshold(Faction.FoundationEnemy, FactionInfluenceManager.Get(Faction.FoundationEnemy), out var t)} / {t}");
            
        };

        Scp079Events.BlackingOutRoom += (e) =>
        {
            if (Config.LightsOutMode)
                e.IsAllowed = false;
        };

        
        Scp049Events.StartingResurrection += (e) =>
        {
            if (Config.MaxSCP049_2Instances < 0)
                return;


            if(Player.List.Count(p => p.Role == RoleTypeId.Scp0492) >= Config.MaxSCP049_2Instances || (Config.BlockSCP049_2Resurection && e.Ragdoll.Role == RoleTypeId.Scp0492))
                e.CanResurrect = false;
        };

        ObjectiveEvents.ActivatedGeneratorCompleted += (e) => {

            int count = Generator.List.Count(g => g.Engaged);

            if (Config.LightsOutMode) // tood: dodać zwiększanie natęzenia swiatła w placówce z każdą aktywacją
            {
                Map.TurnOnLights();

                if (count < 3)
                    Timing.CallDelayed(Config.GeneratorElectricityPower, Map.TurnOffLights);
            }

            if (Config.GeneratorActivateWarheadMechanic)
                return;

            if (count > 2)
            {
                Warhead.LeverStatus = true;
                Warhead.IsLocked = true;

                Announcer.Clear();
                Announcer.GlitchyMessage(WarheadDetonationMessage, 0.3f, 0.4f);
            }
        };

        PlayerEvents.InteractedWarheadLever += (e) =>
        {
            if (!e.Enabled || !Config.WarheadMessages)
                return;

            static void AnnounceWarheadReady() => Announcer.GlitchyMessage("WARHEAD WARHEAD WARHEAD WARHEAD.G3 READY READY READY READY", 0.3f, 0.4f);

            if (Announcer.IsSpeaking)
                Timing.CallDelayed(10, AnnounceWarheadReady);
            else
                AnnounceWarheadReady();
        };

        PlayerEvents.UnlockingWarheadButton += (e) =>
        {
            e.IsAllowed =  (e.IsAllowed || Generator.List.Count(g => g.Engaged) > 2 && Config.GeneratorActivateWarheadMechanic) || (Config.WarheadButtonPermissions > 0 && e.Player.CurrentItem.Category == ItemCategory.Keycard && ((KeycardItem)e.Player.CurrentItem).Permissions == Config.WarheadButtonPermissions);
            
            if (e.IsAllowed && Config.WarheadMessages)
            {
                static void AnnounceWarheadReady() => Announcer.GlitchyMessage("WARHEAD .G3 READY TO DESTRUCT DETONATION", 0.3f, 0.4f);
                
                if (Announcer.IsSpeaking)
                    Timing.CallDelayed(10, AnnounceWarheadReady);
                else
                    AnnounceWarheadReady();
            }
        };

    }

    public void LoadConfig()
    {
        try
        {
            if (File.Exists(ConfigPath))
            {
                var file = File.ReadAllText(ConfigPath);
                Config = JsonSerializer.Deserialize<UlepszatorConfig>(file) ?? new UlepszatorConfig();
                LogAll($"Config read from: ({ConfigPath}).");
            }
            else
            {
                RebuiltConfig();
            }
        }
        catch(Exception ex)
        {
            LogAll($"Cannot load Config: " + ex.Message);
        }
        finally
        {
            LogAll($"Config successfuly loaded...");
        }
    }

    public void RebuiltConfig()
    {
        LogAll($"Rebuilt Config at location: ({ConfigPath}).");
        File.WriteAllText(ConfigPath, JsonSerializer.Serialize(Config, jsonSerializerOptions));
    }

    public void ResetConfig() => Config = new();


    /// <summary>
    /// Logs to melon loader and to server console
    /// </summary>
    internal void LogAll(string text)
    {
        LoggerInstance.Msg(text);
        ServerConsole.AddLog(text, System.ConsoleColor.Magenta);
    }

    public void LightsOutMode(bool on)
    {
        if(on)
        {
            LogAll("Lights out mode activated");
            GiveAllPlayersIfNeeded(ItemType.Flashlight);
   
            Map.TurnOffLights();
        }
        else
        {
            LogAll("Lights out mode disabled");
            Map.TurnOnLights();
        }
    }

    public void GiveAllPlayersIfNeeded(ItemType type)
    {
        foreach (var p in Player.List)
            p.GivePlayerIfNeeded(type);
    } 

    public bool KrasnalizePlayer(Player player)
    {
        if ((player.MaxHealth - Config.KrasnalHealthRemove < 100 - Config.KrasnalHealthRemove * Config.MaxFlips) || player.MaxHealth > 100)
            return false;


        player.Scale = player.Scale.ModifyVector3(v => v * Config.KrasnalScaleFactor);
        player.MaxHealth -= Config.KrasnalHealthRemove;


        if (player.MaxHealth <= 0)
            player.Kill("Coś ty najlepszego odjaniepawlił", $"{player.DisplayName} fliped AnnounceWarheadReady coin one");

        player.GiveRandomCandy();
        // TODO: dodac losowanie emotki
        player.Emotion = EmotionPresetType.Ogre;
        return true;
    }

    public bool GigantizePlayer(Player player)
    {
        if ((player.MaxHealth + Config.GigantHealthAdd > 100 + Config.GigantHealthAdd * Config.MaxFlips) || player.MaxHealth < 100)
            return false;


        player.Scale = player.Scale.ModifyVector3(v => v * Config.GigantScaleFactor);
        player.MaxHealth += Config.GigantHealthAdd;
        player.Emotion = EmotionPresetType.AwkwardSmile;
        return true;
    }

    
}
