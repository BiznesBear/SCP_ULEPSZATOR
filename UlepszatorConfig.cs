using static Respawning.RespawnTokensManager;
namespace SCP_ULEPSZATOR;


internal class UlepszatorConfig
{
    // SCP Spawn chances
    public float PlagaSpawnChace { get; set; } = 1f;
    public float OrzechSpawnChace { get; set; } = 1f;
    public float JaszczurSpawnChace { get; set; } = 1f;
    public float DziadekSpawnChace { get; set; } = 1f;
    public float SzarekSpawnChace { get; set; } = 1f;
    public float KomputerekSpawnChace { get; set; } = 1f;




    // Influence milestones
    public int[] FundacjaTokeny { get; set; } = [10, 20, 30, 40];
    public int[] ChaosTokeny { get; set; } = [10, 20, 30, 40];
    
    
    // MTF and Chaos special roles spawn chances
    public float MtfCapitanSpawnChance { get; set; } = 0.2f;
    public float MtfSergantSpawnChance { get; set; } = 0.3f;


    public float ChaosLogicerSpawnChance { get; set; } = 0.2f;
    public float ChaosShotgunSpawnChance { get; set; } = 0.3f;
    public bool MiniWaveSpecjalist { get; set; } = true;


    public List<Milestone> GetMilestones(int[] ints)
    {
        var lista = new List<Milestone>();
        foreach (var token in ChaosTokeny)
            lista.Add(new(token));
        return lista;
    } 

}
