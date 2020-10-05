#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/runtime:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["Berger.Worker/Berger.Worker.csproj", "Berger.Worker/"]
RUN dotnet restore "Berger.Worker/Berger.Worker.csproj"
COPY . .
WORKDIR "/src/Berger.Worker"
RUN dotnet build "Berger.Worker.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Berger.Worker.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Berger.Worker.dll"]