namespace FrameWork {
    public interface IEvent { }

    public abstract class Event : IEvent { }

    public interface ICanAddEventListener {
        IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent;
        void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent;
    }

    public interface ICanTriggerEvent {
        void TriggerEvent<T>() where T : IEvent, new();
        void TriggerEvent<T>(T e) where T : IEvent;
    }
}