using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public abstract class Game<T> : Singleton<T>, ICanGetConfig where T : Component {
        #region Config

        private readonly IOCContainer<IConfig> ConfigContainer = new IOCContainer<IConfig>();

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return ConfigContainer.Get<TConfig>();
        }

        protected void AddConfig<TConfig>(TConfig config) where TConfig : class, IConfig {
            ConfigContainer.Add(config);
        }

        protected void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            ConfigContainer.Remove<TConfig>();
        }

        #endregion

        #region Update

        public Action OnUpdate; //给Node调用的时间接口

        private void Update() {
            OnUpdate?.Invoke();
        }

        #endregion


        #region Scene

        public void ChangeScene(string sceneName) {
            OnUpdate = null;
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }


        protected readonly Dictionary<string, Func<IEnumerator>> BeforeLoadScene =
            new Dictionary<string, Func<IEnumerator>>();

        protected readonly Dictionary<string, Func<IEnumerator>> AfterLoadScene =
            new Dictionary<string, Func<IEnumerator>>();

        private IEnumerator ChangeSceneCoroutine(string sceneName) {
            yield return BeforeLoadScene[sceneName].Invoke();
            SceneManager.LoadScene(sceneName);
            yield return AfterLoadScene[sceneName].Invoke();
        }

        #endregion
    }
}