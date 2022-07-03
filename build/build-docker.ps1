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

Write-Debug "[build-docker.ps1]=====================START========================"
Write-Debug "[build-docker.ps1][输入参数] BaseUrl       : $($BaseUrl)"
Write-Debug "[build-docker.ps1][输入参数] ImageName     : $($ImageName)"
Write-Debug "[build-docker.ps1][输入参数] Version       : $($Version)"
Write-Debug "[build-docker.ps1][输入参数] NoBuild       : $($NoBuild)"

$CurrentDirectory = $PWD.Path
Write-Debug "[build-docker.ps1][当前目录] ${$CurrentDirectory}"

if (-not $NoBuild) {
    ./build.ps1 -ApiUrl $BaseUrl
}

if (-not ($? -eq $true)) {
    Write-Output "发布失败: $($LASTEXITCODE)"
    Exit $LASTEXITCODE
}

# 构建docker镜像
try {
    # 检查构建环境
    ./check-build-environment.ps1

    if (-not ($? -eq $true)) {
        Write-Output "[build-docker.ps1][检查docker镜像构建环境] 检查失败"
        Exit $LASTEXITCODE
    }

    $ClientDockerfile = Join-Path $CurrentDirectory "./client/Dockerfile"
    
    Write-Debug "[build-docker.ps1][客户端Dockerfile] $($ClientDockerfile)"

    Copy-Item $ClientDockerfile (Join-Path $CurrentDirectory "outputs/client")

    $ServerDockerfile = Join-Path $CurrentDirectory "./server/Dockerfile"

    Write-Debug "[build-docker.ps1][服务器端Dockerfile] $($ServerDockerfile)"

    Copy-Item $ServerDockerfile (Join-Path $CurrentDirectory "outputs/server")

    # 构建服务器镜像
    Set-Location (Join-Path $CurrentDirectory "outputs/server")

    $ServerImageTag = "$($ImageName).api:$($Version)"

    Write-Debug "[build-docker.ps1][服务器端镜像名称] $($ServerImageTag)"

    docker rmi $ServerImageTag -f
    docker build -t $ServerImageTag .
    docker tag $ServerImageTag "$($ImageName).api:latest"

    if (-not ($? -eq $true)) {
        Write-Output "[build-docker.ps1][server image 构建失败]"
        Exit $LASTEXITCODE
    }

    Write-Debug "[build-docker.ps1][服务器端] 镜像构建完成"

    # 构建前端镜像
    Set-Location (Join-Path $CurrentDirectory "outputs/client")

    $ClientImageTag = "$($ImageName).ui:$($Version)"
    Write-Debug "[build-docker.ps1][客户端镜像名称] $($ClientImageTag)"

    docker rmi $ClientImageTag -f
    docker build -t $ClientImageTag .
    docker tag $ClientImageTag "$($ImageName).ui:latest"

    if (-not ($? -eq $true)) {
        Write-Output "[build-docker.ps1][client image 构建失败]"
        Exit $LASTEXITCODE
    }

    Write-Debug "[build-docker.ps1][客户端] 镜像构建完成"
    Write-Debug "[build-docker.ps1][准备构建Migrator]"

    # 构建Migrator镜像，已构建，无需构建
    Set-Location $CurrentDirectory
    ./build-migrator-docker.ps1 -ImageName $ImageName -Version $Version -NoBuild
}
finally {
    Write-Debug "[build-docker.ps1]=====================END========================"
    Set-Location $CurrentDirectory
}