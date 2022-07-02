[CmdletBinding()]
param (
    # 镜像名称
    [Parameter()]
    [string]
    $ImageName,

    [Parameter()]
    [string]
    $Version,

    # 不做构建
    [Parameter()]
    [switch]
    [bool]
    $NoBuild    
)

Write-Debug "[build-migrator-docker.ps1]==================START================="
Write-Debug "[build-migrator-docker.ps1][输入参数] ImageName  : $($ImageName)"
Write-Debug "[build-migrator-docker.ps1][输入参数] Version    : $($Version)"
Write-Debug "[build-migrator-docker.ps1][输入参数] NoBuild    : $($NoBuild)"

$CurrentLocation = $PWD.Path

Write-Debug "[build-migrator-docker.ps1][当前目录] $($CurrentLocation)"

if (-not $NoBuild) {
    ./build-migrator.ps1
}

if (-not ($? -eq $true)) {
    Write-Output "发布Migrator失败: $($LASTEXITCODE)"
    Exit $LASTEXITCODE
}

try {
    $Dockerfile = Join-Path $CurrentLocation "./migrator/Dockerfile"

    Copy-Item $Dockerfile (Join-Path $CurrentLocation "./outputs/migrator")

    Write-Debug "[build-migrator-docker.ps1][复制Dockerfile] $((Get-Item $Dockerfile -Verbose).FullName) -> $(Join-Path $CurrentLocation "./outputs/migrator")"

    Set-Location (Join-Path $CurrentLocation "./outputs/migrator")

    $MigratorImageName = "$($ImageName).migrator:$($Version)"
    $LatestMigratorImageName = "$($ImageName).migrator:latest"

    Write-Debug "[build-migrator-docker.ps1][生成镜像] $($MigratorImageName)"

    docker rmi $MigratorImageName -f
    docker build -t $MigratorImageName .
    docker tag $MigratorImageName $LatestMigratorImageName
}
finally {
    Write-Debug "[build-migrator-docker.ps1]==================END================="
    Set-Location $CurrentLocation
}