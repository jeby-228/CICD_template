# CI/CD 安全掃描指南

本文檔說明如何使用和維護本專案的 CI/CD 安全掃描功能。

## 📋 目錄

- [功能概述](#功能概述)
- [CodeQL 安全掃描](#codeql-安全掃描)
- [Pull Request 安全檢查](#pull-request-安全檢查)
- [查看和下載報告](#查看和下載報告)
- [修復安全漏洞](#修復安全漏洞)
- [配置和自定義](#配置和自定義)

## 功能概述

本專案的 CI/CD 管道包含多層次的安全檢查：

### 🔒 自動化安全掃描

- **語言支援**: C# 和 JavaScript/TypeScript
- **掃描頻率**: 
  - 每次推送到 `main` 分支
  - 每個 Pull Request
  - 每週一凌晨 2 點定期掃描
- **報告生成**: 自動生成 SARIF 格式的漏洞報告
- **報告保留**: Artifacts 保留 90 天

### 📊 漏洞報告功能

1. **GitHub Actions Summary**
   - 在每次掃描後立即顯示摘要
   - 按嚴重程度分類的漏洞統計
   - 視覺化的問題指標

2. **SARIF 報告**
   - 標準化的安全報告格式
   - 可下載供離線分析
   - 相容於多種安全分析工具

3. **GitHub Security 整合**
   - 自動上傳到 Security 頁面
   - 持續追蹤漏洞狀態
   - 整合修復建議

## CodeQL 安全掃描

### 工作流程說明

#### 1. `codeql.yml` - JavaScript/TypeScript 掃描

針對前端程式碼的輕量級掃描：

```yaml
觸發條件:
- Push to main
- Pull requests to main
- 每週一排程執行
```

**功能**:
- 自動偵測 JS/TS 檔案
- 執行安全和品質查詢
- 生成 SARIF 報告
- 上傳到 GitHub Security
- 生成視覺化摘要

#### 2. `codeql-full.yml` - 完整掃描

針對所有支援語言的全面掃描：

```yaml
支援語言:
- C# (manual build mode)
- JavaScript/TypeScript (none build mode)

矩陣策略:
- 並行掃描多種語言
- 獨立的報告和 artifacts
```

**功能**:
- 完整的 C# 和 JS/TS 掃描
- 自動建構 .NET 專案
- 為每種語言生成獨立報告
- 統計和視覺化摘要

### 掃描流程

```
觸發 → 檢出程式碼 → 設定環境 → 初始化 CodeQL → 建構 → 分析 → 生成報告 → 上傳
```

#### 詳細步驟

1. **初始化**: CodeQL 準備分析資料庫
2. **建構** (僅 C#): 執行 `dotnet restore` 和 `dotnet build`
3. **分析**: CodeQL 執行安全查詢
4. **報告生成**:
   - 創建 SARIF 檔案
   - 解析漏洞統計
   - 生成 Markdown 摘要
5. **上傳**:
   - 上傳到 GitHub Security
   - 保存為 Artifacts

## Pull Request 安全檢查

### `review.yml` 工作流程

每個 PR 自動執行以下檢查：

#### 1. 程式碼品質檢查
- .NET 建置驗證
- NuGet 套件授權檢查
- 相依性還原測試

#### 2. 安全性掃描
- 完整的 CodeQL 掃描（C# + JS/TS）
- 與 main 分支的變更比對
- 新增漏洞識別

#### 3. 自動化回饋
- PR 評論中的摘要報告
- 包含:
  - 程式碼品質狀態
  - 安全掃描狀態
  - 詳細報告連結
  - 修復建議

#### 範例 PR 評論

```markdown
## 🔍 PR 自動檢查報告

**檢查時間**: 2024-01-15 10:30:00
**PR**: #123 - 新增用戶認證功能
**分支**: `feature/auth` → `main`

### 📊 檢查結果

| 檢查項目 | 狀態 |
|---------|-----|
| 🏗️ 程式碼品質 | ✅ 通過 |
| 🔒 安全性掃描 | ✅ 通過 |

### 🔒 安全性掃描詳情

本次 PR 已通過 CodeQL 安全掃描...
```

## 查看和下載報告

### 方法 1: GitHub Actions Summary

1. 進入 "Actions" 頁面
2. 選擇一個 CodeQL workflow run
3. 查看 "Summary" 標籤頁
4. 查看漏洞統計表格和摘要

**範例輸出**:
```markdown
## 🔍 CodeQL csharp 安全掃描報告

### 📊 漏洞統計

| 嚴重程度 | 數量 | 圖示 |
|---------|------|------|
| Critical/Error | 0 | |
| High/Warning | 2 | 🟠🟠 |
| Medium/Note | 5 | 🟡🟡🟡🟡🟡 |
| **總計** | **7** | |
```

### 方法 2: 下載 SARIF Artifacts

1. 在 workflow run 頁面
2. 滾動到 "Artifacts" 區段
3. 下載 `codeql-<language>-vulnerability-report`
4. 解壓縮取得 `.sarif` 檔案

**SARIF 檔案用途**:
- 使用 VS Code SARIF Viewer 擴充功能查看
- 匯入到其他安全分析工具
- 進行趨勢分析和報告
- 歸檔和合規性記錄

### 方法 3: GitHub Security 頁面

1. 前往專案的 "Security" 頁籤
2. 選擇 "Code scanning alerts"
3. 查看所有已發現的漏洞清單
4. 點擊個別警報查看:
   - 漏洞描述
   - 受影響的程式碼位置
   - 嚴重程度評級
   - 修復建議
   - 相關 CWE/CVE 資訊

**功能**:
- 篩選和排序漏洞
- 追蹤修復狀態
- 查看歷史趨勢
- 匯出報告

## 修復安全漏洞

### 一般流程

1. **識別漏洞**
   - 查看 Security 頁面或 PR 評論
   - 了解漏洞類型和嚴重程度

2. **分析影響**
   - 檢查受影響的程式碼
   - 評估潛在風險
   - 確認是否為誤報

3. **實施修復**
   - 根據 CodeQL 建議修改程式碼
   - 使用安全的替代方案
   - 添加必要的驗證

4. **驗證修復**
   - 提交 PR 觸發新的掃描
   - 確認漏洞已解決
   - 檢查是否引入新問題

### 常見漏洞類型和修復

#### SQL 注入
```csharp
// ❌ 不安全
string query = "SELECT * FROM Users WHERE Id = " + userId;

// ✅ 安全
string query = "SELECT * FROM Users WHERE Id = @userId";
command.Parameters.AddWithValue("@userId", userId);
```

#### XSS (跨站腳本攻擊)
```csharp
// ❌ 不安全
return Content(userInput);

// ✅ 安全
return Content(HttpUtility.HtmlEncode(userInput));
```

#### 路徑遍歷
```csharp
// ❌ 不安全
string path = Path.Combine(basePath, userInput);

// ✅ 安全
string fullPath = Path.GetFullPath(Path.Combine(basePath, userInput));
if (!fullPath.StartsWith(basePath))
    throw new SecurityException("Invalid path");
```

### 標記誤報

如果某個警報是誤報：

1. 在 Security 頁面開啟該警報
2. 點擊 "Dismiss alert"
3. 選擇原因（如 "False positive"）
4. 添加註解說明為什麼是誤報

## 配置和自定義

### 修改掃描查詢集

在 workflow 檔案中調整 `queries` 參數：

```yaml
- name: 初始化 CodeQL
  uses: github/codeql-action/init@v4
  with:
    languages: ${{ matrix.language }}
    # 選項:
    # - security-extended: 更嚴格的安全檢查
    # - security-and-quality: 安全 + 程式碼品質（預設）
    # - security-only: 僅安全檢查
    queries: security-and-quality
```

### 調整掃描頻率

修改 `schedule` cron 表達式：

```yaml
schedule:
  # 每週一凌晨 2 點 (預設)
  - cron: '0 2 * * 1'
  
  # 每天凌晨 3 點
  # - cron: '0 3 * * *'
  
  # 每週三和週六
  # - cron: '0 2 * * 3,6'
```

### 自定義報告保留期限

調整 artifacts 保留天數：

```yaml
- name: 上傳漏洞報告 Artifact
  uses: actions/upload-artifact@v4
  with:
    name: codeql-report
    path: sarif-results/
    retention-days: 90  # 可調整為 1-90 天
```

### 添加自定義查詢

1. 創建 `.github/codeql/` 目錄
2. 添加自定義查詢檔案（`.ql`）
3. 在 workflow 中引用：

```yaml
- name: 初始化 CodeQL
  uses: github/codeql-action/init@v4
  with:
    languages: csharp
    queries: security-and-quality,+./.github/codeql/custom-queries.ql
```

### 設定嚴重程度閾值

如果想在發現 Critical 漏洞時失敗：

在 `generate-security-report.sh` 中取消註解：

```bash
# 如果有 critical 錯誤，返回非零狀態碼
if [ "$CRITICAL" -gt 0 ]; then
    echo "::error::發現 $CRITICAL 個 Critical 級別的安全漏洞"
    exit 1
fi
```

## 最佳實踐

### 開發流程

1. **提交前本地檢查**
   - 使用 IDE 的安全分析工具
   - 執行本地建置和測試

2. **創建 PR**
   - 等待自動安全掃描完成
   - 查看並修復任何發現的問題

3. **定期檢查**
   - 每週檢查 Security 頁面
   - 優先處理 High 和 Critical 漏洞

4. **保持更新**
   - 及時處理 Dependabot 的更新
   - 關注 CodeQL 的新查詢規則

### 團隊協作

1. **責任分配**
   - 指定安全負責人
   - 輪流審查安全警報

2. **知識分享**
   - 記錄常見漏洞和修復方法
   - 團隊內部安全培訓

3. **流程改進**
   - 定期回顧掃描結果
   - 優化查詢配置
   - 更新文檔

## 疑難排解

### 掃描失敗

**問題**: CodeQL 掃描失敗

**解決方案**:
1. 檢查 Actions 日誌查看具體錯誤
2. 確保專案能正常建置
3. 驗證 .NET SDK 版本匹配
4. 檢查網路連線和 GitHub Actions 狀態

### SARIF 檔案為空

**問題**: 下載的 SARIF 檔案沒有結果

**解決方案**:
1. 確認掃描真的執行了
2. 檢查語言偵測是否正確
3. 驗證 output 路徑設定

### 誤報過多

**問題**: 報告了大量誤報

**解決方案**:
1. 調整查詢集為 `security-only`
2. 使用自定義查詢過濾特定規則
3. 在 Security 頁面批量標記誤報

## 相關資源

- [CodeQL 官方文檔](https://codeql.github.com/docs/)
- [GitHub Code Scanning](https://docs.github.com/en/code-security/code-scanning)
- [SARIF 格式規範](https://sarifweb.azurewebsites.net/)
- [CWE 常見漏洞列表](https://cwe.mitre.org/)

## 支援

如有問題或建議，請：
- 創建 GitHub Issue
- 聯繫專案維護者
- 查閱 GitHub Actions 日誌
