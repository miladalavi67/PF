<#
  اجرای محلی سیستم جامع امکان تولید (Backend روی 5250 و Frontend روی 5005).
  رمز و رشته اتصال را داخل مخزن نمی‌گذاریم؛ در user-secrets نگهداری می‌شود:

    cd backend\hosts
    dotnet user-secrets set "ConnectionStrings:Default" "<رشته اتصال>"
    dotnet user-secrets set "Oidc:EmbeddedSigningKey" "<حداقل ۳۲ کاراکتر تصادفی>"
    dotnet user-secrets set "ProductionFeasibility:Development:UserName" "<شماره پرسنلی برای اجرای مستقل>"   # اختیاری
    dotnet user-secrets set "ProductionFeasibility:Sap:WhereUsedUrl" "<آدرس سرویس SAP>"                    # اختیاری
    dotnet user-secrets set "ProductionFeasibility:Sap:UserName" "<...>"                                      # اختیاری
    dotnet user-secrets set "ProductionFeasibility:Sap:Password" "<...>"                                      # اختیاری
#>
param([switch]$SkipInstall)

$root = Split-Path -Parent $MyInvocation.MyCommand.Path

if (-not $SkipInstall -and -not (Test-Path (Join-Path $root 'frontend\node_modules'))) {
    Push-Location (Join-Path $root 'frontend')
    npm install
    Pop-Location
}

$cfg = Join-Path $root 'backend\nuget.config'
Start-Process powershell -ArgumentList '-NoExit', '-Command', "Set-Location '$root\backend\hosts'; `$env:ASPNETCORE_ENVIRONMENT='Development'; dotnet run --launch-profile IKCO.ProductionFeasibility.Host"
Start-Process powershell -ArgumentList '-NoExit', '-Command', "Set-Location '$root\frontend'; npm run dev"

Write-Host 'Backend:  http://localhost:5250/swagger'
Write-Host 'Frontend: http://localhost:5005'
