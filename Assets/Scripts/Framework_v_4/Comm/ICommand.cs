namespace FrameWork {
    public interface ICommand : IBelongedLeader, ICanGetOperator {
        void Execute();
    }

    public abstract class Command : ICommand {
        public T GetOperation<T>() where T : class, IOperation {
            return BelongedLeader.GetOperation<T>();
        }

        void ICommand.Execute() {
            OnExecute();
        }

        protected abstract void OnExecute();
        public ILeader BelongedLeader { get; set; }

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }
    }

    public interface ICanSendCommand {
        void SendCommand<T>() where T : ICommand, new();
        void SendCommand<T>(T command) where T : ICommand;
    }
}