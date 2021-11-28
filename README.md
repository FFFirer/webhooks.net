# WebHooks.NET

#### 介绍

实现 Gitee WebHook 的接收方，未来也会考虑添加其他平台的 WebHook。
命令行执行依赖 PowerShell，推荐安装 PowerShell 7.0+ 或 PowerShell Core。

#### 软件架构

使用.NET 6，分层结构  
执行 PowerShell 明令使用的 PowerShell SDK

#### 安装教程

```sh
# bash
bash ./install.sh
# powershell
pwsh ./install.ps1
```

#### 使用说明

配置 webhooks.json
配置节`Gitee`表示是 Gitee 平台的 WebHook，`test`区分每次的请求，对标请求连接的`/api/webhooks/Gitee/push/{repoKey}`中的 repoKey；Secret 表示 Gitee WebHook 密码或私钥；Branch 表示要操作的分支；Steps 表示要操作的多个步骤，Scripts 表示执行具体脚本（PowerShell）

```json
{
  "Gitee": {
    "test": {
      "Secret": "123456",
      "Branch": "master",
      "HookId": "1234567",
      "Steps": [
        {
          "Scripts": ["Write-Output \"Hello world!\""]
        }
      ]
    }
  }
}
```

#### 参与贡献

开发中
