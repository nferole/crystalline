using Ferole.CommandLineUtils.Attributes;
using System.Reflection;
using System.Text.Json;

namespace Ferole.CommandLineUtils.Inputs
{
    public static class BindInputArguments
    {
        public static async Task BindToAndExecuteCommandAsync(string[] args, Type? bindToType)
        {
            if (args == null)
                throw new ArgumentNullException(nameof(args));
            if (bindToType == null)
                throw new ArgumentNullException(nameof(bindToType));

            string command = args[0].ToLowerInvariant();

            ICLICommand commandObject = (ICLICommand?)Activator.CreateInstance(bindToType)
                ?? throw new ArgumentException($"Could not create a new instance of {command}");

            await MapArgumentsToObjectAsync(args, bindToType, commandObject);

            string configPath = GetConfigPath(bindToType, commandObject);

            if (IsValidPath(configPath))
            {
                MergeConfigArguments(bindToType, commandObject, configPath);
            }

            await commandObject.ExecuteCommandAsync();
        }

        private static async Task MapArgumentsToObjectAsync(string[] args, Type argumentType, ICLICommand arguments)
        {
            for (int i = 1; i < args.Length; i++)
            {
                string argumentName = args[i].TrimStart('-');
                bool isFlag = IsFlagArgument(args, i);

                string argumentValue = args[i + 1];
                await SetPropertyValueAsync(argumentType, arguments, argumentName, isFlag ? "true" : argumentValue);
                i++;
            }
        }

        private static async Task SetPropertyValueAsync(Type commandType, ICLICommand arguments, string argumentName, string argumentValue)
        {
            PropertyInfo property = await GetWritablePropertyAsync(commandType, argumentName);
            object value = Convert.ChangeType(argumentValue, property.PropertyType);
            property.SetValue(arguments, value);
        }

        private static void MergeConfigArguments(Type argumentType, ICLICommand arguments, string configPath)
        {
            string json = File.ReadAllText(configPath);
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            var configArguments = JsonSerializer.Deserialize(json, argumentType, options);
            if (configArguments != null)
            {
                foreach (var property in argumentType.GetProperties())
                {
                    if (property.CanWrite)
                    {
                        var value = property.GetValue(configArguments);
                        if (value != null)
                        {
                            property.SetValue(arguments, value);
                        }
                    }
                }
            }
        }

        private static string GetConfigPath(Type argumentType, ICLICommand arguments)
        {
            string configPath = string.Empty;

            foreach (var property in argumentType.GetProperties())
            {
                if (property.GetCustomAttributes(typeof(ConfigPathAttribute), false).Length > 0)
                {
                    configPath = property.GetValue(arguments) as string ?? string.Empty;
                    break;
                }
            }

            return configPath;
        }

        private static bool IsValidPath(string path)
        {
            try
            {
                Path.GetFullPath(path);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool IsFlagArgument(string[] args, int index)
        {
            return args[index].StartsWith("-") && args[index + 1].StartsWith("-");
        }
        private static async Task<PropertyInfo> GetWritablePropertyAsync(Type argumentType, string propertyName)
        {
            // Check if property exists as named
            PropertyInfo? property = argumentType.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (property != null)
            {
                return property;
            }
            // Check if any property with the ShorthandAttribute decoration is matching
            var propertyFromShorthand = argumentType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                {
                    var shorthandAttribute = p.GetCustomAttribute<ShorthandAttribute>();
                    return shorthandAttribute != null && string.Equals(shorthandAttribute.Shorthand, propertyName, StringComparison.OrdinalIgnoreCase);
                })
                .FirstOrDefault();
            if (propertyFromShorthand != null)
            {
                return propertyFromShorthand;
            }
            // Neither the property nor the shorthand property exists
            throw new ArgumentException($"Invalid property: {propertyName}");
        }

    }
}