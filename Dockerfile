FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /src
EXPOSE 80
# Copy everything
COPY ./API ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /src
COPY --from=build-env /src/out .

ENTRYPOINT ["dotnet", "API.dll"]