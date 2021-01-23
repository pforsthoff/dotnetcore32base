### Command to Add Migration 
(Execute the Command in the solution directory (src))

(install) dotnet tool install --global dotnet-ef --version 5.0.1
(upgrade) dotnet tool update --global dotnet-ef

```shell
dotnet ef migrations add CheetasDb_Migration -s Cheetas3.EU.WebApi -p Cheetas3.EU.Infrastructure
dotnet ef migrations add CheetasDb_Migration -s .\API\Cheetas3.EU.WebApi\ -p .\Infrastructure\Cheetas3.EU.Persistence\
```

### Docker Run Commands 
EU Conversion Service

```shell
docker run -it -d <Continer Image ID> -e SliceId=3 [-e ServiceHealthEndPoint="http://localhost:5000/actuator/health"] [-e SleepDuration=10000] [-e RetryCount=5]
```
### K8S yaml files are locted in 


TODO:
LIST
 1. Persist Message from Rabbit from MessageQueue Service <Slice Updates>
 2. (DONE) Create Parameters Controller & CQRS for updates (DONE)
 3. Manage Concurrency for Jobs
 4. Manage Jobs from Queues
 5. HostInfo Actuator - Paul
 6. k8s secrets for connections - Paul
 7. More housekeeping for Clean Architecture
 8. Jobs in their own namespaces [cheetasv3sql.default.svc.cluster.local]
 8. CI/CD Pipeline
