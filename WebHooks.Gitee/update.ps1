$servicePath = "/etc/systemd/system/webhooks.gitee.service"

$ExistsServiceFilePath = Test-Path $servicePath

if (-not $ExistsServiceFilePath) {
    Write-Error "未安装服务！";
    exit 1
}

