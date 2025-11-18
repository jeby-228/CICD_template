# PortalApi - ABP Framework Application

這是一個使用 ABP Framework 8.2.1 和 PostgreSQL 建立的 ASP.NET Core 應用程式。

## 專案結構

```
PortalApi/
├── src/
│   └── PortalApi/
│       ├── Controllers/           # API 控制器
│       │   └── ProductsController.cs
│       ├── Entities/              # 領域實體
│       │   └── Product.cs
│       ├── EntityFrameworkCore/   # EF Core 配置
│       │   └── PortalApiDbContext.cs
│       ├── Properties/            # 啟動配置
│       ├── PortalApiModule.cs     # ABP 模組定義
│       ├── Program.cs             # 應用程式入口點
│       └── appsettings.json       # 應用程式設定
└── PortalApi.csproj               # 專案檔案
```

## 功能特色

- ✅ ABP Framework 8.2.1
- ✅ Entity Framework Core with PostgreSQL
- ✅ Autofac 依賴注入容器
- ✅ RESTful API 支援
- ✅ Entity Framework Core Migrations 支援
- ✅ 模組化架構

## 資料庫設定

### 連接字串

在 `appsettings.json` 中配置 PostgreSQL 連接字串：

```json
{
  "ConnectionStrings": {
    "Default": "Host=localhost;Port=5432;Database=PortalApiDb;Username=postgres;Password=postgres"
  }
}
```

### 執行資料庫遷移

```bash
# 建立第一個遷移
dotnet ef migrations add InitialCreate

# 更新資料庫
dotnet ef database update
```

## API 端點

應用程式提供以下 API 端點（範例）：

### Products API

- `GET /api/products` - 取得所有產品
- `GET /api/products/{id}` - 取得特定產品
- `POST /api/products` - 建立新產品
- `PUT /api/products/{id}` - 更新產品
- `DELETE /api/products/{id}` - 刪除產品

### 範例請求

建立產品：
```bash
curl -X POST https://localhost:5001/api/products \
  -H "Content-Type: application/json" \
  -d '{
    "name": "Sample Product",
    "description": "This is a sample product",
    "price": 99.99,
    "stock": 100
  }'
```

## 開發

### 執行應用程式

```bash
cd src/PortalApi
dotnet run
```

應用程式將在以下位址啟動：
- HTTP: http://localhost:5000
- HTTPS: https://localhost:5001

### 建置專案

```bash
dotnet build
```

### 執行測試

```bash
dotnet test
```

## 套件依賴

主要套件：
- `Volo.Abp.AspNetCore.Mvc` - ABP Framework ASP.NET Core MVC 整合
- `Volo.Abp.Autofac` - Autofac 依賴注入容器整合
- `Volo.Abp.EntityFrameworkCore.PostgreSql` - PostgreSQL 資料庫提供者
- `Microsoft.EntityFrameworkCore.Design` - EF Core 設計時工具

## 環境需求

- .NET 8.0 SDK
- PostgreSQL 12 或更新版本

## 授權

MIT License
