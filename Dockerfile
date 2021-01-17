FROM mcr.microsoft.com/dotnet/aspnet:5.0.2-alpine3.12-amd64
COPY src/Cheetas3.EU.WebApi/bin/Release/net5.0/publish/ /app

WORKDIR /app
RUN ls
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "WebApi.dll"]