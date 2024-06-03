#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Presentation/RectangleApp.Api/RectangleApp.Api.csproj", "Presentation/RectangleApp.Api/"]
COPY ["Infrastructure/Project.Service/Project.Service.csproj", "Infrastructure/Project.Service/"]
COPY ["Infrastructure/Project.Infrastructure/Project.Infrastructure.csproj", "Infrastructure/Project.Infrastructure/"]
COPY ["Core/Project.Core/Project.Core.csproj", "Core/Project.Core/"]
RUN dotnet restore "./Presentation/RectangleApp.Api/RectangleApp.Api.csproj"
COPY . .
WORKDIR "/src/Presentation/RectangleApp.Api"
RUN dotnet build "./RectangleApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./RectangleApp.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RectangleApp.Api.dll"]