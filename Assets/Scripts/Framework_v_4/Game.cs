using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public interface IGame : ICanGetConfig {
        Action OnUpdate { get; set; } //给Node调用的时间接口

        public void GotoScene(string sceneName);
        public void ExitGame();

        void SaveConfig<TConfig>() where TConfig : class, IConfig;
    }

    public abstract class Game : MonoBehaviour, IGame {
        protected virtual void Awake() {
            LeaderFactory = new LeaderFactory(this);
            DontDestroyOnLoad(this);
        }


        #region Leader

        public LeaderFactory LeaderFactory;

        #endregion

        #region Config

        private readonly ConfigController ConfigController = new ConfigController();

        protected void AddConfig<TConfig>(TConfig config) where TConfig : class, IConfig {
            ConfigController.AddConfig(config);
        }

        protected void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            ConfigController.RemoveConfig<TConfig>();
        }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return ConfigController.GetConfig<TConfig>();
        }

        public virtual void SaveConfig<TConfig>() where TConfig : class, IConfig {
            //var config = GetConfig<TConfig>();
        }

        #endregion

        #region Update

        public Action OnUpdate { get; set; } //给Node调用的时间接口

        private void Update() {
            OnUpdate?.Invoke();
        }

        #endregion

        #region Scene

        protected readonly Dictionary<string, Func<IEnumerator>> BeforeLoadSceneAnim =
            new Dictionary<string, Func<IEnumerator>>();

        protected readonly Dictionary<string, Func<IEnumerator>> AfterLoadSceneAnim =
            new Dictionary<string, Func<IEnumerator>>();

        protected Func<IEnumerator> DefaultBeforeLoadSceneAnim, DefaultAfterLoadSceneAnim;

        protected readonly Dictionary<string, Action> OnLeaveSceneAfterOtherSceneLoaded =
            new Dictionary<string, Action>();

        protected readonly Dictionary<string, Action> OnStartLoadScene = new Dictionary<string, Action>();
        protected readonly Dictionary<string, Action> OnEndLoadScene = new Dictionary<string, Action>();


        public void GotoScene(string sceneName) {
            OnUpdate = null;

            var beforeSceneName = SceneManager.GetActiveScene().name;

            //开始加载场景时
            if (OnStartLoadScene.ContainsKey(sceneName))
                OnStartLoadScene[sceneName]?.Invoke();

            //加载
            StartCoroutine(ChangeSceneCoroutine(sceneName));
            
            LeaderFactory.RemoveLeader("beforeSceneName");

            //离开之前场景时（已经加载完了）
            if (OnLeaveSceneAfterOtherSceneLoaded.ContainsKey(beforeSceneName))
                OnLeaveSceneAfterOtherSceneLoaded[beforeSceneName]?.Invoke();

            //结束加载时
            if (OnEndLoadScene.ContainsKey(sceneName))
                OnEndLoadScene[sceneName]?.Invoke();
        }

        public void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator ChangeSceneCoroutine(string sceneName) {
            if (BeforeLoadSceneAnim.ContainsKey(sceneName) && BeforeLoadSceneAnim[sceneName] != null)
                yield return BeforeLoadSceneAnim[sceneName].Invoke();
            else if (DefaultBeforeLoadSceneAnim != null) {
                yield return DefaultBeforeLoadSceneAnim.Invoke();
            }

            SceneManager.LoadScene(sceneName);

            if (AfterLoadSceneAnim.ContainsKey(sceneName) && AfterLoadSceneAnim[sceneName] != null)
                yield return AfterLoadSceneAnim[sceneName].Invoke();
            else if (DefaultAfterLoadSceneAnim != null)
                yield return DefaultAfterLoadSceneAnim.Invoke();
        }

        #endregion
    }

    public interface IBelongedToGame {
        IGame BelongedGame { get; }
    }
    
}