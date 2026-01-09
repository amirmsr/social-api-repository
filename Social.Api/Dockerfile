# Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY Social.Api/*.csproj Social.Api/
RUN dotnet restore Social.Api/Social.Api.csproj

COPY . .
RUN dotnet publish Social.Api/Social.Api.csproj -c Release -o /out

# Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /out .
EXPOSE 8080

ENTRYPOINT ["dotnet", "Social.Api.dll"]
