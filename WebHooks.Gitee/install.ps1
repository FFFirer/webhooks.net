[CmdletBinding()]
param (
    [Parameter()]
    [switch]
    $Update
)

$VerbosePreference = "Continue"
$DebugPreference = "Continue"

$InstallPath = "/var/www/webhooks-gitee"

# 首先编译发布项目


$ExistsInstallPath = Test-Path $InstallPath

if (-not $ExistsInstallPath) {
    Write-Warning "没有找到安装目录：$InstallPath"
    
    New-Item -Path $InstallPath -ItemType Directory

    Write-Debug "新建文件夹：$InstallPath"

    Write-Debug "开始修改权限"
    chown -R www-data:www-data $InstallPath
    Write-Debug "修改 $InstallPath 所有者为 www-data:www-data"
}

# 服务安装文件

$InstallServiceFolder = "/etc/systemd/system"
$InstallServiceFile = "webhooks.gitee.service"

$InstallServiceTargetPath = Join-Path -Path $InstallServiceFolder -ChildPath $InstallServiceFile

Write-Debug "systemd配置文件路径: $InstallServiceTargetPath"

$ExistsServiceFile = Test-Path $InstallServiceTargetPath

if (-not $ExistsServiceFile) {
    Write-Debug "没有发现systemd配置文件"
    Write-Debug "复制service文件到systemd路径[/etc/systemd/system/]下"

    Copy-Item $InstallServiceFile -Destination "/etc/systemd/system/" -ErrorAction Stop
}
else {
    Write-Debug "已存在systemd配置文件"

    Write-Debug "停止systemd服务 $InstallServiceFile"
    systemctl stop $InstallServiceFile
}

# 拷贝文件
try {
    Write-Debug "开始发布"
    dotnet publish "./WebHooks.Gitee.csproj" -c Release -o publish
}
catch {
    Write-Warning "发布失败，$_"
    exit 1
}

if ($Update) {
    $ExcludeFiles = @("appsetting.*.json", "webhooks.json", "NLog.config")

    Write-Debug "更新文件，排除文件：$ExcludeFiles"
    Get-ChildItem  -Path "./publish/*" -Recurse -Exclude $ExcludeFiles | Copy-Item -Destination $InstallPath -Force -Recurse  -ErrorAction Stop
    Write-Debug "文件复制完成"
}
else {
    Copy-Item -Path "./publish/*" -Destination $InstallPath -Force -Recurse  -ErrorAction Stop
}


Write-Debug "启动systemd服务 $InstallServiceFile"
systemctl start $InstallServiceFile
systemctl status $InstallServiceFile

exit 0


