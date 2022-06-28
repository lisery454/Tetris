using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public interface IGame : ICanChangeScene {
        Action OnUpdate { get; set; } //给Node调用的时间接口


        ConfigController ConfigController { get; set; }
        NodeController NodeController { get; set; }
        EventDispatcher EventDispatcher { get; set; }
        CommandController CommandController { get; set; }
        SoundManager SoundManager { get; set; }
    }

    public abstract class Game : MonoBehaviour, IGame {
        protected virtual void Awake() {
            NodeController = new NodeController(this);
            EventDispatcher = new EventDispatcher();
            CommandController = new CommandController(this);
            ConfigController = new ConfigController();
            SoundManager = new SoundManager(gameObject.AddComponent<AudioSource>());
            GameViewListener = new GameViewListener();

            BeforeLoadSceneAnim = new Dictionary<string, Func<IEnumerator>>();
            AfterLoadSceneAnim = new Dictionary<string, Func<IEnumerator>>();
            OnHaveLeftScene = new Dictionary<string, Action>();
            OnBeforeLoadScene = new Dictionary<string, Action>();
            OnAfterLoadScene = new Dictionary<string, Action>();
            DontDestroyOnLoad(this);
        }


        private void Update() {
            OnUpdate?.Invoke();
            //监听分辨率变化
            GameViewListener.ViewListenerUpdate();
        }

        public void GotoScene(string sceneName) {
            //如果是"Exit"，就退出游戏
            if (sceneName == "Exit") {
                ExitGame();
                return;
            }

            //加载
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        private void ExitGame() {
            OnExitGame?.Invoke();

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        protected virtual IEnumerator ChangeSceneCoroutine(string sceneName) {
            //清空update中的调用方法
            OnUpdate = null;

            //获取前一个场景的名字
            var beforeSceneName = SceneManager.GetActiveScene().name;

            //开始加载场景时
            if (OnBeforeLoadScene.ContainsKey(sceneName))
                OnBeforeLoadScene[sceneName]?.Invoke();


            //加载场景动画开始
            if (BeforeLoadSceneAnim.ContainsKey(sceneName) && BeforeLoadSceneAnim[sceneName] != null)
                yield return BeforeLoadSceneAnim[sceneName].Invoke();
            else if (DefaultBeforeLoadSceneAnim != null)
                yield return DefaultBeforeLoadSceneAnim.Invoke();


            //加载
            var op = SceneManager.LoadSceneAsync(sceneName);
            op.allowSceneActivation = false;
            while (op.progress < 0.9f) {
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForEndOfFrame();
            op.allowSceneActivation = true;

            //离开之前场景时（已经加载完了）
            if (OnHaveLeftScene.ContainsKey(beforeSceneName))
                OnHaveLeftScene[beforeSceneName]?.Invoke();

            //结束加载时
            if (OnAfterLoadScene.ContainsKey(sceneName))
                OnAfterLoadScene[sceneName]?.Invoke();

            //等待所有物体awake，但是还没有渲染
            yield return null;

            //场景加载开始适配分辨率
            if (GameViewListener.OnViewChange.ContainsKey(sceneName))
                GameViewListener.OnViewChange[sceneName]
                    .Invoke(GameViewListener.lastWidth, GameViewListener.lastHeight);

            //加载场景动画结束
            if (AfterLoadSceneAnim.ContainsKey(sceneName) && AfterLoadSceneAnim[sceneName] != null)
                yield return AfterLoadSceneAnim[sceneName].Invoke();
            else if (DefaultAfterLoadSceneAnim != null)
                yield return DefaultAfterLoadSceneAnim.Invoke();
        }


        public SoundManager SoundManager { get; set; }
        public NodeController NodeController { get; set; }
        public EventDispatcher EventDispatcher { get; set; }
        public CommandController CommandController { get; set; }
        public ConfigController ConfigController { get; set; }
        public GameViewListener GameViewListener { get; set; }

        public Action OnUpdate { get; set; } //给Node调用的时间接口

        //每个场景自己的加载动画
        protected Dictionary<string, Func<IEnumerator>> BeforeLoadSceneAnim, AfterLoadSceneAnim;

        //默认加载动画
        protected Func<IEnumerator> DefaultBeforeLoadSceneAnim, DefaultAfterLoadSceneAnim;

        //离开和进入的一些行为
        protected Dictionary<string, Action> OnHaveLeftScene, OnBeforeLoadScene, OnAfterLoadScene;

        //离开游戏的行为
        protected Action OnExitGame;
    }

    public interface IBelongedToGame {
        IGame BelongedGame { get; set; }
    }

    public interface ICanChangeScene {
        void GotoScene(string sceneName);
    }
}