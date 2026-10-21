using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MEC;
using MelonLoader;
using PlayerRoles;
using RemoteAdmin.Communication;
using Respawning;
using Respawning.Graphics;
using System.Text.Json;
using UnityEngine;



[assembly: MelonInfo(typeof(SCP_ULEPSZATOR.Core), "SCP_ULEPSZATOR", "1.0.0", "BadWaterGames", null)]
[assembly: MelonGame("Northwood", "SCPSL")]

namespace SCP_ULEPSZATOR;

public class Core : MelonMod
{
    public static Core Instance { get; private set; }
    public string ConfigPath { get; private set; }
    public  UlepszatorConfig Config { get; private set; } = new();
    public bool LoadedPlugin { get; private set; }

    public static readonly JsonSerializerOptions jsonSerializerOptions = new() { 
        WriteIndented = true,
        Converters = { new JsonConverterFloat() }  
    };
    
    private const int FacilitySceneIndex = 2; 
    private UlepszatorPlugin ulepszatorPlugin;

    public const string WarheadDetonationMessage = "Generator over load .G2 .G3 .G7 .G6 .G5 .G5 ALL GENERATORS .G5 .G3 .G6 .G6 .G6 warhead activated and .G3 .G6 .G5 locked .G6 .G5 .G2 ALL PERSON CAN DETONATION .G3 .G3";


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

        ulepszatorPlugin = new();        
    }

    // TODO: dodac kiedys opcje reloadowania moda by wyczyscic wszystkie eventy itp.

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LogAll($"Loaded scene: (index: {buildIndex}; name: {sceneName})");


        if (!LoadedPlugin)
        { 
            ulepszatorPlugin.Enable();
            RegisterEvents();

            LoadedPlugin = true;
        }


        // Special chaos & mtf classes spawn chances
        //if (buildIndex == FacilitySceneIndex) // Facility
        //{
        //    LogAll("Lights out mode is enabled: " + Config.LightsOutMode);
            
        //    ApplyValues();

        //    // TODO: Dodać kiedyś custom influence objectivy
        //    //FactionInfluenceManager.Objectives.Add(new Objective)


        //    // TODO: Zpatchować kiedyś te wyświetlane influence thresholdy
        //    //var influenceDisplays = GameObject.FindObjectsByType<InfluenceDisplay>(FindObjectsSortMode.None);
        //    //foreach(var display in influenceDisplays)
        //    //{
        //    //    display.
        //    //}
        //}

    }


    public void ApplyValues()
    {
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
        LightsOutMode(Config.LightsOutMode);
    }


    // TODO: Add function for unregistering this things

    public void RegisterEvents()
    {
        PlayerEvents.Kicking += (e) =>
        {
            LogAll($"Server is kicking: {e.Player.DisplayName} for {e.Reason}; Issuer: {e.Issuer.DisplayName}; ");
        };

        PlayerEvents.Spawned += (e) =>
        {
            // ahhh. odwleczanie w czasie czegoś co pewnie można zrobić asynchronicznie albo normalnie jak człowiek. ale nie, bo po co skoro można iść drogą inteligenta
            Timing.CallDelayed(1, () => {
                e?.Player?.Scale = Vector3.one;
                
                if (Config.LightsOutMode)
                    e?.Player?.AddItem(ItemType.Flashlight);
            });
        };

        ServerEvents.RoundStarted += () =>
        {
            ApplyValues();
            Timing.CallDelayed(Config.CustomModesApplyDelay, ApplyModes);

            if (!Config.CustomColorsIsLightsOutOnly || Config.LightsOutMode)
                ApplyCustomLights();
        };


        PlayerEvents.FlippedCoin += (e) =>
        {
            if (!Config.CoinFlipCustomMechanic)
                return;

            
            if (e.IsTails)
            {
                if (KrasnalizePlayer(e.Player))
                {
                    LogAll("Successfullly Krasnalized " + e.Player.DisplayName);
                }
            }
            else
            {
                if(GigantizePlayer(e.Player))
                {
                    LogAll("Successfullly Gigantized " + e.Player.DisplayName);
                }
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

        ObjectiveEvents.ActivatedGeneratorCompleted += (e) => {
            int count = Generator.List.Count(g => g.Engaged);

            if (Config.LightsOutMode)
            {
                Map.TurnOnLights();

                if (count < 3)
                    Timing.CallDelayed(Config.GeneratorElectricityPower, Map.TurnOffLights);
            }

            if (Config.GeneratorActivationWarheadMechanic)
                return;

            if (count > 2)
            {
                Warhead.LeverStatus = true;
                Warhead.IsLocked = true;

                Announcer.Clear();
                Announcer.GlitchyMessage(WarheadDetonationMessage, 0.3f, 0.4f);
            }
        };

        PlayerEvents.UnlockingWarheadButton += (e) =>
        {
            e.IsAllowed = (Generator.List.Count(g => g.Engaged) > 2 && Config.GeneratorActivationWarheadMechanic) || (Config.WarheadButtonPermissions > 0 && e.Player.CurrentItem.Category == ItemCategory.Keycard && ((KeycardItem)e.Player.CurrentItem).Permissions == Config.WarheadButtonPermissions);
            if (e.IsAllowed)
            {
                static void AnnounceWarheadReady() => Announcer.GlitchyMessage("WARHEAD .G3 READY TO DESTRUCT DETONATION", 0.3f, 0.4f);
                
                if (Announcer.IsSpeaking)
                    Timing.CallDelayed(10, AnnounceWarheadReady);
                else
                    AnnounceWarheadReady();
            }
            
        };

        WarheadEvents.Started += (e) =>
        {
            if (Generator.List.Count(g => g.Engaged) > 2)
                Warhead.DetonationTime /= 2;
        };
    }

    public void ApplyCustomLights()
    {
        if (Config.RandomRoomColors)
        {
            foreach (var l in Map.RoomLights)
                l.OverrideLightsColor = new(UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f), UnityEngine.Random.Range(0f, 1f));

        }
        else if (Config.CustomRoomColors.Count > 1)
        {
            foreach (var l in Map.RoomLights)
                l.OverrideLightsColor = Config.CustomRoomColors[UnityEngine.Random.Range(0, Config.CustomRoomColors.Count)].GetColor();
        }
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
            LogAll($"Config successfuly loaded from.");
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
            Map.ResetColorOfLights();
            Map.TurnOnLights();
        }
    }

    public void GiveAllPlayersIfNeeded(ItemType type)
    {
        foreach (var p in Player.List)
            GivePlayerIfNeeded(p, type);
    }

    public void GivePlayerIfNeeded(Player p, ItemType type)
    {
        if (p?.Items.Count(x => x.Type == type) > 0)
            return;

        LogAll($"Added item ({type}) to: {p.DisplayName} ");
        p?.AddItem(type);
    }

    public bool KrasnalizePlayer(Player player)
    {
        if ((player.MaxHealth - Config.KrasnalHealthRemove < 100 - Config.KrasnalHealthRemove * Config.MaxFlips) || player.MaxHealth > 100)
        {
            LogAll($"{player.DisplayName} jest juz wystarczajoco krasnalem");
            return false;
        }


        player.Scale = player.Scale.ModifyVector3(v => v * Config.KrasnalScaleFactor);
        player.MaxHealth -= Config.KrasnalHealthRemove;


        if (player.MaxHealth <= 0)
            player.Kill("Coś ty najlepszego odjaniepawlił", $"{player.DisplayName} fliped AnnounceWarheadReady coin one");

        player.GiveRandomCandy();
        player.Emotion = PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.EmotionPresetType.Ogre;
        return true;
    }

    public bool GigantizePlayer(Player player)
    {
        if ((player.MaxHealth + Config.GigantHealthAdd > 100 + Config.GigantHealthAdd * Config.MaxFlips) || player.MaxHealth < 100)
        {
            LogAll($"{player.DisplayName} jest juz wystarczajoco gigantem");
            return false;
        }


        player.Scale = player.Scale.ModifyVector3(v => v * Config.GigantScaleFactor);
        player.MaxHealth += Config.GigantHealthAdd;
        player.Emotion = PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.EmotionPresetType.AwkwardSmile;
        return true;
    }
}
