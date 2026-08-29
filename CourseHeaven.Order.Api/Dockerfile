FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["CourseHeaven.Order.Api/CourseHeaven.Order.Api.csproj", "CourseHeaven.Order.Api/"]
COPY ["CourseHeaven.Order.Application/CourseHeaven.Order.Application.csproj", "CourseHeaven.Order.Application/"]
COPY ["CourseHeaven.Bus/CourseHeaven.Bus.csproj", "CourseHeaven.Bus/"]
COPY ["CourseHeaven.Order.Domain/CourseHeaven.Order.Domain.csproj", "CourseHeaven.Order.Domain/"]
COPY ["CourseHeaven.Shared/CourseHeaven.Shared.csproj", "CourseHeaven.Shared/"]
COPY ["CourseHeaven.Order.Persistence/CourseHeaven.Order.Persistence.csproj", "CourseHeaven.Order.Persistence/"]
RUN dotnet restore "CourseHeaven.Order.Api/CourseHeaven.Order.Api.csproj"
COPY . .
WORKDIR "/src/CourseHeaven.Order.Api"
RUN dotnet build "./CourseHeaven.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./CourseHeaven.Order.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CourseHeaven.Order.Api.dll"]
