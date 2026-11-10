# SCP_ULEPSZATOR
Najlepszy mod na gry do 10 osób w SCP SL bo mozna wylaczyc plage hahaha

# Budowa
Zkombiluj za pomocą dotneta 6.0. Musisz mieć zainstalowanego melon ładowacz (patrz instalacja) aby móc w projekcie dodać zależności serwera łescepe lab.
Ewentualnie zkorzyztaj z dostarczonego przezemnie pliku ktury NA PEWNO DZIAŁA do bezpośredniej instsalacji poniżej.

# Instalacja
1. Zainstaluj Melon mod loader zstond: https://github.com/lavagang/melonloader
2. Uruchom Melona na SCP:SL Dedicated Server
3. Dodaj do folderu mods SCP_ULEPSZATOR.dll i powinno działać. Jak jesteś na linuxie to miej pod uwage, że jesteś na linuxie i że linux windowsowi nie równy, a więc łap poradnik: https://melonwiki.xyz/#/?id=linux-launch-instructions


# Ficzery
1. Pozwala na wyłącznie i włączenie escepekuf
2. p0zwala na zmiane liczby tokenuf dla poszczeuglbnych ogranizacji
3. zezwala na zmiane szczescia spawnu specjalnych rul jednostek operacyjnych
4. dodaje nowom mechanike giganta i karla przy zucie monetom
5. ma fajnom custom comende `ulepszator` 
6. Zmiana koloru świateł w placówce

# Congif
Generalnie to wygloda tak:
```json
{
  "PlagaSpawnChace": 1,
  "OrzechSpawnChace": 1,
  "JaszczurSpawnChace": 1,
  "DziadekSpawnChace": 1,
  "SzarekSpawnChace": -1,
  "KomputerekSpawnChace": -1,
  "SzkieletorSpawnChace": -1,
  "FundacjaTokeny": [
    10,
    20,
    30,
    40
  ],
  "ChaosTokeny": [
    10,
    20,
    30,
    40
  ],
  "MtfCapitanSpawnChance": 0.2,
  "MtfSergantSpawnChance": 0.3,
  "ChaosLogicerSpawnChance": 0.2,
  "ChaosShotgunSpawnChance": 0.3,
  "MiniWaveSpecjalist": true,
  "CoinFlipCustomMechanic": true,
  "KrasnalScaleFactor": 0.7,
  "KrasnalHealthRemove": 30,
  "GigantScaleFactor": 1.1,
  "GigantHealthAdd": 50,
  "MaxFlips": 2,
  "WarheadButtonPermissions": 16,
  "GeneratorActivationWarheadMechanic": true,
  "LightsOutMode": false,
  "GeneratorElectricityPower": 90,
  "CustomModesApplyDelay": 30,
  "MaxSCP049_2Instances": 1,
  "CustomColors": true,
  "RandomRoomColors": false,
  "CustomRoomColors": [
    {
      "R": 255,
      "G": 0,
      "B": 255
    },
    {
      "R": 0,
      "G": 0,
      "B": 255
    },
    {
      "R": 0,
      "G": 255,
      "B": 0
    },
    {
      "R": 255,
      "G": 0,
      "B": 0
    },
    {
      "R": 255,
      "G": 235,
      "B": 4
    },
    {
      "R": 0,
      "G": 255,
      "B": 255
    },
    {
      "R": 255,
      "G": 255,
      "B": 255
    }
  ]
}
```

Jak dasz ziomek na coś minus jeden to bedom uzywane defultowe ustawienia Scp sekretny laboratorium.

# Komandosy
1. `reload` - przeładowuje konfig
2. `rebuilt` - zapisuje plik konfigu obecnie zaladowanym konfigiem
3. `reset` - resetuje konfig (WAŻNE! nie nadpisuje pliku)
4. `set [nazwa wlasciwosci] [wartosc]` - daje mozlwiowc dynamicznej zmiany wartosci w konfigu
5. `settings` - pokazuje konfig
6. `test` - testuje ruzne ficzery tego cacka 
    - `engage [1/2/3/all]` - aktuwuje generatory
    - `fastGenerators` - generatory aktuwujom sie w 5 sekund
    - `warheadMessage` - sprawia ze cassie sie zesra i powie cos ciekawego
7. `lightsOut [on/off]` - włącza/wyłącza tryb lightout podczas istnienia serwera


# Przyszłe rzeczy które som zaplanowane, ale nie dokońca wiadomo czy będą
- Customowe role tj. 
    - Sprzątacz (wysadza placówkę sprzątając tym samym ten cały syf), 
    - Impostor (zbuntowana zbuntowana klasa D, czyli taka która toi po stronie placówki), 
    - Technik (może robić ciekawe rzeczy z kartami i permisjami do drzwi), 
    - Posiadacz jąder - zrespawnowany maczetą dostaje 2 odibjające się kulki
- Poprawne wyświetlanie influenca w grze, bo by się przydało (jednakże nie jest to możlwie do naprawienia z poziomu serwera)
- Przeniesienie tego do full do LabApi od pónocnego drzewa
- Naprawa systemu przechowywania kolorów, bo ten jest beznadziejny
- Zangielszczenie wszystkich nazw, danie im małych liter
- Dodanie `ulepszator update` która pozwala na aktualizacje moda
- Spawnowanie tylko suppoortowych ruól jako 2 scp ✅
- reseructowanie tylko normalnych cial i nie slugusuw
- dodać trackowanie ile razy zabezpieczyło się danego scp 