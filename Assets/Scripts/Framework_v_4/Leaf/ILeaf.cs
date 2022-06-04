using UnityEngine;

namespace FrameWork {
    public interface ILeaf : IBelongedStem, ICanSendNerveWave, ICanRegisterHormone { }

    public abstract class AbstractLeaf : MonoBehaviour, ILeaf {
        public void SendNerveWave<T>() where T : INerveWave, new() {
            BelongedStem.SendNerveWave<T>();
        }

        public void SendNerveWave<T>(T command) where T : INerveWave {
            BelongedStem.SendNerveWave(command);
        }

        public IUnReceiver ReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            return BelongedStem.ReceiveHormone(onHormone);
        }

        public void UnReceiveHormone<T>(OnHormone<T> onHormone) where T : IHormone {
            BelongedStem.UnReceiveHormone(onHormone);
        }

        public IStem BelongedStem { get; set; }
    }
}