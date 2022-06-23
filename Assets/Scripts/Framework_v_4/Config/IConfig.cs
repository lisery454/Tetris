using System;

namespace FrameWork {
    public interface IConfig { }

    public interface ICanGetConfig {
        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig;
    }

    public interface ICanAddConfig {
        void AddConfig<TConfig>(string path, Func<string, TConfig> Reader) where TConfig : class, IConfig;
        void RemoveConfig<TConfig>() where TConfig : class, IConfig;
    }

    public interface ICanSaveConfig {
        void SaveConfig<TConfig>(string path, Action<string, TConfig> Writer)
            where TConfig : class, IConfig;
    }
}