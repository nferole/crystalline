using Ferole.CommandLineUtils.Attributes;
using Ferole.CommandLineUtils.Inputs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Ferole.CommandLineUtils.CLIApp
{
    public static class CLIApp
    {
        public static void EnableMakeConfig(string[] args, List<Type> commands)
        {
            try
            {
                if (!args.Contains("--make-config"))
                    return;

                Type? type = commands.FirstOrDefault(t => t.Name.ToLower() == args[0].ToLower());

                if (type == null)
                    return;

                object? instance = Activator.CreateInstance(type);

                string json = JsonSerializer.Serialize(instance, new JsonSerializerOptions { WriteIndented = true });

                using (StreamWriter streamWriter = new StreamWriter($"./{type.Name.ToLower()}.json"))
                {
                    streamWriter.Write(json);
                }

                Console.WriteLine($"Example JSON config file for type {type.Name} created at ./{type.Name.ToLower()}.json");

                Environment.Exit(1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while generating the JSON config file: {ex.Message}");
                Environment.Exit(0);
            }
        }

        public static void EnableHelp(string[] args, List<Type> commandTypes, string header = null)
        {
            if (args.Length != 0 && !Array.Exists(args, arg => arg.Equals("--help", StringComparison.OrdinalIgnoreCase)))
                return;

            if(header != null)
                Console.WriteLine(header);

            foreach (var commandType in commandTypes)
            {
                Console.WriteLine(commandType.Name.ToLower());
                PropertyInfo[] properties = commandType.GetProperties();
                foreach (PropertyInfo property in properties)
                {
                    HelpAttribute? helpAttribute = property.GetCustomAttributes(typeof(HelpAttribute), false).FirstOrDefault() as HelpAttribute;
                    ShorthandAttribute? shorthandAttribute = property.GetCustomAttributes(typeof(ShorthandAttribute), false).FirstOrDefault() as ShorthandAttribute;

                     var arguments = $"--{property.Name}";
                    if (shorthandAttribute != null)
                    {
                        arguments += $" -{shorthandAttribute.Shorthand}";
                    }

                    var helpInfo = helpAttribute != null ? helpAttribute.HelpInfo : "[ no info ]";
                    Console.WriteLine($"  {arguments, 30}  {helpInfo,-48}");
                }
                Console.WriteLine($"  {"--make-config",30}  {"Generates an example json config file for this driver",-48}");
            }
            Environment.Exit(1);
        }
    }
}
