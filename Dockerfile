FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY "Api-Gateway.sln" "Api-Gateway.sln"
COPY ["Api-Gateway.csproj", "Api-Gateway.csproj"]
RUN dotnet restore

WORKDIR /app
COPY . .

RUN dotnet build "Api-Gateway.csproj" -c Release -o /build

FROM build AS publish
RUN dotnet publish "Api-Gateway.csproj" -c Release -o /publish

FROM base AS final
WORKDIR /app/publish
COPY --from=publish /publish .
ENTRYPOINT ["dotnet", "Api-Gateway.dll"]

