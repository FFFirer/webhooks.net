Write-Debug "[Migrator]===================START==================="

$CurrentLocation = $PWD.Path

$MigratorOutputs = Join-Path $CurrentLocation "outputs/migrator"

Write-Debug "[Migrator][输出路径] $($MigratorOutputs)"

Remove-Item $MigratorOutputs -Recurse -ErrorAction Ignore
Write-Debug "[Migrator][清空输出目录]"

New-Item -Path $MigratorOutputs -ItemType Directory
Write-Debug "[Migrator][新建输出目录]"

try {
    $ProjectFolder = Join-Path $CurrentLocation "../server/src/WebHooks.EFCore.Migrator"
    Set-Location $ProjectFolder
    Write-Debug "[Migrator][当前目录] $($PWD.Path)" 

    Write-Debug "[Migrator][开始发布] 到 $($MigratorOutputs)"
    dotnet publish "./WebHooks.EFCore.Migrator.csproj" -c Release -o $MigratorOutputs
    Write-Debug "[Migrator][发布结束]"
}
finally {
    Write-Debug "[Migrator]===================END==================="
    Set-Location $CurrentLocation
}
