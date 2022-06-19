namespace FrameWork {
    
    public interface ICommand : IBelongedLeader, ICanGetOperator {
        void Execute();
    }

    public abstract class AbstractCommand : ICommand {
        public T GetOperation<T>() where T : class, IOperation {
            return belongedLeader.GetOperation<T>();
        }

        void ICommand.Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
        public ILeader belongedLeader { get; set; }
    }

    public interface ICanSendCommand {
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
    }
}