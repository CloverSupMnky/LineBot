#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["6.WebAPI/LineBot.WebAPI/LineBot.WebAPI.csproj", "6.WebAPI/LineBot.WebAPI/"]
COPY ["5.Module/LineBot.Module.Service/LineBot.Module.Service.csproj", "5.Module/LineBot.Module.Service/"]
COPY ["5.Module/LineBot.Module.Interface/LineBot.Module.Interface.csproj", "5.Module/LineBot.Module.Interface/"]
COPY ["3.Entity/LineBot.Entitys/LineBot.Entitys.csproj", "3.Entity/LineBot.Entitys/"]
COPY ["2.Asset/LineBot.Asset/LineBot.Asset.csproj", "2.Asset/LineBot.Asset/"]
COPY ["1.Plugins/LineBot.Plugins/LineBot.Plugins.csproj", "1.Plugins/LineBot.Plugins/"]
RUN dotnet restore "6.WebAPI/LineBot.WebAPI/LineBot.WebAPI.csproj"
COPY . .
WORKDIR "/src/6.WebAPI/LineBot.WebAPI"
RUN dotnet build "LineBot.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LineBot.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
CMD ASPNETCORE_URLS=http://*:$PORT dotnet LineBot.WebAPI.dll