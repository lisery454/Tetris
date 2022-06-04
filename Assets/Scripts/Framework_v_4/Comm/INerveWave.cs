namespace FrameWork {
    
    public interface INerveWave : IBelongedStem, ICanGetRoot {
        void Execute();
    }

    public abstract class AbstractNerveWave : INerveWave {
        public T GetRoot<T>() where T : class, IRoot {
            return BelongedStem.GetRoot<T>();
        }

        void INerveWave.Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
        public IStem BelongedStem { get; set; }
    }

    public interface ICanSendNerveWave {
        void SendNerveWave<T>() where T : INerveWave, new();
        void SendNerveWave<T>(T command) where T : INerveWave;
    }
}