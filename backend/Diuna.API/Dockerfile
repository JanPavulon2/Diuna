# Use the official ASP.NET Core runtime as a base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the official ASP.NET Core SDK to build the app
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Diuna.API/Diuna.API.csproj", "Diuna.API/"]
COPY ["Diuna.SignalR/Diuna.SignalR.csproj", "Diuna.SignalR/"]
COPY ["Diuna.Services/Diuna.Services.csproj", "Diuna.Services/"]
COPY ["Diuna.Models/Diuna.Models.csproj", "Diuna.Models/"]

# Restore the dependencies for the Web API project
RUN dotnet restore "Diuna.API/Diuna.API.csproj"

# Copy the rest of the source code and build the application
COPY . .
WORKDIR "/src/Diuna.API"
RUN dotnet build "Diuna.API.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "Diuna.API.csproj" -c Release -o /app/publish

# Final stage - set up the runtime environment
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Diuna.API.dll"]
