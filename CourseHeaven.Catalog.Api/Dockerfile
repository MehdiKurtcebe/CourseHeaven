FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CourseHeaven.Catalog.Api/CourseHeaven.Catalog.Api.csproj", "CourseHeaven.Catalog.Api/"]
COPY ["CourseHeaven.Bus/CourseHeaven.Bus.csproj", "CourseHeaven.Bus/"]
COPY ["CourseHeaven.Shared/CourseHeaven.Shared.csproj", "CourseHeaven.Shared/"]
RUN dotnet restore "CourseHeaven.Catalog.Api/CourseHeaven.Catalog.Api.csproj"
COPY . .
WORKDIR "/src/CourseHeaven.Catalog.Api"
RUN dotnet build "./CourseHeaven.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CourseHeaven.Catalog.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseHeaven.Catalog.Api.dll"]
