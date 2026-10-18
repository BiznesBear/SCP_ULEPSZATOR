using static Respawning.RespawnTokensManager;
using Interactables.Interobjects.DoorUtils;
namespace SCP_ULEPSZATOR;


public class UlepszatorConfig
{
    // SCP Spawn chances
    public float PlagaSpawnChace { get; set; } = 1f;
    public float OrzechSpawnChace { get; set; } = 1f;
    public float JaszczurSpawnChace { get; set; } = 1f;
    public float DziadekSpawnChace { get; set; } = 1f;
    public float SzarekSpawnChace { get; set; } = -1f;
    public float KomputerekSpawnChace { get; set; } = -1f;
    public float SzkieletorSpawnChace { get; set; } = -1f;


    // Influence milestones
    public int[] FundacjaTokeny { get; set; } = [10, 20, 30, 40];
    public int[] ChaosTokeny { get; set; } = [10, 20, 30, 40];
    
    
    // MTF and Chaos special roles spawn chances
    public float MtfCapitanSpawnChance { get; set; } = 0.2f;
    public float MtfSergantSpawnChance { get; set; } = 0.3f;


    public float ChaosLogicerSpawnChance { get; set; } = 0.2f;
    public float ChaosShotgunSpawnChance { get; set; } = 0.3f;
    public bool MiniWaveSpecjalist { get; set; } = true;


    public bool CoinFlipCustomMechanic { get; set; } = true;
    public float KrasnalScaleFactor { get; set; } = 0.7f;
    public float KrasnalHealthRemove { get; set; } = 30;
    public float GigantScaleFactor { get; set; } = 1.1f;
    public float GigantHealthAdd { get; set; } = 50;
    public int MaxFlips { get; set; } = 2;


    public DoorPermissionFlags WarheadButtonPermissions { get; set; } = DoorPermissionFlags.ContainmentLevelOne; // 0 - defalut; ALL: 65535. NORMAL: 64. JANITOR: 16
    public bool GeneratorActivationWarheadMechanic { get; set; } = true;


    public List<Milestone> GetMilestones(int[] numbers)
    {
        var lista = new List<Milestone>();
        foreach (var token in numbers)
            lista.Add(new(token));
        return lista;
    } 

}
