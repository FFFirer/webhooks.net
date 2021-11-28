$InstallPath = "/var/www/webhooks-gitee"

# 首先编译发布项目
dotnet publish "./WebHooks.Gitee.csproj" -c Release -o publish

$ExistsInstallPath = Test-Path $InstallPath

if (-not $ExistsInstallPath) {
    Write-Output "没有找到安装目录：$InstallPath"
}

Copy-Item -Path "./publish/*" -Destination $InstallPath -Recurse

Write-Output "开始修改权限"
chown -R www-data:www-data $InstallPath
Write-Output "修改 $InstallPath 所有者为 www-data:www-data"

# 服务安装文件
$InstallServicePath = "./webhooks.gitee.service"

Write-Output "复制service文件到systemd路径[/etc/systemd/system/]下"
Copy-Item $InstallServicePath -Destination "/etc/systemd/system/" -ErrorAction Stop

Write-Output "文件复制完成"


