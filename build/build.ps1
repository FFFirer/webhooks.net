[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $ApiUrl = "/"
)


Write-Debug "[Build]=====================START====================="

Write-Debug "[Build][输入参数] ApiUrl : $($ApiUrl)"

$CurrentLocation = (Get-Item ./ -Verbose).FullName;

Write-Debug "[Build][当前目录] $((Get-Item $CurrentLocation -Verbose).FullName)"

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
    Write-Debug "[Build][WebHooks.API][发布结束] -> $((Get-Item $ServerOutputs -Verbose).FullName)"

    # 发布Client
    Set-Location (Join-Path $CurrentLocation "../client/webhooks-ui")

    $EnvProdLocalFile = Join-Path $CurrentLocation "../client/webhooks-ui/.env.prod.local"

    Remove-Item -Path $EnvProdLocalFile -ErrorAction Ignore

    New-Item -Path $EnvProdLocalFile -ItemType File

    $EnvProdExampleFileContent = Get-Content (Join-Path $CurrentLocation "./client/.env.prod.example")

    # 替换配置
    $EnvProdLocalFileContent = $EnvProdExampleFileContent -replace "@API_URL", $ApiUrl

    Set-Content -Path $EnvProdLocalFile -Value $EnvProdLocalFileContent -Encoding utf8

    yarn run build-prod

    Copy-Item (Join-Path $CurrentLocation "../client/webhooks-ui/dist/*") $ClientOutputs -Recurse

    Write-Debug "[Build][webhooks-ui][发布结束] -> $((Get-Item $ClientOutputs -Verbose).FullName)"

    # 构建Migrator
    Set-Location $CurrentLocation
    Write-Debug "[Build][当前目录] $($CurrentLocation)"
    ./build-migrator.ps1
}
finally {
    
    Write-Debug "[Build]=====================END====================="
    Set-Location $CurrentLocation
}
