namespace FrameWork {
    public interface ILeader : ICanGetModel, ICanGetOperator, ICanSendCommand, ICanAddEventListener, ICanGetConfig,
        ICanTriggerEvent {
        IGame BelongedGame { get; }
    }

    public class Leader : ILeader {
        public Leader(IGame belongedGame) {
            NodeController = new NodeController(this);
            eventDispatcher = new EventDispatcher();
            BelongedGame = belongedGame;
        }

        #region Config

        public IGame BelongedGame { get; }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedGame.GetConfig<TConfig>();
        }

        #endregion

        #region 注册Node

        private readonly NodeController NodeController;

        public T GetOperation<T>() where T : class, IOperation {
            return NodeController.GetOperation<T>();
        }

        public T GetModel<T>() where T : class, IModel {
            return NodeController.GetModel<T>();
        }

        public void Register<T>(T node) where T : class, INode {
            NodeController.Register<T>(node);
        }

        public void RegisterWithoutInit<T>(T node) where T : class, INode {
            NodeController.RegisterWithoutInit(node);
        }

        public void UnRegister<T>(T node) where T : class, INode {
            NodeController.UnRegister(node);
        }

        #endregion

        #region Command

        public void SendCommand<T>() where T : ICommand, new() {
            var command = new T {BelongedLeader = this};
            command.Execute();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            command.BelongedLeader = this;
            command.Execute();
        }

        #endregion

        #region Event

        private EventDispatcher eventDispatcher { get; }

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return eventDispatcher.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            eventDispatcher.RemoveEventListener(onEvent);
        }

        public void TriggerEvent<T>() where T : IEvent, new() {
            eventDispatcher.TriggerEvent<T>();
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            eventDispatcher.TriggerEvent(e);
        }

        #endregion
    }
}