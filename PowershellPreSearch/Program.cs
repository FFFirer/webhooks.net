// See https://aka.ms/new-console-template for more information
using PowershellPreSearch;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

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

initialState.ExecutionPolicy = Microsoft.PowerShell.ExecutionPolicy.RemoteSigned;

using (var runspace = RunspaceFactory.CreateRunspace(initialState))
{
    runspace.Open();

    var ps = PowerShell.Create();

    var pipeline = runspace.CreatePipeline();

    ps.Runspace = runspace;
    ps.AddCommand("cd").AddArgument(runOption.WorkingDirectory);

    foreach (var script in runOption.Scripts)
    {
        ps.AddScript(script);
    }

    var results = ps.Invoke();

    Console.WriteLine("执行结果");
    foreach (var result in results)
    {
        Console.WriteLine(result);
    }

    if (ps.HadErrors)
    {
        Console.WriteLine("执行失败，存在错误");
        foreach (var error in ps.Streams.Error)
        {
            Console.WriteLine(error);
        }
    }
    else
    {
        // 执行成功，输出
        Console.WriteLine("执行成功！");
    }

    runspace.Close();
}

Console.ReadKey();
