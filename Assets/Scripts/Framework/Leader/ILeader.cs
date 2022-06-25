namespace FrameWork {
    public interface ILeader : ICanGetModel, ICanGetOperator, ICanSendCommand, ICanAddEventListener, ICanGetConfig,
        ICanTriggerEvent, IBelongedToGame, ICanRegisterNode { }

    public class Leader : ILeader {
        public IGame BelongedGame { get; }
        
        private NodeController NodeController { get; }
        private EventDispatcher EventDispatcher { get; }


        public Leader(IGame belongedGame) {
            NodeController = new NodeController(this);
            EventDispatcher = new EventDispatcher();
            BelongedGame = belongedGame;
        }

        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedGame.GetConfig<TConfig>();
        }

        #endregion

        #region ICanGetModel

        public T GetModel<T>() where T : class, IModel {
            return NodeController.GetModel<T>();
        }

        #endregion

        #region ICanGetOperator

        public T GetOperation<T>() where T : class, IOperation {
            return NodeController.GetOperation<T>();
        }

        #endregion

        #region ICanRegisterNode

        public void Register<T>(T node) where T : class, INode {
            NodeController.Register(node);
        }

        public void RegisterWithoutInit<T>(T node) where T : class, INode {
            NodeController.RegisterWithoutInit(node);
        }

        public void UnRegister<T>(T node) where T : class, INode {
            NodeController.UnRegister(node);
        }

        #endregion

        #region ICanSendCommand

        public void SendCommand<T>() where T : ICommand, new() {
            var command = new T {BelongedLeader = this};
            command.Execute();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            command.BelongedLeader = this;
            command.Execute();
        }

        #endregion

        #region ICanAddEventListener, ICanTriggerEvent

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return EventDispatcher.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            EventDispatcher.RemoveEventListener(onEvent);
        }

        public void TriggerEvent<T>() where T : IEvent, new() {
            EventDispatcher.TriggerEvent<T>();
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            EventDispatcher.TriggerEvent(e);
        }

        #endregion
    }
}