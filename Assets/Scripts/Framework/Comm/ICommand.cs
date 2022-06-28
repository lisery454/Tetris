namespace FrameWork {
    public interface ICommand : IBelongedToGame, ICanGetNode {
        void Execute();
    }

    public abstract class Command : ICommand {
        void ICommand.Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
        IGame IBelongedToGame.BelongedGame { get; set; }

        public T GetNode<T>() where T : class, INode {
            return (this as IBelongedToGame).BelongedGame.NodeController.GetNode<T>();
        }
    }

    public interface ICanSendCommand {
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
    }
}