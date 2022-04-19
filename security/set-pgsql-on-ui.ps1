$OriginalPath = $PWD.Path
$ProjectPath = "../WebHooks.UI"
$ConnectionString = "Host=127.0.0.1;Database=webhooks;Username=postgres;Password=123456;Encoding=UTF8;"
$ConfigKey = "ConnectionStrings:Default"

$SetUserSecretsCommand = "dotnet user-secrets set '$ConfigKey' '$ConnectionString'"
$ListUserSecretsCommand = "dotnet user-secrets list"

Write-Output "1) 进入项目路径：$ProjectPath"
Set-Location -Path $ProjectPath

Write-Output "2) 已进入：$PWD"

try { 
    Write-Output "3) 设置用户机密命令：$SetUserSecretsCommand"
    Invoke-Expression -Command $SetUserSecretsCommand -ErrorAction Stop

    Write-Output "4) 检查已设置用户机密：$ListUserSecretsCommand"
    Invoke-Expression -Command $ListUserSecretsCommand -ErrorAction Stop
}
finally {
    Set-Location $OriginalPath
}

Exit-PSHostProcess