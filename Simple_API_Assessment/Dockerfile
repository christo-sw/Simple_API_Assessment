#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Simple_API_Assessment/Simple_API_Assessment.csproj", "Simple_API_Assessment/"]
RUN dotnet restore "./Simple_API_Assessment/Simple_API_Assessment.csproj"
COPY . .
WORKDIR "/src/Simple_API_Assessment"
RUN dotnet build "./Simple_API_Assessment.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Simple_API_Assessment.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Simple_API_Assessment.dll"]