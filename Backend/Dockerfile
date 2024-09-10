#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base

ENV ASPNETCORE_URLS=http://+:8080

WORKDIR /backend_app

EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /backend_src
COPY ["Aurigma.DirectMail.Sample.sln", "."]
COPY ["src/Aurigma.DirectMail.Sample.App/Aurigma.DirectMail.Sample.App.csproj", "src/Aurigma.DirectMail.Sample.App/"]
COPY ["src/Aurigma.DirectMail.Sample.DAL.EFCore/Aurigma.DirectMail.Sample.DAL.EFCore.csproj", "src/Aurigma.DirectMail.Sample.DAL.EFCore/"]
COPY ["src/Aurigma.DirectMail.Sample.DAL.Postgres/Aurigma.DirectMail.Sample.DAL.Postgres.csproj", "src/Aurigma.DirectMail.Sample.DAL.Postgres/"]
COPY ["src/Aurigma.DirectMail.Sample.DAL.FileSystem/Aurigma.DirectMail.Sample.DAL.FileSystem.csproj", "src/Aurigma.DirectMail.Sample.DAL.FileSystem/"]
COPY ["src/Aurigma.DirectMail.Sample.DomainEntities/Aurigma.DirectMail.Sample.DomainEntities.csproj", "src/Aurigma.DirectMail.Sample.DomainEntities/"]
COPY ["src/Aurigma.DirectMail.Sample.WebApi/Aurigma.DirectMail.Sample.WebApi.csproj", "src/Aurigma.DirectMail.Sample.WebApi/"]
COPY ["src/Aurigma.DirectMail.Sample.WebHost/Aurigma.DirectMail.Sample.WebHost.csproj", "src/Aurigma.DirectMail.Sample.WebHost/"]
RUN dotnet restore "/backend_src/src/Aurigma.DirectMail.Sample.WebHost/Aurigma.DirectMail.Sample.WebHost.csproj"
COPY . .
WORKDIR "/backend_src/src/Aurigma.DirectMail.Sample.WebHost/"
RUN dotnet build "Aurigma.DirectMail.Sample.WebHost.csproj" -c Release -o /backend_app/build

FROM build AS publish
RUN dotnet publish "Aurigma.DirectMail.Sample.WebHost.csproj" -r linux-x64 -c Release -o /backend_app/publish

FROM base AS final
WORKDIR /backend_app
COPY --from=publish /backend_app/publish .

ENTRYPOINT ["dotnet", "Aurigma.DirectMail.Sample.WebHost.dll"]