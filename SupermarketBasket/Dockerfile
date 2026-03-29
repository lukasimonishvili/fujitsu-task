FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY . ./

RUN dotnet restore ./API/*.csproj
RUN dotnet publish ./API/*.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app

COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "API.dll"]