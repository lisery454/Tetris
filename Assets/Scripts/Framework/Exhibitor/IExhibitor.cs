using System;
using UnityEngine;

namespace FrameWork {
    public interface IExhibitor : IBelongedToGame, ICanSendCommand, ICanAddEventListener, ICanGetNode,
        ICanGetConfig, ICanChangeScene, ICanSaveConfig, ICanPlaySound { }


    /// <summary>
    /// 展示者，用来呈现
    /// 一般来说展示者自己可以解决的简单逻辑自己解决
    /// 如果说要修改后台数据或者是和其他展示者要沟通数据, 或者是有什么复杂操作，就要使用command
    /// 可以读model，但是不要改
    /// </summary>
    public abstract class Exhibitor : MonoBehaviour, IExhibitor {
        IGame IBelongedToGame.BelongedGame { get; set; }

        protected virtual void Awake() {
            (this as IBelongedToGame).BelongedGame = FindObjectOfType<Game>();
        }

        #region ICanSendCommand

        public void SendCommand<T>() where T : ICommand, new() {
            (this as IBelongedToGame).BelongedGame.CommandController.SendCommand<T>();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            (this as IBelongedToGame).BelongedGame.CommandController.SendCommand(command);
        }

        #endregion

        #region ICanAddEventListener

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return (this as IBelongedToGame).BelongedGame.EventDispatcher.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            (this as IBelongedToGame).BelongedGame.EventDispatcher.RemoveEventListener(onEvent);
        }

        #endregion

        #region ICanGetNode

        public T GetNode<T>() where T : class, INode {
            return (this as IBelongedToGame).BelongedGame.NodeController.GetNode<T>();
        }

        #endregion

        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return (this as IBelongedToGame).BelongedGame.ConfigController.GetConfig<TConfig>();
        }

        #endregion

        #region ICanChangeScene

        public void GotoScene(string sceneName) {
            (this as IBelongedToGame).BelongedGame.GotoScene(sceneName);
        }

        #endregion

        #region ICanSaveConfig

        public void SaveConfig<TConfig>(string path, Action<string, TConfig> Writer) where TConfig : class, IConfig {
            (this as IBelongedToGame).BelongedGame.ConfigController.SaveConfig(path, Writer);
        }

        #endregion

        #region ICanPlaySound

        public void PlayGlobalSound(string clipName) {
            (this as IBelongedToGame).BelongedGame.SoundManager.PlayGlobalSound(clipName);
        }

        public void StopGlobalSound() {
            (this as IBelongedToGame).BelongedGame.SoundManager.StopGlobalSound();
        }

        public void PlaySFX(string clipName, float volumeFactor = 1) {
            (this as IBelongedToGame).BelongedGame.SoundManager.PlaySFX(clipName, volumeFactor);
        }

        #endregion
    }
}