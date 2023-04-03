using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Xml;

namespace Cry.Features.ConfigGenerator
{
    internal class ConfigGenerator
    {
        public static void GenerateExample<T>(string filePath) where T : class, new()
        {
            T instance = new T();

            string json = JsonSerializer.Serialize<T>(instance, new JsonSerializerOptions { WriteIndented = true });

            using (StreamWriter streamWriter = new StreamWriter(filePath))
            {
                streamWriter.Write(json);
            }

            Console.WriteLine($"Example JSON-config file for {typeof(T).Name} created at {filePath}");
        }
    }
}
