# CICD_template
給C# 專案使用的CICD 模板 

![Alt](https://repobeats.axiom.co/api/embed/471b719a2db89bdf73223d371c9ecc340211b790.svg "Repobeats analytics image")

## 專案簡介

本專案使用 ABP Framework 建立，這是一個現代化的 ASP.NET Core 應用程式模板。

### 技術堆疊

- **框架**: ABP Framework 8.2.1
- **資料庫**: PostgreSQL
- **ORM**: Entity Framework Core with PostgreSQL Provider
- **依賴注入**: Autofac
- **.NET 版本**: .NET 8.0

### 專案結構

```
PortalApi/
├── src/
│   └── PortalApi/              # 主要應用程式專案
│       ├── EntityFrameworkCore/ # EF Core DbContext 和配置
│       ├── PortalApiModule.cs  # ABP 模組定義
│       ├── Program.cs          # 應用程式入口點
│       └── appsettings.json    # 應用程式設定
└── PortalApi.sln               # 解決方案檔案
```

## 開始使用

### 先決條件

- .NET 8.0 SDK 或更新版本
- PostgreSQL 資料庫伺服器

### 資料庫設定

1. 確保 PostgreSQL 正在運行
2. 更新 `appsettings.json` 中的連接字串：

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres"
  }
}
```

### 建置和執行

```bash
# 還原套件
cd PortalApi
dotnet restore

# 建置專案
dotnet build

# 執行專案
cd src/PortalApi
dotnet run
```

應用程式將在 `http://localhost:5000` (或 `https://localhost:5001`) 上啟動。

## CI/CD

本專案包含以下 GitHub Actions 工作流程：

- **CodeQL 安全掃描**: 自動化安全漏洞掃描（支援 C# 和 JavaScript/TypeScript）
- **Pull Request 檢查**: 程式碼品質檢查和自動化審查
- **Dependabot**: 自動依賴更新

## 授權

本專案採用 MIT 授權 - 詳見 [LICENSE](LICENSE) 檔案。

