using System;
using System.Collections.Generic;

namespace FrameWork {
    public class IOCContainer<TComponent> where TComponent : class {
        private readonly Dictionary<Type, TComponent> Container = new Dictionary<Type, TComponent>();

        public T Get<T>() where T : class, TComponent {
            if (Container.TryGetValue(typeof(T), out var component)) {
                return (T) component;
            }

            return null;
        }

        public void Add<T>(T component) where T : class, TComponent {
            if (Container.ContainsKey(typeof(T)))
                Container[typeof(T)] = component;
            else
                Container.Add(typeof(T), component);
        }

        public void Remove<T>() where T : class, TComponent {
            Container.Remove(typeof(T));
        }

        public void RemoveAll() {
            Container.Clear();
        }
    }
}