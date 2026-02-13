# DistributedCalculations

#### **Opgavebeskrivelse**
**Must have:**
1. Create a basic calculator with history.
2. There should be a clear separation of logic.
3. All logic should be covered by unit tests.
4. The calculator should expose a RESTful interface deployed via a backend technology fx Firebase, but a docker container on eg. Heroku will also suffice or something else you are familar with.
5. The interface should be documented with an accompanying Postman collection for easy testing.
6. All source code should be available via your personal Github account.

**Nice to have:**
1. Best effort micro-service architecture (given the timeframe).
2. Auth for using the service (email/password will suffice).
3. A small webpage utilizing the calculator.

As the title says, it is an over-engineered calculator, which means: You should go all-in on design patterns and best practices as well as making sure to fulfill the "must have" requirements. The coding language is also up to you, but GO lang would be preferred, since it is part of our tech stack.

#### **Overvejelser**
1. Skal hver operation have deres egen service, eller skal alt udregning samles i én service?
    1. Laver én Calculation-service der udregner basic matematiske operationer.
2. Minimal API til POST metoder til at modtage regnestykke og returnere resultat?
    1. Tynd API som kun modtager og returnere data, minimal API passer godt til.
3. Microservice arkitektur, hvilke og hvor mange services er nødvendige?
    1. Synkront vs Event-baseret kommunikation, tæt vs løs kobling.
4. Historik over udregninger, skal det være in-memory eller persistent?
    1. Til at starte med, gemmes udregningerne in-memory.
5. Pattern til separation of logic?
    1. Opdeler udregning, api, og historik logik i hver deres services med ét klart ansvar, nemmere at teste separat logik.
    2. Minimal API der orkestrerer I/O.
    3. Calculation-service står for udregning.
    4. History-service gemmer operationerne, kan ændres uafhængigt af Calculation.
    5. Monorepo da det er et mindre POC-projekt.
    6. Clean Architecture pattern
6. Muligheder for at skalere projektet?
    1. Skal kunne skaleres uafhængigt og horisontalt.
