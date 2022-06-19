using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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


        private void Update() {
            OnUpdate?.Invoke();
        }

        public Action OnUpdate;

        protected readonly Dictionary<string, Action> OnGotoScenes = new Dictionary<string, Action>();

        public void ChangeScene(string sceneName) {
            OnUpdate = null;
            OnGotoScenes[sceneName].Invoke();

            SceneManager.LoadScene(sceneName);
        }
        
        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}