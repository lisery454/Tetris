namespace FrameWork {
    public interface IModel : INode { }

    public abstract class AbstractModel : IModel {
        public ILeader belongedLeader { get; set; }
        public abstract void Init();
    }

    public interface ICanGetModel {
        T GetModel<T>() where T : class, IModel;
    }
}