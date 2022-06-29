[CmdletBinding()]
param (
    [Parameter()]
    [string]
    $ApiUrl = "/"
)


$CurrentLocation = (Get-Item ./ -Verbose).FullName;

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

}
finally {
    
    Set-Location $CurrentLocation
}
