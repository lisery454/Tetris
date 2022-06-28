namespace FrameWork {
    public interface IModel : INode, ICanGetConfig { }

    public abstract class Model : IModel {
        public abstract void Init();
        IGame IBelongedToGame.BelongedGame { get; set; }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return (this as IBelongedToGame).BelongedGame.ConfigController.GetConfig<TConfig>();
        }
    }
}