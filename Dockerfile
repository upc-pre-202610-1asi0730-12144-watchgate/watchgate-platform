FROM mcr.microsoft.com/dotnet/sdk:10.0 AS builder
WORKDIR /app
COPY Watchgate.Locksight.Platform/*.csproj Watchgate.Locksight.Platform/
RUN dotnet restore ./Watchgate.Locksight.Platform/Watchgate.Locksight.Platform.csproj
COPY . .
RUN dotnet publish ./Watchgate.Locksight.Platform/Watchgate.Locksight.Platform.csproj -c Release -o /app/out

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=builder /app/out .
EXPOSE 8080
ENTRYPOINT ["dotnet", "Watchgate.Locksight.Platform.dll"]
