using Cry.Features.Help;
using Ferole.CommandLineUtils.CLIApp;
using Ferole.CommandLineUtils.Inputs;
using System.Reflection;

internal class Program
{
    private static async Task Main(string[] args)
    {
        List<Type> commands = GetAllCommands();

        CLIApp.EnableMakeConfig(args, commands);
        CLIApp.EnableHelp(args, commands, HelpHeader.GetLogoString());

        await BindInputArguments.BindToAndExecuteCommandAsync(args, GetTypeFromArgs(args));
    }

    private static Type? GetTypeFromArgs(string[] args)
    {
        return Type.GetType($"Cry.Features.Commands.{args[0]}", true, true);
    }

    private static List<Type> GetAllCommands()
    {
        return Assembly.GetExecutingAssembly()
        .GetTypes()
        .Where(t => typeof(ICLICommand).IsAssignableFrom(t) && t.IsClass)
        .ToList();
    }
}
