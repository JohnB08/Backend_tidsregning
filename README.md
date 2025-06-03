# Readme / Nyttige verktøy brukt.


Her har vi brukt vår sln fil for å bl.a. tillate INTELLISENSE for flere .NET prosjekter samtidig. <br>
Det er gjort ved at vi først genererte en sln i vår tomme folder via:

```bash
dotnet new sln
```

Det genererte en sln fil i vår tomme folder. Vi kan se for oss at en sln fil fungerer som en container og manager for alle prosjektene vi ønsker å lage i folderen vår. <br>
<br>
Vi genererte så tre prosjekter, og puttet de i en tilsvarende beskrivende mappe.<br>

Unit Test prosjekt via xUnit:

```bash
dotnet new xunit -o Backend_tidsregning.Tests
```

Et klassebibliotek som skal inneholde businesslogikken til prosjektet vårt:

```bash
dotnet new classlib -o Backend_tidsregning.Core
```

WebApiet som skal være mellomleddet mellom vår mongodb, og en potensiell front-end:

```bash
dotnet new webapi --use-controllers -o Backend_tidsregning.WebApi
```

Legg merke til at vi bruker -o flagget for å strukturere hvert prosjekt i sin egen folder i mappestrukturen. Det gjør det også lettere for oss å separere ut namespacet for hvert prosjekt. <br>

Vi kan nå legge hvert av disse prosjektene inn i vår sln fil, for å markere de som subprosjekter av hovedprosjektet vårt:

```bash
dotnet sln add ./Backend_tidsregning.WebApi

dotnet sln add ./Backend_tidsregning.Core

dotnet sln add ./Backend_tidsregning.Tests
```

Legg merke til vi trenger bare gi cli verktøyet en relativ path, så finner den første gyldige csproj fil i mappen, og knytter den til sln filen. 

Siden vi har et klassebibliotek som skal "consumes" av de andre prosjektene våre, kan det også være lurt å lage referanser til dette i de to andre prosjektene våre.

```bash
dotnet add ./Backend_tidsregning.WebApi reference ./Backend_tidsregning.Core

dotnet add ./Backend_tidsregning.Tests reference ./Backend_tidsregning.Core
```

Det betyr at vi tilgjengeliggjør alle pakker og namespaces fra Core i de andre prosjektene våre. 
<br>
Vi trenger også derfor bare installere nuget pakker i Core prosjektet, og disse er automatisk også tilgjengelig i de andre prosjektene. 
<br>
I dette prosjektet har vi også bruk for å kunne "gjemme" noen secrets som ikke skal exponeres ut fra prosjektet. Et eksempel på dette er connectionstringen for å koble oss til mongodb.<br>

I dette prosjektet viste vi hvordan vi kunne bruke et dotnet cli verktøy som het user-secrets for å håndtere dette. <br>

```bash
dotnet tool install --global dotnet-user-secrets
```

dette tilgjengeliggjør følgende cli verktøy:

```bash
dotnet user-secrets
```

Vi kan initialisere en "keychain" via følgende kommando:

```bash
dotnet user-secrets init
```

Og vi kan nå legge til hemmelige verdier til vår "keychain" via set commandoen:


```bash
dotnet user-secrets set "key" "value"
```

For å hente ut disse, må vi ha tilgang til IConfiguration interfacen. Den er med som standard i WebApi templaten vi bruker, men ikke i vårt klassebibliotek, så skal vi ta den i bruk der må vi hente både den, og userSecret extentionen fra nuget:

```bash
dotnet add package Microsoft.Extensions.Configuration

dotnet add package Microsoft.Extensions.Configuration.UserSecrets
```

Da kan vi bruke nøkkelen vi definerte som "key" for å hente ut en "value" fra KeyChain. 

Det finnes andre "keychain" løsninger dere kan komme borti i arbeidslivet. Både Azure og AWS har sine egne devops keychain løsninger, en annen populær løsning er HashiCorp's sin Vault.

Vi kan hente ut generell configurasjon via json filer ved å bruke pakken:

```bash
dotnet add package Microsoft.Extensions.Configuration.Json
```

Vi kan hente ut configurasjon fra environmentet applikasjonen kjører i via pakken:

```bash
dotnet add package Microsoft.Extensions.Configuration.EnvironmentVariables
```


## Actions
Vi har nå satt opp et sett med Actions for å kunne automatisk validere og skjekke at testene våre fungerer via Github workflows. <br>
Vi pinger databasen vår via en connectionstring som er i secrets environmentet i repoet vårt. <br>
For en enkel introduksjon til hvordan disse virker, se gjerne på kommentarene i ci.yaml filen som ligger i .github/workflow folderen i prosjektet (dette er folderen github bruker for å finne workflow konfigurasjonsfiler)

Vi har nå sett at vi kan etablere, lese og skrive til en mongo db atlas server via koden vår. Men koden vi har nå er veldig grov.

Neste steg:
- [ ] Hardkodete databasenavn og collectionnavn er fy-fy. Vi bør finne en metode som lar oss injecte databasenavn som driveren skal koble seg til direkte.

- [ ] Mye av businesslogikken vår lever nå i Controlleren vår vi bruke for eksperimentering. Det er også fy-fy. Vi bør ha som mål å sette opp en service for hver collection, så en controller for hver service. 

- [ ] Vi kan implementere tester for servicene våre, for å ensure at funksjonaliteten er som forventet. Når vi implementerer testene våre, bør vi tenke på hva som er miminumskravet for hver funksjonalitet i servicen vår, så bygge testene opp gradvis der etter. Litt som med den super enkle ping testen vi har: Vi tester først om vi kan hente ned / garantere for at connection stringen vår finnes, før vi har en etterfølgende test som prøver å pinge databasen med stringen.