FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 80
# Stage 1: buil
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build 
WORKDIR /src
COPY ["FoodProject.csproj","src/"]
RUN dotnet restore "src/FoodProject.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "FoodProject.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "FoodProject.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: runtime
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FoodProject.dll"] 
