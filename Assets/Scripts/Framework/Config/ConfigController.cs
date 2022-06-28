using System;

namespace FrameWork {
    public class ConfigController {
        private readonly IOCContainer<IConfig> ConfigContainer = new IOCContainer<IConfig>();

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return ConfigContainer.Get<TConfig>();
        }

        public void AddConfig<TConfig>(string path, Func<string, TConfig> Reader) where TConfig : class, IConfig {
            var config = Reader.Invoke(path);
            ConfigContainer.Add(config);
        }

        public void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            ConfigContainer.Remove<TConfig>();
        }

        public void SaveConfig<TConfig>(string path, Action<string, TConfig> Writer)
            where TConfig : class, IConfig {
            Writer.Invoke(path, GetConfig<TConfig>());
        }
    }
}