# BUILD STAGE
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy the entire solution
COPY . .

# Restore dependencies
RUN dotnet restore VoiceChat.Server/VoiceChat.Server.csproj

# Compile and publish
RUN dotnet publish VoiceChat.Server/VoiceChat.Server.csproj \
    -c Release \
    -o /app/publish \
    --no-restore

# RUNTIME STAGE
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime

WORKDIR /app

COPY --from=build /app/publish .

# Render supplies PORT; locally, default to 8080
ENTRYPOINT ["sh", "-c", "dotnet VoiceChat.Server.dll --urls http://0.0.0.0:${PORT:-8080}"]
