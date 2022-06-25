using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public interface IExhibitor : IBelongedLeader, ICanSendCommand, ICanAddEventListener, ICanGetModel,
        ICanGetConfig, ICanChangeScene, ICanSaveConfig, ICanPlaySound { }


    /// <summary>
    /// 展示者，用来呈现
    /// 一般来说展示者自己可以解决的简单逻辑自己解决
    /// 如果说要修改后台数据或者是和其他展示者要沟通数据, 或者是有什么复杂操作，就要使用command
    /// 可以读model，但是不要改
    /// </summary>
    public abstract class Exhibitor : MonoBehaviour, IExhibitor {
        public ILeader BelongedLeader { get; set; }

        protected virtual void Awake() {
            BelongedLeader = FindObjectOfType<Game>().LeaderFactory.GetLeader(SceneManager.GetActiveScene().name);
        }

        #region ICanSendCommand

        public void SendCommand<T>() where T : ICommand, new() {
            BelongedLeader.SendCommand<T>();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            BelongedLeader.SendCommand(command);
        }

        #endregion

        #region ICanAddEventListener

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return BelongedLeader.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            BelongedLeader.RemoveEventListener(onEvent);
        }

        #endregion

        #region ICanGetModel

        public T GetModel<T>() where T : class, IModel {
            return BelongedLeader.GetModel<T>();
        }

        #endregion

        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }

        #endregion

        #region ICanChangeScene

        public void GotoScene(string sceneName) {
            BelongedLeader.BelongedGame.GotoScene(sceneName);
        }

        #endregion

        #region ICanSaveConfig

        public void SaveConfig<TConfig>(string path, Action<string, TConfig> Writer) where TConfig : class, IConfig {
            BelongedLeader.BelongedGame.SaveConfig(path, Writer);
        }

        #endregion

        #region ICanPlaySound

        public void PlayGlobalSound(string clipName) {
            BelongedLeader.BelongedGame.PlayGlobalSound(clipName);
        }

        public void StopGlobalSound() {
            BelongedLeader.BelongedGame.StopGlobalSound();
        }

        public void PlaySFX(string clipName, float volumeFactor = 1) {
            BelongedLeader.BelongedGame.PlaySFX(clipName, volumeFactor);
        }

        #endregion
    }
}