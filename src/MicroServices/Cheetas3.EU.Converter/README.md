Docker Build Command
```
docker build -t pguerette/euconverter:latest .
docker build -f DockerFiler.EUConverter -t pguerette/euconverter:latest .
```

Docker Run Command
```
docker run -it -d 3d240efde245 -e SliceId=3 -e ApiHealthUrl="http://localhost:5000/actuator/health" -e SleepDuration=10000
```
YAML Files for Kubernetes Deployments in YAML Folder