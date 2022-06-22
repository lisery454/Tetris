using UnityEngine;
using UnityEngine.SceneManagement;

namespace FrameWork {
    public interface IExhibitor : IBelongedLeader, ICanSendCommand, ICanAddEventListener, ICanGetModel,
        ICanGetConfig { }

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
    }
}