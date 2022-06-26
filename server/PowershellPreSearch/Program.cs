// See https://aka.ms/new-console-template for more information
using PowershellPreSearch;
using System.Globalization;
using System.Management.Automation;
using System.Management.Automation.Host;
using System.Management.Automation.Runspaces;
using System.Runtime.InteropServices;

Console.WriteLine("Start use powershell!");

//var script = File.ReadAllText("D:\\Playground\\repos\\LittleBlog\\build.ps1");

var runOption = new RunPowershellOption()
{
    WorkingDirectory = "D:/Playground/repos/LittleBlog",
    Scripts = new List<string>()
    {
        "& {./build.ps1 -mode prod -build_docker 0 -api_address / -admin_prefix /admin/}"
    }
};

DirectoryInfo dirInfo = new DirectoryInfo(runOption.WorkingDirectory);

if (!dirInfo.Exists)
{
    throw new DirectoryNotFoundException(runOption.WorkingDirectory);
}

var initialState = InitialSessionState.CreateDefault();

if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
    // 判断windows环境
    // 如果是windows需要添加执行脚本权限
    initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;
}

System.Threading.Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo("en-US");

var myProgram = new MyProgram();
var myHost = new MyHost(myProgram);

using (var runspace = RunspaceFactory.CreateRunspace(myHost, initialState))
{
    runspace.Open();

    //var ps = PowerShell.Create();

    var pipeline = runspace.CreatePipeline();

    pipeline.Commands.AddScript($"cd '{runOption.WorkingDirectory}'");

    //ps.Runspace = runspace;
    //ps.AddCommand("cd").AddArgument(runOption.WorkingDirectory);

    foreach (var script in runOption.Scripts)
    {
        if (script.Contains("Write-Host"))
        {
            pipeline.Commands.Add("Write-Output '脚本中存在Write-Host命令，可能会导致RemoteException错误'");
        }

        pipeline.Commands.AddScript(script);
    }

    for (int i = 0; i < pipeline.Commands.Count; i++)
    {
        pipeline.Commands[i].MergeMyResults(PipelineResultTypes.Error, PipelineResultTypes.Output);
    }

    //pipeline.Commands.AddScript(@"out-default");

    try
    {
        var results = pipeline.Invoke();

        Console.WriteLine("执行结果");
        foreach (var result in results)
        {
            Console.WriteLine(result);
        }

    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }

    if (myProgram.ShouldExit)
    {
        Console.WriteLine($"Exit Code: {myProgram.ExitCode}");
    }

    runspace.Close();
}

Console.ReadKey();
