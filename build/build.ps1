[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $ApiUrl = "/"
)


Write-Debug "[build.ps1]=====================START====================="

Write-Debug "[build.ps1][输入参数] ApiUrl : $($ApiUrl)"

$CurrentLocation = (Get-Item ./ -Verbose).FullName;

Write-Debug "[build.ps1][当前目录] $((Get-Item $CurrentLocation -Verbose).FullName)"

$Outputs = Join-Path $CurrentLocation "outputs"

Remove-Item $Outputs -Recurse -ErrorAction Ignore

New-Item -Path $Outputs -ItemType Directory

$ServerOutputs = Join-Path $outputs "server"
$ClientOutputs = Join-Path $outputs "client"

New-Item -Path $ServerOutputs -ItemType Directory
New-Item -Path $ClientOutputs -ItemType Directory

try {
    
    # 发布Server
    Set-Location (Join-Path $CurrentLocation "../server/src/WebHooks.API")

    dotnet publish "./WebHooks.API.csproj" -c Release -o $ServerOutputs

    if (-not ($? -eq $true)) {
        Write-Output "[build.ps1][client] 构建失败"
        Exit $LASTEXITCODE
    }

    Write-Debug "[build.ps1][WebHooks.API][发布结束] -> $((Get-Item $ServerOutputs -Verbose).FullName)"

    # 发布Client
    Set-Location (Join-Path $CurrentLocation "../client/webhooks-ui")

    $EnvProdLocalFile = Join-Path $CurrentLocation "../client/webhooks-ui/.env.prod.local"

    Remove-Item -Path $EnvProdLocalFile -Force -ErrorAction Ignore

    New-Item -Path $EnvProdLocalFile -ItemType File

    $EnvProdExampleFileContent = Get-Content (Join-Path $CurrentLocation "./client/.env.prod.example")

    # 替换配置
    $EnvProdLocalFileContent = $EnvProdExampleFileContent -replace "@API_URL", $ApiUrl

    Set-Content -Path $EnvProdLocalFile -Value $EnvProdLocalFileContent -Encoding utf8

    # 还原
    yarn

    yarn run build-prod

    Copy-Item (Join-Path $CurrentLocation "../client/webhooks-ui/dist/*") $ClientOutputs -Recurse

    if (-not ($? -eq $true)) {
        Write-Output "[build.ps1][client] 构建失败"
        Exit $LASTEXITCODE
    }

    Write-Debug "[build.ps1][webhooks-ui][发布结束] -> $((Get-Item $ClientOutputs -Verbose).FullName)"

    # 构建Migrator
    Set-Location $CurrentLocation
    Write-Debug "[build.ps1][当前目录] $($CurrentLocation)"
    ./build-migrator.ps1
}
finally {
    
    Write-Debug "[build.ps1]=====================END====================="
    Set-Location $CurrentLocation
}
