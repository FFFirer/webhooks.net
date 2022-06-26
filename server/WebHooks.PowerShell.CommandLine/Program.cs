using WebHooks.PowerShell;

IProcessRunner runner = new ProcessRunner();

var powershellPath = @"C:\Windows\SysWOW64\WindowsPowerShell\v1.0\powershell.exe";
var powershellStartSetting = new ProcessSettings()
{
    WorkingDirectory = @"C:\Users\GOD\Desktop\test-dir",
    RedirectStandardError = true,
    RedirectStandardOutput = true,
};

IProcess process = runner.Start(powershellPath, powershellStartSetting);

var error = process.GetStandardError();
var output = process.GetStandardOutput();

Console.WriteLine("output:");
Console.WriteLine(String.Join("\n", output));

Console.WriteLine("error");
Console.WriteLine(String.Join("\n", error));

process.WaitForExit();

Console.WriteLine($"Exit: {process.GetExitCode()}");
Console.ReadKey();