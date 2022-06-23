using System.IO;
using UnityEngine;
using YamlDotNet.Serialization;

namespace FrameWork {
    public class YamlConfig : IConfig {
        private static readonly Deserializer deserializer = new Deserializer();
        private static readonly Serializer serializer = new Serializer();

        public static T ReadConfig<T>(string yamlPath) where T : YamlConfig, new() {
            IfNotExistThenCreateDefaultConfig<T>(yamlPath);
            var yaml = File.ReadAllText(Application.persistentDataPath + '/' + yamlPath);
            var config = deserializer.Deserialize<T>(yaml);
            return config;
        }

        public static void WriteConfig<T>(string yamlPath, T config) where T : YamlConfig, new() {
            IfNotExistThenCreateDefaultConfig<T>(yamlPath);
            var yaml = serializer.Serialize(config);
            File.WriteAllText(Application.persistentDataPath + '/' + yamlPath, yaml);
        }

        private static void IfNotExistThenCreateDefaultConfig<T>(string yamlPath) where T : YamlConfig, new() {
            var path = Application.persistentDataPath + '/' + yamlPath;
            var directoryPath = path.Substring(0, path.LastIndexOf('/'));

            if (!Directory.Exists(directoryPath)) {
                Directory.CreateDirectory(directoryPath);
            }

            if (!File.Exists(path)) {
                var fileStream = File.Create(path);
                fileStream.Close();
                File.WriteAllText(path, serializer.Serialize(new T()));
            }
        }
    }
}