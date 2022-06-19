using System;

namespace FrameWork {
    public interface IOperation : INode, ICanGetOperator, ICanGetModel, ICanAddEventListener, ICanTriggerEvent { }

    public abstract class AbstractOperation : IOperation {
        public ILeader belongedLeader { get; set; }
        public abstract void Init();


        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            return belongedLeader.AddEventListener(onEvent);
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            belongedLeader.RemoveEventListener(onEvent);
        }

        public void TriggerEvent<T>() where T : IEvent, new() {
            belongedLeader.TriggerEvent<T>();
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            belongedLeader.TriggerEvent(e);
        }

        public T GetOperation<T>() where T : class, IOperation {
            return belongedLeader.GetOperation<T>();
        }

        public T GetModel<T>() where T : class, IModel {
            return belongedLeader.GetModel<T>();
        }
    }


    public interface ICanGetOperator {
        T GetOperation<T>() where T : class, IOperation;
    }
}