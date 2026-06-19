FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY EduApoyos.sln .
COPY EduApoyos.Domain/EduApoyos.Domain.csproj EduApoyos.Domain/
COPY EduApoyos.Application/EduApoyos.Application.csproj EduApoyos.Application/
COPY EduApoyos.Infrastructure/EduApoyos.Infrastructure.csproj EduApoyos.Infrastructure/
COPY EduApoyos.API/EduApoyos.API.csproj EduApoyos.API/
COPY EduApoyos.Tests/EduApoyos.Tests.csproj EduApoyos.Tests/

RUN dotnet restore

COPY . .

RUN dotnet publish EduApoyos.API/EduApoyos.API.csproj \
    --configuration Release \
    --no-restore \
    --output /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "EduApoyos.API.dll"]
