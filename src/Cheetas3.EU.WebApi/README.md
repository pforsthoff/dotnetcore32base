### Command to Add Migration 
(Execute the Command in the Infrastructure Project Path)
```shell
dotnet ef migrations add Migration -s Cheetas3.EU.WebApi -p Cheetas3.EU.Infrastructure
```

### Docker Run Commands 
EU Conversion Service

```shell
docker run -it -d <Continer Image ID> -e SliceId=3 -e ServiceHealthEndPoint="http://localhost:8890/actuator/health" -e SleepDuration=10000
```