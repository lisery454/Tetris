using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

namespace FrameWork {
    public interface IConfig { }

    public class YamlConfig : IConfig { }

    public static class ConfigRWer {
        private static readonly Deserializer deserializer = new Deserializer();
        private static readonly Serializer serializer = new Serializer();

        public static T ReadConfig<T>(string yamlPath) where T : class, IConfig {
            CreateDefaultConfig(yamlPath);
            var yaml = File.ReadAllText(Application.persistentDataPath + '/' + yamlPath);
            var config = deserializer.Deserialize<T>(yaml);
            return config;
        }

        public static void WriteConfig<T>(T config, string yamlPath) where T : class, IConfig {
            CreateDefaultConfig(yamlPath);
            var yaml = serializer.Serialize(config);
            File.WriteAllText(Application.persistentDataPath + '/' + yamlPath, yaml);
        }

        private static void CreateDefaultConfig(string yamlPath) {
            var path = Application.persistentDataPath + '/' + yamlPath;
            var directoryPath = path.Substring(0, path.LastIndexOf('/'));

            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(path)) {
                File.Create(path);
                var originYaml = Resources.Load<TextAsset>(yamlPath.Substring(0, yamlPath.LastIndexOf('.'))).text;
                File.WriteAllText(path, originYaml);
            }
        }
    }

    public interface ICanGetConfig {
        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig;
    }
}