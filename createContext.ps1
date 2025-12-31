# Connection string for NNews database
$connectionString = "Host=localhost;Port=5433;Database=nnews_db;Username=nnews_user;Password=pikpro6"

Write-Host "Using connection string" -ForegroundColor Green
Write-Host "Connection: $connectionString" -ForegroundColor Cyan

# Navigate to NNews.Infra directory
Set-Location -Path ".\NNews.Infra"

# Run dotnet ef dbcontext scaffold
Write-Host "Scaffolding database context..." -ForegroundColor Yellow
dotnet ef dbcontext scaffold "$connectionString" Npgsql.EntityFrameworkCore.PostgreSQL --context NNewsContext --output-dir Context -f

if ($LASTEXITCODE -eq 0) {
    Write-Host "Database context scaffolded successfully!" -ForegroundColor Green
} else {
    Write-Host "Error scaffolding database context!" -ForegroundColor Red
}

# Return to original directory
Set-Location -Path ".."

Write-Host "Press any key to continue..."
$null = $Host.UI.RawUI.ReadKey("NoEcho,IncludeKeyDown")
