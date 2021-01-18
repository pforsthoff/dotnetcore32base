### Command to Add Migration 
(Execute the Command in the solution directory (src))

(install) dotnet tool install --global dotnet-ef --version 5.0.1
(upgrade) dotnet tool update --global dotnet-ef

```shell
dotnet ef migrations add CheetasDb_Migration -s Cheetas3.EU.WebApi -p Cheetas3.EU.Infrastructure
```

### Docker Run Commands 
EU Conversion Service

```shell
docker run -it -d <Continer Image ID> -e SliceId=3 -e ServiceHealthEndPoint="http://localhost:8890/actuator/health" -e SleepDuration=10000
```