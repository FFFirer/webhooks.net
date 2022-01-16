function Get-GitBranch {
    param (

        [string] $Directory = '',
        [string] $RepoUrl = '',
        [string] $Branch = 'master',
        [string] $Tag = '',
        [string] $Ref = ''
    )

    $HeadsRef = "refs/heads/"
    $RemotesRef = "refs/remotes/"
    $TagsRef = "refs/tags/"
    
    $OriginLocation = Get-Location
    Write-Output "当前目录：$OriginLocation"

    if ([String]::IsNullOrEmpty($Directory)) {
        Write-Output "路径为空"
        exit 1
    }

    if ([string]::IsNullOrEmpty($Branch)) {
        Write-Output "分支为空，默认指定[master]"
        $Branch = 'master'
    }

    $ExistFolder = Test-Path $Directory
    if (-not $ExistFolder) {
        # 文件夹不存在
        Write-Output "不存在路径：$Directory"
        exit 1
    }

    # 解析要拉取的目标，如果存在$Ref，则根据Ref来解析
    if (-not $Ref.StartsWith("refs/")) {
        Write-Error "ref格式不正确 -> $Ref" 
        exit 1;
    }
    else {
        if ($Ref.StartsWith($HeadsRef)) {
            # 分支
            $Branch = $Ref.Substring($HeadsRef.Length)
            Write-Output "匹配到分支：$Branch"
        }
        elseif ($Ref.StartsWith($RemotesRef.Length)) {
            # 远程分支
            $Branch = $Ref.Substring($RemotesRef.Length).Split("/")[1]
            Write-Output "匹配到远程分支：$Branch"
        }
        elseif ($Ref.StartsWith($TagsRef)) {
            # tag
            $Tag = $Ref.Substring($TagsRef.Length)
            Write-Output "匹配到标签：$Tag"
        }
        else {
            Write-Error "错误的ref格式 -> $Ref"
            exit 1 
        }
    }

    Set-Location $Directory
    Write-Output "进入目录：$Directory"

    $GitFolder = Join-Path $Directory -ChildPath ".git";
    $ExistsGitFolder = Test-Path $GitFolder
    
    if (-not $ExistsGitFolder) {
        # Git文件夹不存在
        if ([String]::IsNullOrEmpty($RepoUrl)) {
            Write-Output "没有提供仓库拉取地址！"
            exit 3
        }

        try {
            
            git clone -b $Branch $RepoUrl .
        }
        catch {
            Write-Output "拉取仓库 $RepoUrl 分支 $Branch 失败"
            exit 2
        }
    }
    else {
        git checkout .  # 清理当前工作区更改

        $LocalBranchs = git branch
        $ExistsBranch = $false  # 分支在本地已存在
        
        if ($LocalBranchs.GetType() -eq [String]) {
            # 单条记录
            if ($LocalBranchs.Contains($Branch)) {
                Write-Output "匹配到分支: $LocalBranchs[i]"
                $ExistsBranch = $truec
            }
        }
        else {
            for ($i = 0; $i -lt $LocalBranchs.Count; $i++) {
                $currentBranch = $LocalBranchs[$i]

                Write-Output "检查分支: $currentBranch"
    
                if ($currentBranch.Contains($Branch)) {
                    Write-Output "匹配到分支: $currentBranch"
                    $ExistsBranch = $true
                    break
                }
            }
        }

        if (-not $ExistsBranch) {

            git fetch origin
            # 切换分支
            git checkout $Branch
        }
        else {
            git checkout $Branch
        }
    }

    if ([String]::IsNullOrEmpty($Tag)) {
        Write-Output "未指定Tag, 拉取最新代码"
        # 不指定标签
        git pull

        Write-Output "拉取完成！"
    }
    else {
        git pull

        Write-Output "拉取完成！"

        # 签出指定标签
        git checkout $Tag
        Write-Output "签出分支: $Tag"
    }

    # 返回原目录
    Write-Output "返回目录: $OriginLocation"
    Set-Location $OriginLocation    

    <#
        .SYNOPIS
        在指定的目录拉取指定分支的git仓库，如果指定标签，则签出指定标签的记录，否则是最新的代码

        .DESCRIPTION
        检查并拉取git分支

        .PARAMETER Directory
        文件夹

        .PARAMETER RepoUrl
        Git仓库远程地址

        .PARAMETER Branch
        Git分支名字

        .PARAMETER Tag
        要拉取的标签

        .REMARKS
        exit-code
        1-文件夹或路径不存在
        3-RepoUrl 没有提供
    #>

}
Export-ModuleMember -Function Get-GitBranch