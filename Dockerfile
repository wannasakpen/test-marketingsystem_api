#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build

WORKDIR /src
COPY ["AP_MediaService.API/AP_MediaService.API.csproj", "AP_MediaService.API/"]
RUN dotnet restore "APSharingAPI/APSharingAPI.csproj"
COPY . .
WORKDIR "/src/AP_MediaService.API"
RUN dotnet build "AP_MediaService.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "AP_MediaService.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AP_MediaService.API.dll"]

# COPY . /app
# WORKDIR /app/AP_MediaService.API

# RUN dotnet restore
# RUN dotnet publish -c Release --output /app/publish

# FROM base AS final
# WORKDIR /app
# COPY --from=build /app/publish .
# ENTRYPOINT ["dotnet", "AP_MediaService.API.dll"]

# ENV TZ=Asia/Bangkok
# RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone