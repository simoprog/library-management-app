FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY ["LibraryManagement.Api/LibraryManagement.Api.csproj", "LibraryManagement.Api/"]
COPY ["LibraryManagement.Application/LibraryManagement.Application.csproj", "LibraryManagement.Application/"]
COPY ["LibraryManagement.Domain/LibraryManagement.Domain.csproj", "LibraryManagement.Domain/"]
COPY ["LibraryManagement.Infrastructure/LibraryManagement.Infrastructure.csproj", "LibraryManagement.Infrastructure/"]
RUN dotnet restore "LibraryManagement.Api/LibraryManagement.Api.csproj"
COPY . .
WORKDIR "/src/LibraryManagement.Api"
RUN dotnet build "LibraryManagement.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "LibraryManagement.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LibraryManagement.Api.dll"]