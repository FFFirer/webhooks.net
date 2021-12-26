# WebHooks.NET

#### 介绍

实现 Gitee WebHook 的接收方，未来也会考虑添加其他平台的 WebHook。
命令行执行依赖 PowerShell，推荐安装 PowerShell 7.0+ 或 PowerShell Core。

#### 软件架构

使用.NET 6，分层结构  
执行 PowerShell 明令使用的 PowerShell SDK

#### 安装教程

> 本软件现基于.NET6编写，所以运行环境最好是.NET6的。
> 本软件命令行部分均使用**PowerShell**实现，所以需要安装PowerShell环境，推荐安装PowerShell Core，请根据官方安装文档操作。

> PowerShell 安装：[在 Windows、Linux 和 macOS 上安装 PowerShell](https://docs.microsoft.com/zh-cn/powershell/scripting/install/installing-powershell?view=powershell-7.2)

> PS: Linux部署时，systemd默认使用的用户是**www-data**，但实际使用时，改用户的权限不足以完成所有操作，当提示权限不足时请自行修改用户

> PSS: 以下脚本仅限Linux部署，Nginx+systemd

```sh
# 全新安装
# bash
bash ./install.sh
# powershell
pwsh ./install.ps1

# 仅更新
# bash
bash ./update.sh
# powershell
pwsh ./install.ps1 --update

# ps: 所有*.sh脚本也都是调用的pwsh
```

#### 使用说明

**配置 webhooks.json**
配置节`Gitee`表示是 Gitee 平台的 WebHook，`test`区分每次的请求，对标请求连接的`/api/webhooks/Gitee/push/{repoKey}`中的 repoKey；Secret 表示 Gitee WebHook 密码或私钥；Branch 表示要操作的分支；Events 表示要触发的事件，Steps 表示一个个步骤，Scripts 表示执行具体脚本（PowerShell）

```json
{
  "Gitee": {
    "test": {
      "Secret": "123456",
      "Branch": "master",
      "Events": {
        "Push Hook": [
          {
            "Scripts": [
              "Write-Output 'Hello world!'"
            ]
          }
        ]
      }
    }
  }
}
```


#### 敬请期待

基础功能已开发完成，功能验证完成也已完成，接下来将优化使用体验，以及考虑加上手动触发，图形界面配置功能等。


#### 参与贡献

[gitee/fffirer](https://gitee.com/fffirer)