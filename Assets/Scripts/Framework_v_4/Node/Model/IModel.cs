namespace FrameWork {
    public interface IModel : INode, ICanGetConfig { }

    public abstract class Model : IModel {
        public ILeader BelongedLeader { get; set; }
        public abstract void Init();

        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }

        #endregion
    }


    public interface ICanGetModel {
        T GetModel<T>() where T : class, IModel;
    }
}