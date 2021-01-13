FROM mcr.microsoft.com/dotnet/aspnet:5.0
COPY /DotNetCore32Base.MVC/bin/Release/netcoreapp3.1/publish/ /app
#WORKDIR /src
#COPY dotnetcore32base.sln ./
#COPY dotnetcore32base.mvc/*.csproj ./dotnetcore32base.mvc/
#COPY dotnetcore32base.data/*.csproj ./dotnetcore32base.data/
#COPY dotnetcore32base.service/*.csproj ./dotnetcore32base.service/

#WORKDIR /src/dotnetcore32base.data
#RUN dotnet build -c Release -o /app

#FROM build AS publish
#RUN donet publish -c Release -o /app

#FROM base AS final
WORKDIR /app
RUN ls
#COPY --from=publish /app .
# Install cultures (same approach as Alpine SDK image)
RUN apk add --no-cache icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENV ASPNETCORE_FORWARDEDHEADERS_ENABLED=true
ENV DOTNET_RUNNING_IN_CONTAINER=true

ENTRYPOINT ["dotnet", "DotNetCore32Base.MVC.dll"]