using LabApi.Events.Handlers;
using LabApi.Features.Wrappers;
using MapGeneration;
using MEC;
using MelonLoader;
using MelonLoader.TinyJSON;
using PlayerRoles;
using Respawning;
using Steamworks;
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

    

    public override void OnSceneWasLoaded(int buildIndex, string sceneName)
    {
        base.OnSceneWasLoaded(buildIndex, sceneName);
        LogAll($"Loaded scene: (index: {buildIndex}; name: {sceneName})");


        if (!LoadedPlugin)
        { 
            ulepszatorPlugin.Enable(); 
            LoadedPlugin = true;
        } 
        

        // Special chaos & mtf classes spawn chances
        if (buildIndex == FacilitySceneIndex) // Facility
        {
            if (Config.MtfCapitanSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.CaptainsPercentage = Config.MtfCapitanSpawnChance;

            if (Config.MtfSergantSpawnChance >= 0)
                RespawnWaves.PrimaryMtfWave.SergeantsPercentage = Config.MtfSergantSpawnChance;

            if (Config.ChaosLogicerSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.LogicerPercent = Config.ChaosLogicerSpawnChance;

            if (Config.ChaosShotgunSpawnChance >= 0)
                RespawnWaves.PrimaryChaosWave.ShotgunPercent = Config.ChaosShotgunSpawnChance;


            if(Config.MiniWaveSpecjalist)
            {
                RespawnWaves.MiniMtfWave.DefaultRole = RoleTypeId.NtfSpecialist;
                RespawnWaves.MiniChaosWave.DefaultRole = RoleTypeId.ChaosMarauder;
            }

            
            PlayerEvents.Kicking += (e) =>
            {
                LogAll($"Server is kicking: {e.Player.DisplayName} for {e.Reason}; Issuer: {e.Issuer.DisplayName}; ");
            };

                
            PlayerEvents.Spawned += (e) =>
            {
                Timing.CallDelayed(2, () => e?.Player?.Scale = Vector3.one);                
            };

            // TODO: 
            PlayerEvents.FlippedCoin += (e) => 
            {
                if (!Config.CoinFlipCustomMechanic)
                    return;


                Vector3 targetSize = e.Player.Scale;
                
                
                if(e.IsTails || e.Player.MaxHealth < 100)
                {
                    targetSize = targetSize.ModifyVector3(v => v * Config.KrasnalScaleFactor);

                    if (e.Player.MaxHealth - Config.KrasnalHealthRemove < 100 - Config.KrasnalHealthRemove * Config.MaxFlips)
                    {
                        LogAll($"{e.Player.DisplayName} jest juz wystarczajoco krasnalem");
                        return;
                    }

                   
                    e.Player.MaxHealth -= Config.KrasnalHealthRemove;


                    if (e.Player.MaxHealth <= 0)
                        e.Player.Kill("Coś ty najlepszego odjaniepawlił", $"{e.Player.DisplayName} fliped a coin one");

                    e.Player.GiveRandomCandy();
                    e.Player.Emotion = PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.EmotionPresetType.Ogre;
                }
                else if(e.Player.MaxHealth > 99)
                {
                    targetSize = targetSize.ModifyVector3(v => v * Config.GigantScaleFactor);

                    if (e.Player.MaxHealth + Config.GigantHealthAdd > 100 + Config.GigantHealthAdd * Config.MaxFlips)
                    {
                        LogAll($"{e.Player.DisplayName} jest juz wystarczajoco gigantem");
                        return;
                    }


                    e.Player.MaxHealth += Config.GigantHealthAdd;
                    e.Player.Emotion = PlayerRoles.FirstPersonControl.Thirdperson.Subcontrollers.EmotionPresetType.AwkwardSmile;
                }
                // TODO: Dodać kary


                e.Player.Scale = targetSize;
            };

            ServerEvents.AchievedMilestone += (ev) =>
            {
                LogAll(
                   $"{ev.Faction} faction reached {ev.MilestoneIndex} milestone ({ev.Threshold} - threshold) from [{string.Join(",", RespawnTokensManager.Milestones[ev.Faction].Select(m => m.Threshold))}]. \n" +
                   $"Now chaos now has {RespawnWaves.PrimaryChaosWave.RespawnTokens} tokens, {RespawnWaves.PrimaryChaosWave.Influence} influence & fundation now has {RespawnWaves.PrimaryMtfWave.RespawnTokens} tokens, {RespawnWaves.PrimaryMtfWave.Influence} influence");
            };

            
            if(Config.GeneratorActivationWarheadMechanic)
                ObjectiveEvents.ActivatedGeneratorCompleted += (e) => {

                

                    int count = Generator.List.Count(g => g.Engaged);
                    Warhead.DetonationTime /= count;
               
                    LogAll("Warhead detonation time: " + Warhead.DetonationTime);

                    if(count > 2)
                    {
                        Warhead.LeverStatus = true;
                        Warhead.IsLocked = true;

                        Announcer.Clear();
                        Announcer.GlitchyMessage(Core.WarheadDetonationMessage, 0.3f, 0.4f);

                    }
                };

            PlayerEvents.UnlockingWarheadButton += (e) =>
            {
                e.IsAllowed = (Generator.List.Count(g => g.Engaged) > 2 && Config.GeneratorActivationWarheadMechanic) || (Config.WarheadButtonPermissions > 0 && e.Player.CurrentItem.Category == ItemCategory.Keycard && ((KeycardItem) e.Player.CurrentItem).Permissions == Config.WarheadButtonPermissions);
                if(e.IsAllowed)
                {
                    Announcer.Clear();
                    Announcer.GlitchyMessage("WARHEAD .G3 READY TO DESTRUCT DETONATION", 0.3f, 0.4f);
                }
            };

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
            LogAll($"Config successfuly loaded from: ({ConfigPath}).");
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

}

