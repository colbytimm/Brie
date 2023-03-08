using Brie.Helper;
using System.Text.RegularExpressions;
using Brie.Model;
using Spectre.Console;
using ToolBox.Bridge;

AnsiConsole.Write(
    new FigletText("Brie")
        .Color(Color.Fuchsia));

var shell = ShellHelper.GetShellConfigurator();
var result = shell.Term("lsof -i -P -n -sTCP:LISTEN", Output.Hidden);
var splitResult = result.stdout.Split("\n").Where(s => !string.IsNullOrWhiteSpace(s)).Distinct().ToList();
splitResult.RemoveAt(0);

var portsToKill = new List<PortEntity>();

foreach(var lineResult in splitResult)
{
    var lineItem = Regex.Split( lineResult, @"\s{1,}");

    var portEntity = new PortEntity()
    {
        Pid = lineItem[1],
        User = lineItem[2],
        Port = lineItem[8].Split(":")[1],
        Command = lineItem[0]
    };

    if (portEntity.Port != "*" && !string.IsNullOrEmpty(portEntity.Port))
    {
        portsToKill.Add(portEntity);
    }
}

if (result.code == 0 && portsToKill.Count > 0)
{
    try
    {
        var processString = portsToKill.Count > 1 ? "Processes" : "Process";
        var content = new Markup(
                "\nI hear a little :mouse_face: would like some :cheese_wedge:\n\n" +
                $"[fuchsia]{portsToKill.Count} {processString}[/]\n")
            .Centered();

        AnsiConsole.Write(
            new Panel(content)
                .Border(BoxBorder.Rounded)
                .Header(":cheese_wedge: :cheese_wedge: :cheese_wedge:")
                .HeaderAlignment(Justify.Center));
        
        AnsiConsole.Write(new Rule());


        var port = AnsiConsole.Prompt(
            new SelectionPrompt<PortEntity>()
                .Title("What port would you like to [fuchsia]close[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more ports)[/]")
                .HighlightStyle(new Style(Color.White, Color.Fuchsia))
                .AddChoices(portsToKill.OrderBy(p => p.Port)));
        
        var confirmation = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
                .Title($"Are you sure you want to kill port [fuchsia]{port.Port}[/]?")
                .PageSize(10)
                .MoreChoicesText("[grey](Move up and down to reveal more ports)[/]")
                .HighlightStyle(new Style(Color.White, Color.Fuchsia))
                .AddChoices("NO :cheese_wedge: FOR ME", "YES :cheese_wedge: PLEASE"));

        if (confirmation == "YES :cheese_wedge: PLEASE")
        {
            var closePortResponse = shell.Term($"kill -9 {port.Pid}", Output.Hidden);
            if (closePortResponse.code == 0)
            {
                AnsiConsole.MarkupLine($"Port [fuchsia]{port.Port}[/] closed. :cheese_wedge:");
            }
            else
            {
                AnsiConsole.MarkupLine($"Unable to close port [fuchsia]{port.Port}[/]. :crying_face:");
            }
        }
    }
    catch (Exception)
    {
        AnsiConsole.WriteException(new Exception("Oof...seems like you bit off more than you can chew. :cheese_wedge:"));
    }
}
else
{
    AnsiConsole.WriteException(new Exception("Seems like there was an issues fetching your ports. :cheese_wedge:"));
}
