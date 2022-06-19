using UnityEngine;

namespace FrameWork {
    public interface IExhibitor : IBelongedLeader, ICanSendCommand, ICanAddEventListener { }

    public abstract class AbstractExhibitor : MonoBehaviour, IExhibitor {
        public void SendCommand<T>() where T : ICommand, new() {
            belongedLeader.SendCommand<T>();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            belongedLeader.SendCommand(command);
        }

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return belongedLeader.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            belongedLeader.RemoveEventListener(onEvent);
        }

        public ILeader belongedLeader { get; set; }
    }
}