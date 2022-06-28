namespace FrameWork {
    public interface INode : IBelongedToGame {
        public abstract void Init();
    }

    public interface ICanRegisterNode {
        void Register<T>(T node) where T : class, INode;
        void RegisterWithoutInit<T>(T node) where T : class, INode;
        void UnRegister<T>() where T : class, INode;
    }

    public interface ICanGetNode {
        T GetNode<T>() where T : class, INode;
    }
}