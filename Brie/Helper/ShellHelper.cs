using ToolBox.Bridge;
using ToolBox.Platform;

namespace Brie.Helper;

static class ShellHelper
{
    private static IBridgeSystem? BridgeSystem { get; set; }

    public static ShellConfigurator GetShellConfigurator()
    {
        switch (OS.GetCurrent())
        {
            case "win":
                BridgeSystem = ToolBox.Bridge.BridgeSystem.Bat;
                break;
            case "mac":
            case "gnu":
                BridgeSystem = ToolBox.Bridge.BridgeSystem.Bash;
                break;
        }
        return new ShellConfigurator(BridgeSystem);
    }
}