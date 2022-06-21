using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork {
    public delegate void OnEvent<in T>(T e) where T : IEvent;

    public interface IEventDispatcher : ICanAddEventListener, ICanTriggerEvent { }

    public class EventDispatcher : IEventDispatcher {
        private Dictionary<Type, List<object>> EventListeners { get; set; } = new Dictionary<Type, List<object>>();

        public void TriggerEvent<T>() where T : IEvent, new() {
            if (EventListeners.TryGetValue(typeof(T), out var onEvents)) {
                foreach (OnEvent<T> onEvent in onEvents) {
                    onEvent.Invoke(new T());
                }
            }
        }

        public void TriggerEvent<T>(T e) where T : IEvent {
            if (EventListeners.TryGetValue(typeof(T), out var onEvents)) {
                foreach (OnEvent<T> onEvent in onEvents) {
                    onEvent.Invoke(e);
                }
            }
        }

        public IEventRemover AddEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            if (EventListeners.TryGetValue(typeof(T), out var onEvents)) {
                onEvents.Add(onEvent);
            }
            else {
                EventListeners.Add(typeof(T), new List<object> {onEvent});
            }

            return new EventRemover<T> {Dispatcher = this, OnEvent = onEvent};
        }

        public void RemoveEventListener<T>(OnEvent<T> onEvent) where T : IEvent {
            if (EventListeners.TryGetValue(typeof(T), out var onEvents)) {
                onEvents.Remove(onEvent);
            }
        }
    }
    
    public interface IEventRemover {
        void RemoveEventListener();
    }

    public struct EventRemover<T> : IEventRemover where T : IEvent {
        public IEventDispatcher Dispatcher;
        public OnEvent<T> OnEvent;

        public void RemoveEventListener() {
            Dispatcher.RemoveEventListener(OnEvent);
        }
    }

    //自动事件取消器，当销毁时自动触发移除事件监听器
    public class RemoveDestroyerTrigger : MonoBehaviour {
        private readonly HashSet<IEventRemover> Removers = new HashSet<IEventRemover>();

        public void AddRemover(IEventRemover eventRemover) {
            Removers.Add(eventRemover);
        }

        private void OnDestroy() {
            foreach (var remover in Removers) {
                remover.RemoveEventListener();
            }
        }
    }
    
    //静态扩展
    public static class UnregisterExtension {
        public static void UnregisterWhenGameObjectDestroyed(this IEventRemover eventRemover, GameObject gameObject) {
            var trigger = gameObject.GetComponent<RemoveDestroyerTrigger>();

            if (!trigger) {
                trigger = gameObject.AddComponent<RemoveDestroyerTrigger>();
            }

            trigger.AddRemover(eventRemover);
        }
    }
}