FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

RUN apt-get update \
    && apt-get install -y curl

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["src/BeatmapsService/BeatmapsService.csproj", "BeatmapsService/"]
RUN dotnet restore "BeatmapsService/BeatmapsService.csproj"

COPY src/ .

WORKDIR "/src/BeatmapsService"
RUN dotnet build "BeatmapsService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BeatmapsService.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app

COPY --from=publish /app/publish .

COPY scripts /scripts
RUN chmod u+x /scripts/*

RUN apt update && apt install -y openssl python3-pip git curl
RUN pip install --break-system-packages git+https://github.com/osuAkatsuki/akatsuki-cli

ENTRYPOINT ["/scripts/bootstrap.sh"]
