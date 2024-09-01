# Stage 1: Build the API project
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

# Copy the csproj files and restore dependencies
COPY backend/Diuna.API/Diuna.API.csproj ./backend/Diuna.API/
COPY backend/Diuna.Models/Diuna.Models.csproj ./backend/Diuna.Models/
COPY backend/Diuna.Services/Diuna.Services.csproj ./backend/Diuna.Services/
COPY backend/Diuna.SignalR/Diuna.SignalR.csproj ./backend/Diuna.SignalR/

# Restore dependencies
WORKDIR /app/backend/Diuna.API
RUN dotnet restore

# Copy all source code
COPY backend/ ./backend/

# Build and publish only the API project
WORKDIR /app/backend/Diuna.API
RUN dotnet publish "Diuna.API.csproj" -c Release -o out

# Stage 2: Use the runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime
WORKDIR /app
COPY --from=build /app/backend/Diuna.API/out .

# Expose the port
EXPOSE 80

# Set the entry point to the API DLL
ENTRYPOINT ["dotnet", "Diuna.API.dll"]
