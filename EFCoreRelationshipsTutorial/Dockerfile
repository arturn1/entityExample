#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS base
# WORKDIR /app
# EXPOSE 80
# EXPOSE 443

# FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
# # WORKDIR "/src"
# # COPY ["EFCoreRelationshipsTutorial.csproj", "EFCoreRelationshipsTutorial/"]
# # RUN dotnet restore "EFCoreRelationshipsTutorial/EFCoreRelationshipsTutorial.csproj"
# COPY . ./
# # WORKDIR "/src/EFCoreRelationshipsTutorial"
# # RUN dotnet build "EFCoreRelationshipsTutorial.csproj" -c Release -o /app/build

# # FROM build AS publishs
# RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# FROM base AS final
# WORKDIR /app
# COPY --from=publish /app/publish .
# ENTRYPOINT ["dotnet", "EFCoreRelationshipsTutorial.dll"]

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
EXPOSE 5050
# EXPOSE 443
ENV ASPNETCORE_URLS=http://*:80

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "EFCoreRelationshipsTutorial.dll"]