FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /source/

COPY *.csproj ./
RUN dotnet restore -r linux-musl-x64
COPY . ./

RUN dotnet publish -c release -o /app -r linux-musl-x64 --no-restore /p:PublishTrimmed=true

FROM mcr.microsoft.com/dotnet/aspnet:5.0-alpine
WORKDIR /app
COPY --from=build /app ./
#RUN apk update
#RUN apk upgrade
#RUN apk add bash
RUN apk add icu-libs
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false
ENTRYPOINT ["./Cheetas3.EU.Converter"]
