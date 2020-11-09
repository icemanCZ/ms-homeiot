#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["HomeIOT.Web/HomeIOT.Web.csproj", "HomeIOT.Web/"]
RUN dotnet restore "HomeIOT.Web/HomeIOT.Web.csproj"
COPY . .
WORKDIR "/src/HomeIOT.Web"
RUN dotnet build "HomeIOT.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HomeIOT.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HomeIOT.Web.dll"]