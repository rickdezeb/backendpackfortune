FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

LABEL org.opencontainers.image.source=https://github.com/rickdezeb/backendpackfortune

FROM base AS final
WORKDIR /app
COPY ./publish .
ENTRYPOINT ["dotnet", "Packfortune.API.dll"]