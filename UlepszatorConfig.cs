using Interactables.Interobjects.DoorUtils;
using UnityEngine;
namespace SCP_ULEPSZATOR;


public class UlepszatorConfig
{
    // -----
    // SCP Spawn chances
    // -----

    public float PlagaSpawnChace { get; set; } = 1f;
    public float OrzechSpawnChace { get; set; } = 1f;
    public float JaszczurSpawnChace { get; set; } = 1f;
    public float DziadekSpawnChace { get; set; } = 1f;
    public float SzarekSpawnChace { get; set; } = -1f;
    public float KomputerekSpawnChace { get; set; } = -1f;
    public float SzkieletorSpawnChace { get; set; } = -1f;

    public bool ForceSecondarySCPSupportRole { get; set; } = true;


    // -----
    // Influence milestones
    // -----
    public int[] FundacjaTokeny { get; set; } = [5, 15, 30, 40];
    public int[] ChaosTokeny { get; set; } = [5, 15, 30, 40];



    // -----
    // MTF and Chaos special roles spawn chances
    // -----
    public float MtfCapitanSpawnChance { get; set; } = 0.2f;
    public float MtfSergantSpawnChance { get; set; } = 0.3f;


    public float ChaosLogicerSpawnChance { get; set; } = 0.2f;
    public float ChaosShotgunSpawnChance { get; set; } = 0.3f;
    public bool MiniWaveSpecjalist { get; set; } = true;

    // -----
    // Coin flip
    // -----
    public bool CoinFlipCustomMechanic { get; set; } = true;
    public float KrasnalScaleFactor { get; set; } = 0.7f;
    public float KrasnalHealthRemove { get; set; } = 30;
    public float GigantScaleFactor { get; set; } = 1.1f;
    public float GigantHealthAdd { get; set; } = 50;
    public int MaxFlips { get; set; } = 2;



    // -----
    // Warhead
    // -----
    public DoorPermissionFlags WarheadButtonPermissions { get; set; } = DoorPermissionFlags.ContainmentLevelOne; // 0 - defalut; ALL: 65535. NORMAL: 64. JANITOR: 16
    public bool GeneratorActivateWarheadMechanic { get; set; } = true;


    // -----
    // Custom modes
    // -----
    public int CustomModesApplyDelay { get; set; } = 30;
    public bool LightsOutMode { get; set; } = false;
    public int GeneratorElectricityPower { get; set; } = 90;



    // -----
    // Jeżeli to czytasz, to znaczy, że jednak lubisz tego cwela plage 049... Dobrze więc... ALE NA MOICH ZASADACH!!!
    // -----
    public int MaxSCP049_2Instances { get; set; } = 1;
    public bool BlockSCP049_2Resurection { get; set; } = true;


    // -----
    // Custom room colors
    // -----
    public bool CustomColors { get; set; } = true;
    public bool RandomRoomColors { get; set; } = false;
    public List<string> CustomRoomColors { get; set; } = [
        Color.magenta.GetHexString(),
        Color.blue.GetHexString(),
        Color.green.GetHexString(),
        Color.red.GetHexString(),
        Color.yellow.GetHexString(),
        Color.cyan.GetHexString(),
        Color.white.GetHexString(),
    ];

    public IReadOnlyList<Color> GetRoomColors() => CustomRoomColors.ConvertAll<Color>(x => x.GetHexColor());

}
