using System;
using System.Collections.Generic;
using UnityEngine;

namespace FrameWork {
    public delegate void OnHormone<in T>(T hormone) where T : IHormone;

    public interface IHormoneDispatcher : ICanRegisterHormone, ICanTriggerHormone { }

    public class HormoneDispatcher : IHormoneDispatcher {
        private Dictionary<Type, List<object>> HormoneListener { get; set; } = new Dictionary<Type, List<object>>();

        public void ReleaseHormone<T>() where T : IHormone, new() {
            if (HormoneListener.TryGetValue(typeof(T), out var onHormones)) {
                foreach (OnHormone<T> onHormone in onHormones) {
                    onHormone.Invoke(new T());
                }
            }
        }

        public void ReleaseHormone<T>(T e) where T : IHormone {
            if (HormoneListener.TryGetValue(typeof(T), out var onHormones)) {
                foreach (OnHormone<T> onHormone in onHormones) {
                    onHormone.Invoke(e);
                }
            }
        }

        public IUnReceiver ReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            if (HormoneListener.TryGetValue(typeof(T), out var onEvents)) {
                onEvents.Add(onHormone);
            }
            else {
                HormoneListener.Add(typeof(T), new List<object> {onHormone});
            }

            return new UnReceiver<T> {Dispatcher = this, OnHormone = onHormone};
        }

        public void UnReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            if (HormoneListener.TryGetValue(typeof(T), out var onEvents)) {
                onEvents.Remove(onHormone);
            }
        }
    }
    
    public interface IUnReceiver {
        void UnReceive();
    }

    public struct UnReceiver<T> : IUnReceiver where T : IHormone {
        public IHormoneDispatcher Dispatcher;
        public OnHormone<T> OnHormone;

        public void UnReceive() {
            Dispatcher.UnReceiveHormone(OnHormone);
        }
    }

    public class UnReleaseDestroyerTrigger : MonoBehaviour {
        private readonly HashSet<IUnReceiver> UnReceiverSet = new HashSet<IUnReceiver>();

        public void AddUnReceiver(IUnReceiver unReceiver) {
            UnReceiverSet.Add(unReceiver);
        }

        private void OnDestroy() {
            foreach (var unregister in UnReceiverSet) {
                unregister.UnReceive();
            }
        }
    }
    
    //静态扩展
    public static class UnregisterExtension {
        public static void UnregisterWhenGameObjectDestroyed(this IUnReceiver unReceiver, GameObject gameObject) {
            var trigger = gameObject.GetComponent<UnReleaseDestroyerTrigger>();

            if (!trigger) {
                trigger = gameObject.AddComponent<UnReleaseDestroyerTrigger>();
            }

            trigger.AddUnReceiver(unReceiver);
        }
    }
}