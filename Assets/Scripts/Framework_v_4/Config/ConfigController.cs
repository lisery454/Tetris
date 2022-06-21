namespace FrameWork {
    public class ConfigController {
        private readonly IOCContainer<IConfig> ConfigContainer = new IOCContainer<IConfig>();

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return ConfigContainer.Get<TConfig>();
        }

        public void AddConfig<TConfig>(TConfig config) where TConfig : class, IConfig {
            ConfigContainer.Add(config);
        }

        public void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            ConfigContainer.Remove<TConfig>();
        }
    }
}