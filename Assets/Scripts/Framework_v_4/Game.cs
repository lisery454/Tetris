using UnityEngine;

namespace FrameWork {
    public abstract class Game<T> : Singleton<T> where T : Component {
        private IOCContainer<IConfig> IocContainer = new IOCContainer<IConfig>();
        
        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return IocContainer.Get<TConfig>();
        }

        protected void AddConfig<TConfig>(TConfig config) where TConfig : class, IConfig {
            IocContainer.Add(config);
        }

        protected void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            IocContainer.Remove<TConfig>();
        }
    }
}