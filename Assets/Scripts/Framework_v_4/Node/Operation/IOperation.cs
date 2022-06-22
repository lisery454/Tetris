using System;

namespace FrameWork {
    public interface IOperation : INode, ICanGetOperator, ICanGetModel,
        ICanAddEventListener, ICanTriggerEvent, ICanGetConfig { }

    public abstract class Operation : IOperation {
        public ILeader BelongedLeader { get; set; }

        public abstract void Init();

        protected Action Update {
            get => BelongedLeader.BelongedGame.OnUpdate;
            set => BelongedLeader.BelongedGame.OnUpdate = value;
        }
        
        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return BelongedLeader.GetConfig<TConfig>();
        }

        #endregion

        #region ICanTriggerEvent

        public void TriggerEvent<T>() where T : IEvent, new() {
            BelongedLeader.TriggerEvent<T>();
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            BelongedLeader.TriggerEvent(e);
        }

        #endregion

        #region ICanAddEventListener

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return BelongedLeader.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            BelongedLeader.RemoveEventListener(onEvent);
        }

        #endregion

        #region ICanGetOperator

        public T GetOperation<T>() where T : class, IOperation {
            return BelongedLeader.GetOperation<T>();
        }

        #endregion

        #region ICanGetModel

        public T GetModel<T>() where T : class, IModel {
            return BelongedLeader.GetModel<T>();
        }

        #endregion
    }


    public interface ICanGetOperator {
        T GetOperation<T>() where T : class, IOperation;
    }
}