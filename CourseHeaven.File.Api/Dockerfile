FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CourseHeaven.File.Api/CourseHeaven.File.Api.csproj", "CourseHeaven.File.Api/"]
COPY ["CourseHeaven.Bus/CourseHeaven.Bus.csproj", "CourseHeaven.Bus/"]
COPY ["CourseHeaven.Shared/CourseHeaven.Shared.csproj", "CourseHeaven.Shared/"]
RUN dotnet restore "CourseHeaven.File.Api/CourseHeaven.File.Api.csproj"
COPY . .
WORKDIR "/src/CourseHeaven.File.Api"
RUN dotnet build "./CourseHeaven.File.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CourseHeaven.File.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseHeaven.File.Api.dll"]
