// See https://aka.ms/new-console-template for more information
using Demo01;

Console.WriteLine("Hello, World!");
Console.Title = "Demo01";
ConsoleColor oldFg = Console.ForegroundColor;
Console.WriteLine("|      Demo01                          |");
Console.WriteLine("========================================");
Console.WriteLine("|                                      |");
Console.WriteLine("| Author:   fffirer                    |");
Console.WriteLine("| Copyright 2021-2021                  |");
Console.WriteLine("|                                      |");
Console.WriteLine("| Enter 'exit' to exit.                |");

PSListenerConsoleSample PSListener = new PSListenerConsoleSample();
PSListener.Run();