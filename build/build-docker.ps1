[CmdletBinding()]
param (
    # 基础地址
    [Parameter()]
    [string]
    $BaseUrl,

    # 镜像基础名称
    [Parameter()]
    [string]
    $ImageName,

    # 版本
    [Parameter()]
    [string]
    $Version,

    # 无需构建
    [Parameter()]
    [switch]
    [bool]
    $NoBuild
)

$CurrentDirectory = $PWD.Path

if (-not $NoBuild) {
    ./build.ps1 -ApiUrl $BaseUrl
}

if (-not ($LASTEXITCODE -eq 0)) {
    Write-Output "发布失败: $($LASTEXITCODE)"
    Exit $LASTEXITCODE
}

# 构建docker镜像
try {
    $ClientDockerfile = Join-Path $CurrentDirectory "./client/Dockerfile"

    Copy-Item $ClientDockerfile (Join-Path $CurrentDirectory "outputs/client")

    $ServerDockerfile = Join-Path $CurrentDirectory "./server/Dockerfile"

    Copy-Item $ServerDockerfile (Join-Path $CurrentDirectory "outputs/server")

    # 构建服务器镜像
    Set-Location (Join-Path $CurrentDirectory "outputs/server")

    $ServerImageTag = "$($ImageName).api:$($Version)"

    docker rmi $ServerImageTag -f
    docker build -t $ServerImageTag .
    docker tag $ServerImageTag "$($ImageName).api:latest"

    # 构建前端镜像
    Set-Location (Join-Path $CurrentDirectory "outputs/client")

    $ClientImageTag = "$($ImageName).ui:$($Version)"

    docker rmi $ClientImageTag -f
    docker build -t $ClientImageTag .
    docker tag $ClientImageTag "$($ImageName).ui:latest"
}
finally {
    Set-Location $CurrentDirectory
}