namespace FrameWork {
    public interface IRoot : INode, ICanGetRoot, ICanGetSoil, ICanRegisterHormone, ICanTriggerHormone { }

    public abstract class AbstractRoot : IRoot {
        public IStem BelongedStem { get; set; }
        public abstract void Init();


        public IUnReceiver ReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            return BelongedStem.ReceiveHormone(onHormone);
        }

        public void UnReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            BelongedStem.UnReceiveHormone(onHormone);
        }

        public void ReleaseHormone<T>() where T : IHormone, new() {
            BelongedStem.ReleaseHormone<T>();
        }

        public void ReleaseHormone<T>(T e) where T : IHormone {
            BelongedStem.ReleaseHormone(e);
        }

        public T GetRoot<T>() where T : class, IRoot {
            return BelongedStem.GetRoot<T>();
        }

        public T GetSoil<T>() where T : class, ISoil {
            return BelongedStem.GetSoil<T>();
        }
    }


    public interface ICanGetRoot {
        T GetRoot<T>() where T : class, IRoot;
    }
}