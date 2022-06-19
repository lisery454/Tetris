using YamlDotNet.Serialization;

namespace FrameWork {
    public interface IConfig { }

    public class YamlConfig : IConfig { }

    public static class ConfigRWer {
        private static readonly Deserializer deserializer = new Deserializer();
        private static readonly Serializer serializer = new Serializer();

        public static T ReadConfig<T>(string yamlPath) where T : class, IConfig {
            var yaml = System.IO.File.ReadAllText(yamlPath);
            var config = deserializer.Deserialize<T>(yaml);
            return config;
        }

        public static void WriteConfig<T>(T config, string yamlPath) where T : class, IConfig {
            if (!System.IO.File.Exists(yamlPath)) {
                System.IO.File.Create(yamlPath).Dispose();
            }
            
            var yaml = serializer.Serialize(config);
            System.IO.File.WriteAllText(yamlPath, yaml);
        }
    }
}