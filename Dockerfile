FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["backend_tfg.csproj", "./"]
RUN dotnet restore "./backend_tfg.csproj"

COPY . .
RUN dotnet publish "./backend_tfg.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

EXPOSE 5000
ENTRYPOINT ["dotnet", "backend_tfg.dll"]
