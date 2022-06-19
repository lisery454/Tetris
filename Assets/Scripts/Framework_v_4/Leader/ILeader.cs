using System;
using System.Collections.Generic;

namespace FrameWork {
    public interface ILeader : ICanGetModel, ICanGetOperator, ICanSendCommand, ICanAddEventListener, ICanTriggerEvent { }

    public abstract class AbstractLeader : ILeader {
        protected AbstractLeader() {
            IOCContainer = new Dictionary<Type, INode>();
            eventDispatcher = new EventDispatcher();
        }

        #region IOC 注册和获取后台组件

        protected readonly Dictionary<Type, INode> IOCContainer;

        private T Get<T>() where T : class, INode {
            if (IOCContainer.TryGetValue(typeof(T), out var component)) {
                return (T) component;
            }

            return null;
        }

        public T GetOperation<T>() where T : class, IOperation {
            return Get<T>();
        }

        public T GetModel<T>() where T : class, IModel {
            return Get<T>();
        }

        #endregion

        #region Command

        public void SendCommand<T>() where T : ICommand, new() {
            var command = new T {belongedLeader = this};
            command.Execute();
        }

        public void SendCommand<T>(T command) where T : ICommand {
            command.belongedLeader = this;
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

    public class Leader : AbstractLeader {
        public void Register<T>(T node) where T : INode {
            IOCContainer.Add(typeof(T), node);
            node.belongedLeader = this;
            node.Init();
        }

        public void UnRegister<T>(T node) where T : INode {
            IOCContainer.Remove(typeof(T));
            node.belongedLeader = null;
        }
    }
}