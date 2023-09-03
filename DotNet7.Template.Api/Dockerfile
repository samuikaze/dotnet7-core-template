#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["DotNet7.Template.Api/DotNet7.Template.Api.csproj", "DotNet7.Template.Api/"]
COPY ["DotNet7.Template.Repository/DotNet7.Template.Repository.csproj", "DotNet7.Template.Repository/"]
RUN dotnet restore "DotNet7.Template.Api/DotNet7.Template.Api.csproj"
COPY . .
WORKDIR "/src/DotNet7.Template.Api"
RUN dotnet build "DotNet7.Template.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DotNet7.Template.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DotNet7.Template.Api.dll"]