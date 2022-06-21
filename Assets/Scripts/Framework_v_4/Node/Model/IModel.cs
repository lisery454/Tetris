namespace FrameWork {
    public interface IModel : INode { }

    public abstract class Model : IModel {
        public ILeader BelongedLeader { get; set; }
        public abstract void Init();

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }
    }


    public interface ICanGetModel {
        T GetModel<T>() where T : class, IModel;
    }
}