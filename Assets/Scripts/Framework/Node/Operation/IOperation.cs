using System;

namespace FrameWork {
    public interface IOperation : INode, ICanGetNode,
        ICanAddEventListener, ICanTriggerEvent, ICanGetConfig { }

    public abstract class Operation : IOperation {
        public abstract void Init();

        protected Action Update {
            get => (this as IBelongedToGame).BelongedGame.OnUpdate;
            set => (this as IBelongedToGame).BelongedGame.OnUpdate = value;
        }

        #region ICanGetConfig

        public TConfig GetConfig<TConfig>() where TConfig : class, IConfig {
            return (this as IBelongedToGame).BelongedGame.ConfigController.GetConfig<TConfig>();
        }

        #endregion

        #region ICanTriggerEvent

        public void TriggerEvent<T>() where T : IEvent, new() {
            (this as IBelongedToGame).BelongedGame.EventDispatcher.TriggerEvent<T>();
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            (this as IBelongedToGame).BelongedGame.EventDispatcher.TriggerEvent(e);
        }

        #endregion

        #region ICanAddEventListener

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return (this as IBelongedToGame).BelongedGame.EventDispatcher.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            (this as IBelongedToGame).BelongedGame.EventDispatcher.RemoveEventListener(onEvent);
        }

        #endregion

        #region ICanGetNode

        public T GetNode<T>() where T : class, INode {
            return (this as IBelongedToGame).BelongedGame.NodeController.GetNode<T>();
        }

        #endregion

        IGame IBelongedToGame.BelongedGame { get; set; }
    }
}