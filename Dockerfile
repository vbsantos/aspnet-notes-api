FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["dotnet-notes-api.csproj", "dotnet-notes-api/"]
RUN dotnet restore "dotnet-notes-api/dotnet-notes-api.csproj"

WORKDIR "/src/dotnet-notes-api"
COPY . .
RUN dotnet build "dotnet-notes-api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "dotnet-notes-api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "dotnet-notes-api.dll"]
