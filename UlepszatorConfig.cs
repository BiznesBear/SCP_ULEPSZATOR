using System.Text.Json.Serialization;
using static Respawning.RespawnTokensManager;
namespace SCP_ULEPSZATOR;


internal class UlepszatorConfig
{
    [JsonIgnore]
    public List<Milestone> FundacjaMilestones
    {
        get
        {
            var lista = new List<Milestone>();
            foreach (var token in FundacjaTokeny)
                lista.Add(new(token));
            return lista;
        }
    }

    [JsonIgnore]
    public List<Milestone> ChaosMilestones
    {
        get
        {
            var lista = new List<Milestone>();
            foreach (var token in ChaosTokeny)
                lista.Add(new(token));
            return lista;
        }
    }


    public float PlagaSpawnChace { get; set; } = 1f;
    public float OrzechSpawnChace { get; set; } = 1f;
    public float JaszczurSpawnChace { get; set; } = 1f;
    public float DziadekSpawnChace { get; set; } = 1f;
    public float SzarekSpawnChace { get; set; } = 1f;
    public float KomputerekSpawnChace { get; set; } = 1f;

    public int[] FundacjaTokeny { get; set; } = [10, 20, 30, 40];
    public int[] ChaosTokeny { get; set; } = [10, 20, 30, 40];
    
    
    public float MtfCapitanSpawnChance { get; set; } = 0.2f;
    public float MtfSergantSpawnChance { get; set; } = 0.3f;


    public float ChaosLogicerSpawnChance { get; set; } = 0.2f;
    public float ChaosShotgunSpawnChance { get; set; } = 0.3f;


    public int MtfRespawnTokens { get; set; } = -1;
    public int ChaosRespawnTokens { get; set; } = -1;
  
}
