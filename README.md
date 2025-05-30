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

