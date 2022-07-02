Write-Debug "[build-migrator.ps1]===================START==================="

$CurrentLocation = $PWD.Path

$MigratorOutputs = Join-Path $CurrentLocation "outputs/migrator"

Write-Debug "[build-migrator.ps1][输出路径] $($MigratorOutputs)"

Remove-Item $MigratorOutputs -Recurse -ErrorAction Ignore
Write-Debug "[build-migrator.ps1][清空输出目录]"

New-Item -Path $MigratorOutputs -ItemType Directory
Write-Debug "[build-migrator.ps1][新建输出目录]"

try {
    $ProjectFolder = Join-Path $CurrentLocation "../server/src/WebHooks.EFCore.Migrator"
    Set-Location $ProjectFolder
    Write-Debug "[build-migrator.ps1][当前目录] $($PWD.Path)" 

    Write-Debug "[build-migrator.ps1][开始发布] 到 $($MigratorOutputs)"
    dotnet publish "./WebHooks.EFCore.Migrator.csproj" -c Release -o $MigratorOutputs
    Write-Debug "[build-migrator.ps1][发布结束]"
}
finally {
    Write-Debug "[build-migrator.ps1]===================END==================="
    Set-Location $CurrentLocation
}
