# 📚 書香世家 — 圖書館管理系統

> ASP.NET Core 8.0 MVC 圖書館管理系統，採用 SOLID 架構設計，具備完整的借閱、預約、財務與 ERP 管理功能。

---

## 目錄

- [功能概覽](#功能概覽)
- [技術架構](#技術架構)
- [專案結構](#專案結構)
- [快速開始](#快速開始)
- [資料庫設定](#資料庫設定)
- [執行測試](#執行測試)
- [系統截圖](#系統截圖)

---

## 功能概覽

### 會員端
| 功能 | 說明 |
|------|------|
| 書籍搜尋 | 依書名、作者、分類搜尋館藏 |
| 借閱管理 | 借書、還書、查看借閱紀錄 |
| 預約服務 | 預約熱門書籍，到架即通知 |
| 個人帳戶 | 資料管理、借閱歷史、逾期罰款 |

### 管理員端
| 功能 | 說明 |
|------|------|
| 書籍管理 | 新增、編輯、刪除書目與庫存 |
| 會員管理 | 查看、停用、管理會員帳戶 |
| 借還書管理 | 處理借閱申請、還書確認 |
| 罰款管理 | 逾期罰款計算與繳費紀錄 |
| 財務報表 | 收支總覽、收入明細、費用管理 |
| 分類管理 | 書籍分類維護 |
| 系統設定 | 借閱天數、最大借閱數等規則設定 |
| 排行榜 | 熱門書籍借閱統計 |

---

## 技術架構

```
Frontend        │  Razor Views (.cshtml)、Bootstrap 5、Vue.js 3、Chart.js
Backend         │  ASP.NET Core 8.0 MVC
ORM             │  Entity Framework Core 8.0 + Pomelo (MySQL)
Auth            │  ASP.NET Core Identity（角色：Admin / Member）
Testing         │  xUnit + Moq + FluentAssertions
```

### 架構分層（SOLID 原則）

```
Controllers  ──→  Services  ──→  Repositories  ──→  DbContext
   (路由)       (業務邏輯)        (資料存取)         (MySQL)
                    ↕                  ↕
               Interfaces          Interfaces
             (依賴倒轉原則)       (依賴倒轉原則)
```

- **Controllers**：僅處理 HTTP 請求/回應，委派業務邏輯給 Service
- **Services**：封裝業務規則，透過 `ServiceResult<T>` 回傳操作結果
- **Repositories**：統一資料存取介面，隔離 EF Core 實作細節

---

## 專案結構

```
Library/
├── Library/                        # 主要 Web 專案
│   ├── Controllers/                # 22 個控制器
│   ├── Services/
│   │   ├── Interfaces/             # 11 個 Service 介面
│   │   └── *.cs                    # 11 個 Service 實作
│   ├── Repositories/
│   │   ├── Interfaces/             # 10 個 Repository 介面
│   │   └── *.cs                    # 10 個 Repository 實作
│   ├── Models/                     # 11 個 Domain Model
│   ├── ViewModels/                 # ServiceResult、ViewModel
│   ├── Data/                       # LibrarydbContext
│   ├── Migrations/                 # 13 個資料庫遷移
│   ├── Views/                      # Razor 視圖
│   └── wwwroot/                    # 靜態資源（CSS / JS）
│
├── Library.Tests/                  # xUnit 測試專案
│   └── Services/                   # 8 個 Service 測試類別
│
└── Library.sln
```

---

## 快速開始

### 先決條件

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- MySQL 8.0+

### 安裝步驟

```bash
# 1. Clone 專案
git clone https://github.com/Xiu133/LibrarySystem.git
cd LibrarySystem/Library

# 2. 設定資料庫連線字串（appsettings.json）
# 見下方「資料庫設定」

# 3. 套用資料庫遷移
dotnet ef database update

# 4. 啟動專案
dotnet run
```

瀏覽器開啟 `https://localhost:5001`

---

## 資料庫設定

編輯 `Library/appsettings.json`：

```json
{
  "ConnectionStrings": {
    "LibraryConnection": "Server=localhost;Database=librarydb;User=root;Password=你的密碼;",
    "IdentityConnection": "Server=localhost;Database=librarydb;User=root;Password=你的密碼;"
  }
}
```


---

## 執行測試

```bash
cd Library.Tests
dotnet test
```

測試涵蓋範圍：

| 測試類別 | 說明 |
|----------|------|
| BookServiceTests | 書籍新增、搜尋、刪除邏輯 |
| BorrowServiceTests | 借書、還書、逾期判斷 |
| ReserveServiceTests | 預約、取消、通知觸發 |
| NotifyServiceTests | 通知建立與讀取 |
| BorrowRuleServiceTests | 借閱規則設定與讀取 |
| CategoryServiceTests | 分類 CRUD |
| FineServiceTests | 罰款計算與管理 |
| MemberServiceTests | 會員資料管理 |

---

## 系統截圖

### 首頁（未登入）
![首頁](docs/screenshot-home.png)

> 深色漸層 Hero 設計，提供「立即登入」與「免費註冊」快速入口，底部展示豐富館藏、便利借閱、預約服務、排行榜四大功能亮點。

### 管理後台 Dashboard
![管理後台](docs/screenshot-admin.png)

> 一覽書籍總數、會員數、借閱中、逾期書籍、未繳罰款、總收入等 KPI 卡片，搭配近 6 個月借閱趨勢折線圖與最近活動列表。

---

## License

MIT
