FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["AZSync.API/AZSync.API.csproj", "AZSync.API/"]
RUN dotnet restore "AZSync.API/AZSync.API.csproj"
COPY . .
WORKDIR "/src/AZSync.API"
RUN dotnet build "AZSync.API.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "AZSync.API.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "AZSync.API.dll"]