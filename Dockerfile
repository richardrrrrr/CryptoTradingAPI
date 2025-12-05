# 1. Runtime image（跑程式用）
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080

# 2. Build image（編譯程式用）
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# 把整個 repo 複製進來
COPY . .

# 還原套件
RUN dotnet restore CryptoTrading.sln

# 編譯 + 發佈
RUN dotnet publish src/CryptoTrading.API/CryptoTrading.API.csproj -c Release -o /app/publish

# 3. 最終 image：只包含 runtime + publish 結果
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

# 啟動指令（如果你的 dll 名稱不同，記得改）
ENTRYPOINT ["dotnet", "CryptoTrading.API.dll"]
