using System;
using System.Collections.Generic;

namespace FrameWork {
    public interface IStem : ICanGetSoil, ICanGetRoot, ICanSendNerveWave, ICanRegisterHormone, ICanTriggerHormone { }

    public abstract class AbstractStem : IStem {
        protected AbstractStem() {
            IOCContainer = new Dictionary<Type, INode>();
            HormoneDispatcher = new HormoneDispatcher();
        }

        #region IOC 注册和获取后台组件

        protected readonly Dictionary<Type, INode> IOCContainer;

        private T Get<T>() where T : class, INode {
            if (IOCContainer.TryGetValue(typeof(T), out var component)) {
                return (T) component;
            }

            return null;
        }

        public T GetRoot<T>() where T : class, IRoot {
            return Get<T>();
        }

        public T GetSoil<T>() where T : class, ISoil {
            return Get<T>();
        }

        #endregion

        #region Command

        public void SendNerveWave<T>() where T : INerveWave, new() {
            var command = new T {BelongedStem = this};
            command.Execute();
        }

        public void SendNerveWave<T>(T command) where T : INerveWave {
            command.BelongedStem = this;
            command.Execute();
        }

        #endregion

        #region Event

        private HormoneDispatcher HormoneDispatcher { get; }

        public IUnReceiver ReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            return HormoneDispatcher.ReceiveHormone(onHormone);
        }

        public void UnReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            HormoneDispatcher.UnReceiveHormone(onHormone);
        }

        public void ReleaseHormone<T>() where T : IHormone, new() {
            HormoneDispatcher.ReleaseHormone<T>();
        }

        public void ReleaseHormone<T>(T e) where T : IHormone {
            HormoneDispatcher.ReleaseHormone(e);
        }

        #endregion
    }

    public class Stem : AbstractStem {
        public void Register<T>(T organ) where T : INode {
            IOCContainer.Add(typeof(T), organ);
            organ.BelongedStem = this;
            organ.Init();
        }

        public void UnRegister<T>(T organ) where T : INode {
            IOCContainer.Remove(typeof(T));
            organ.BelongedStem = null;
        }
    }
}