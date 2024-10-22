FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /App
EXPOSE 5050
# EXPOSE 443
ENV ASPNETCORE_URLS=http://*:5050

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Argumento para capturar a hora do build
ARG BUILD_DATE

# Definir o valor de BUILD_DATE durante o build
RUN BUILD_DATE=$(date) && echo "Build Time: $BUILD_DATE"

# Passar a data como variável de ambiente para o contêiner
ENV BUILD_TIME=$BUILD_DATE


# Build runtime image
FROM build-env as final
WORKDIR /App
COPY --from=build-env /App/out .
ENTRYPOINT ["dotnet", "EFCoreRelationshipsTutorial.dll"]