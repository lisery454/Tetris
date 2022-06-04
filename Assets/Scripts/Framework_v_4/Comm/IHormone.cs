namespace FrameWork {
    public interface IHormone { }

    public abstract class AbstractHormone : IHormone { }

    public interface ICanRegisterHormone {
        IUnReceiver ReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone;
        void UnReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone;
    }

    public interface ICanTriggerHormone {
        void ReleaseHormone<T>() where T : IHormone, new();
        void ReleaseHormone<T>(T e) where T : IHormone;
    }
}