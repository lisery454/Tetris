using UnityEngine;

namespace FrameWork {
    public interface IExhibitor : IBelongedLeader, ICanSendCommand, ICanAddEventListener, ICanGetModel { }

    public abstract class Exhibitor : MonoBehaviour, IExhibitor {
        public void SendCommand<T>() where T : ICommand, new() {
            BelongedLeader.SendCommand<T>();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            BelongedLeader.SendCommand(command);
        }

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return BelongedLeader.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            BelongedLeader.RemoveEventListener(onEvent);
        }

        public ILeader BelongedLeader { get; set; }

        public T GetModel<T>() where T : class, IModel {
            return BelongedLeader.GetModel<T>();
        }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }
    }
}