$VerbosePreference = "Continue"
$DebugPreference = "Continue"

$InstallPath = "/var/www/webhooks-gitee"

# 首先编译发布项目
try {
    dotnet publish "./WebHooks.Gitee.csproj" -c Release -o publish
}
catch {
    Write-Warning "发布失败，$_"
    exit 1
}

$ExistsInstallPath = Test-Path $InstallPath

if (-not $ExistsInstallPath) {
    Write-Warning "没有找到安装目录：$InstallPath"
    exit 1
}

Copy-Item -Path "./publish" -Destination $InstallPath -Recurse -ErrorAction Stop

Write-Debug "开始修改权限"
chown -R www-data:www-data $InstallPath
Write-Debug "修改 $InstallPath 所有者为 www-data:www-data"

# 服务安装文件
$InstallServicePath = "./webhooks.gitee.service"

Write-Debug "复制service文件到systemd路径[/etc/systemd/system/]下"
Copy-Item $InstallServicePath -Destination "/etc/systemd/system/" -ErrorAction Stop

Write-Debug "文件复制完成"
exit 0


