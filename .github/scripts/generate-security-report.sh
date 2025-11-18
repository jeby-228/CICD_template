#!/bin/bash
# 生成 CodeQL 安全掃描報告
# 使用方式: ./generate-security-report.sh <sarif-file> <language>

set -e

SARIF_FILE="${1}"
LANGUAGE="${2:-unknown}"

if [ ! -f "$SARIF_FILE" ]; then
    echo "❌ 錯誤: 找不到 SARIF 檔案 $SARIF_FILE"
    exit 1
fi

echo "## 🔍 CodeQL ${LANGUAGE} 安全掃描報告" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY
echo "**掃描時間**: $(date '+%Y-%m-%d %H:%M:%S')" >> $GITHUB_STEP_SUMMARY
echo "**語言**: ${LANGUAGE}" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY

# 解析 SARIF 檔案統計漏洞
CRITICAL=$(jq '[.runs[].results[] | select(.level=="error")] | length' "$SARIF_FILE")
HIGH=$(jq '[.runs[].results[] | select(.level=="warning")] | length' "$SARIF_FILE")
MEDIUM=$(jq '[.runs[].results[] | select(.level=="note")] | length' "$SARIF_FILE")
TOTAL=$(jq '[.runs[].results[]] | length' "$SARIF_FILE")

echo "### 📊 漏洞統計" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY
echo "| 嚴重程度 | 數量 | 圖示 |" >> $GITHUB_STEP_SUMMARY
echo "|---------|------|------|" >> $GITHUB_STEP_SUMMARY
echo "| Critical/Error | $CRITICAL | $(printf '🔴%.0s' $(seq 1 $CRITICAL)) |" >> $GITHUB_STEP_SUMMARY
echo "| High/Warning | $HIGH | $(printf '🟠%.0s' $(seq 1 $HIGH)) |" >> $GITHUB_STEP_SUMMARY
echo "| Medium/Note | $MEDIUM | $(printf '🟡%.0s' $(seq 1 $MEDIUM)) |" >> $GITHUB_STEP_SUMMARY
echo "| **總計** | **$TOTAL** | |" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY

if [ "$TOTAL" -eq 0 ]; then
    echo "### ✅ 掃描結果" >> $GITHUB_STEP_SUMMARY
    echo "" >> $GITHUB_STEP_SUMMARY
    echo "🎉 **未發現安全漏洞！程式碼品質良好。**" >> $GITHUB_STEP_SUMMARY
else
    echo "### ⚠️ 發現潛在問題" >> $GITHUB_STEP_SUMMARY
    echo "" >> $GITHUB_STEP_SUMMARY
    echo "本次掃描發現 **$TOTAL** 個潛在的安全問題。請仔細檢查以下類別的問題：" >> $GITHUB_STEP_SUMMARY
    echo "" >> $GITHUB_STEP_SUMMARY
    
    # 列出前 5 個問題
    echo "#### 🔎 主要問題摘要（前 5 項）" >> $GITHUB_STEP_SUMMARY
    echo "" >> $GITHUB_STEP_SUMMARY
    
    jq -r '.runs[].results[0:5] | .[] | 
        "- **\(.ruleId)**: \(.message.text)\n  - 位置: \(.locations[0].physicalLocation.artifactLocation.uri):\(.locations[0].physicalLocation.region.startLine)\n  - 嚴重程度: \(.level)"' \
        "$SARIF_FILE" >> $GITHUB_STEP_SUMMARY || echo "無法解析問題詳情" >> $GITHUB_STEP_SUMMARY
    
    echo "" >> $GITHUB_STEP_SUMMARY
fi

echo "" >> $GITHUB_STEP_SUMMARY
echo "### 🔗 後續步驟" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY
echo "1. 📥 [下載完整 SARIF 報告](../actions/runs/${GITHUB_RUN_ID}) 從 Artifacts" >> $GITHUB_STEP_SUMMARY
echo "2. 🔒 前往 [Security > Code scanning alerts](../security/code-scanning) 查看詳細漏洞資訊" >> $GITHUB_STEP_SUMMARY
echo "3. 📖 參考 [CodeQL 文檔](https://codeql.github.com/docs/) 了解如何修復特定問題" >> $GITHUB_STEP_SUMMARY
echo "" >> $GITHUB_STEP_SUMMARY

# 輸出環境變數供其他步驟使用
echo "SECURITY_SCAN_TOTAL=$TOTAL" >> $GITHUB_OUTPUT
echo "SECURITY_SCAN_CRITICAL=$CRITICAL" >> $GITHUB_OUTPUT
echo "SECURITY_SCAN_HIGH=$HIGH" >> $GITHUB_OUTPUT
echo "SECURITY_SCAN_MEDIUM=$MEDIUM" >> $GITHUB_OUTPUT

# 如果有 critical 錯誤，返回非零狀態碼（可選）
# if [ "$CRITICAL" -gt 0 ]; then
#     echo "::error::發現 $CRITICAL 個 Critical 級別的安全漏洞"
#     exit 1
# fi

exit 0
