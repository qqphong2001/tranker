FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/ExpenseManagement.WebAPI/ExpenseManagement.WebAPI.csproj", "src/ExpenseManagement.WebAPI/"]
COPY ["src/ExpenseManagement.Application/ExpenseManagement.Application.csproj", "src/ExpenseManagement.Application/"]
COPY ["src/ExpenseManagement.Domain/ExpenseManagement.Domain.csproj", "src/ExpenseManagement.Domain/"]
COPY ["src/ExpenseManagement.Infrastructure/ExpenseManagement.Infrastructure.csproj", "src/ExpenseManagement.Infrastructure/"]
RUN dotnet restore "src/ExpenseManagement.WebAPI/ExpenseManagement.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/ExpenseManagement.WebAPI"
RUN dotnet build "ExpenseManagement.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ExpenseManagement.WebAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ExpenseManagement.WebAPI.dll"]
