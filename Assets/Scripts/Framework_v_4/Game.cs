using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public interface IGame : ICanGetConfig, ICanChangeScene, ICanSaveConfig, ICanAddConfig {
        Action OnUpdate { get; set; } //给Node调用的时间接口
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

        public void AddConfig<TConfig>(string path, Func<string, TConfig> Reader)
            where TConfig : class, IConfig {
            var config = Reader.Invoke(path);
            ConfigController.AddConfig(config);
        }

        public void RemoveConfig<TConfig>() where TConfig : class, IConfig {
            ConfigController.RemoveConfig<TConfig>();
        }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return ConfigController.GetConfig<TConfig>();
        }

        public virtual void SaveConfig<TConfig>(string path, Action<string, TConfig> Writer)
            where TConfig : class, IConfig {
            Writer.Invoke(path, GetConfig<TConfig>());
        }

        #endregion

        #region Update

        public Action OnUpdate { get; set; } //给Node调用的时间接口

        private void Update() {
            OnUpdate?.Invoke();
            ViewListenerUpdate();
        }

        #endregion

        #region GameViewListener

        private int lastWidth { get; set; }
        private int lastHeight { get; set; }

        private void ViewListenerUpdate() {
            if (lastWidth != Screen.width ||
                lastHeight != Screen.height) {
                lastWidth = Screen.width;
                lastHeight = Screen.height;

                OnViewUpdate(lastWidth, lastHeight);
            }
        }

        public virtual void OnViewUpdate(int width, int height) { }

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
            if (sceneName == "Exit") {
                ExitGame();
                return;
            }

            //加载
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        private void ExitGame() {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        private IEnumerator ChangeSceneCoroutine(string sceneName) {
            OnUpdate = null;

            var beforeSceneName = SceneManager.GetActiveScene().name;

            //开始加载场景时
            if (OnStartLoadScene.ContainsKey(sceneName))
                OnStartLoadScene[sceneName]?.Invoke();


            //加载场景动画开始
            if (BeforeLoadSceneAnim.ContainsKey(sceneName) && BeforeLoadSceneAnim[sceneName] != null)
                yield return BeforeLoadSceneAnim[sceneName].Invoke();
            else if (DefaultBeforeLoadSceneAnim != null) {
                yield return DefaultBeforeLoadSceneAnim.Invoke();
            }

            //加载
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f) {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
            op.allowSceneActivation = true;

            //移除leader
            LeaderFactory.RemoveLeader("beforeSceneName");

            //离开之前场景时（已经加载完了）
            if (OnLeaveSceneAfterOtherSceneLoaded.ContainsKey(beforeSceneName))
                OnLeaveSceneAfterOtherSceneLoaded[beforeSceneName]?.Invoke();

            //结束加载时
            if (OnEndLoadScene.ContainsKey(sceneName))
                OnEndLoadScene[sceneName]?.Invoke();

            yield return null;
            //yield return null;

            //场景中适配分辨率
            OnViewUpdate(lastWidth, lastHeight);
            
            //加载场景动画结束
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

    public interface ICanChangeScene {
        void GotoScene(string sceneName);
    }
}