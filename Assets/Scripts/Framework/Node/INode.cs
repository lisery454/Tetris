namespace FrameWork {
    public interface INode : IBelongedLeader {
        public abstract void Init();
    }

    public interface ICanRegisterNode {
        void Register<T>(T node) where T : class, INode;
        void RegisterWithoutInit<T>(T node) where T : class, INode;
        void UnRegister<T>(T node) where T : class, INode;
    }
}