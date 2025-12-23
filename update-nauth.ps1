# Update NAuth libraries
Write-Host "Navigating to NAuth directory..." -ForegroundColor Cyan
Set-Location -Path "..\NAuth"
Get-Location

Write-Host "Building NAuth solution..." -ForegroundColor Yellow
dotnet build -c Release NAuth.sln

if ($LASTEXITCODE -ne 0) {
    Write-Host "Error building NAuth solution!" -ForegroundColor Red
    Set-Location -Path "..\NNews"
    exit 1
}

Write-Host "Navigating to build output..." -ForegroundColor Cyan
Set-Location -Path ".\NAuth.ACL\bin\Release\net8.0"
Get-Location

Write-Host "Copying DLL files to NNews/Lib..." -ForegroundColor Green
Copy-Item -Path "NAuth.ACL.dll" -Destination "..\..\..\..\..\NNews\Lib" -Force
Copy-Item -Path "NAuth.DTO.dll" -Destination "..\..\..\..\..\NNews\Lib" -Force

Write-Host "NAuth libraries updated successfully!" -ForegroundColor Green

# Return to NNews directory
Set-Location -Path "..\..\..\..\..\NNews"

Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
