FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS build
WORKDIR /src
COPY dotnetcore32base.sln ./
COPY dotnetcore32base.mvc/*.csproj ./dotnetcore32base.mvc/
COPY dotnetcore32base.data/*.csproj ./dotnetcore32base.data/

RUN dotnet restore
COPY . .
WORKDIR /src/dotnetcore32base.mvc
RUN dotnet build -c Release -o /app

WORKDIR /src/dotnetcore32base.data
RUN dotnet build -c Release -o /app

FROM build AS publish
RUN donet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .

ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=1
ENTRYPOINT ["dotnet", "dotnetcore32base.mvc.dll"]