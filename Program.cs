using Translation.Services.Files;
using Translation.Services.Translations;

Console.WriteLine("Please enter a translate source:");
var word = Console.ReadLine();
ConnectToRemote remote = new ConnectToRemote(word);
Console.WriteLine("Please enter a translate target:");
ICollection<string> target = new List<string>();
while (true)
{
    var line = Console.ReadLine();
    if (line == "")
    {
        break;
    }
    Console.WriteLine("Please enter a translate target:");
    remote.SetToLanguage(line);
}
Console.WriteLine("you can finis the translate by pressing enter :)");
if (Console.ReadKey().Key == ConsoleKey.Enter)
{
    new FileService().SetRemote(remote).LoadXMLFromFile().Build();
}
else
{
    Console.WriteLine("You rejected the translate");
} 